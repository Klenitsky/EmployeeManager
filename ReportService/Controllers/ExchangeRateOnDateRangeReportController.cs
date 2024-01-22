using EmployeeService.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using ReportService.Structures.Reports.ExchangeRateOnDateRange;

namespace ReportService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRateOnDateRangeReportController : ControllerBase
    {

        private string _exchangeRateApiConnectionString = "https://localhost:44341/";

        ExchangeRateOnDateRangeReportBuilder _exchangeRateOnDateRangeReportBuilder;

        public ExchangeRateOnDateRangeReportController()
        {
            _exchangeRateOnDateRangeReportBuilder = new ExchangeRateOnDateRangeReportBuilder(_exchangeRateApiConnectionString);
            
        }

        [HttpGet("")]
        ActionResult<ExchangeRateOnDateRangeReport> GetOnDateRange(DateTime startDate, DateTime endDate, IEnumerable<Currency> currencies)
        {
            try
            {
                if(startDate> endDate)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _exchangeRateOnDateRangeReportBuilder.SetProperties(startDate, endDate,currencies);
                _exchangeRateOnDateRangeReportBuilder.LoadData();
                _exchangeRateOnDateRangeReportBuilder.CountMetrics();
                return Ok(_exchangeRateOnDateRangeReportBuilder.GetResult());
            }
            catch(ArgumentOutOfRangeException ex)
            {
                return BadRequest("Date Period is not valid");
            }
        }
    }
}
