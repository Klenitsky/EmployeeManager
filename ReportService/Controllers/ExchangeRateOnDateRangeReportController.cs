using EmployeeService.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using ReportService.Structures.ParameterModels;
using ReportService.Structures.Reports.ExchangeRateOnDateRange;
using ReportService.Structures.Reports.PaymentOnDateRange;

namespace ReportService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRateOnDateRangeReportController : ControllerBase
    {

        ExchangeRateOnDateRangeReportBuilder _exchangeRateOnDateRangeReportBuilder;

        public ExchangeRateOnDateRangeReportController(ExchangeRateOnDateRangeReportBuilder builder)
        {
            _exchangeRateOnDateRangeReportBuilder = builder;
            
        }

        [HttpPost]
        public IActionResult SetParameters(ExchangeRateParametersModel args)
        {
            if (args.StartDate > args.EndDate)
            {
                throw new ArgumentOutOfRangeException();
            }
            _exchangeRateOnDateRangeReportBuilder.SetProperties(args.StartDate, args.EndDate, args.IncludedCurrencies);
            return Ok();
        }

        [HttpGet("")]
        public IActionResult GetOnDateRange()
        {
            try
            {
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
