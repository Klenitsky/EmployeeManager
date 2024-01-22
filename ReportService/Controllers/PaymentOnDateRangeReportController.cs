using EmployeeService.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReportService.Structures.Reports.ExchangeRateOnDateRange;
using ReportService.Structures.Reports.PaymentOnDateRange;
using ReportService.Structures.Reports.SalarySummary;

namespace ReportService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentOnDateRangeReportController : ControllerBase
    {

        private string _exchangeRateApiConnectionString = "https://localhost:44341/";
        private string _EmployeeApiConnectionString = "https://localhost:7054/";

       PaymentOnDateRangeReportBuilder _paymentOnDateRangeReportBuilder;

        public PaymentOnDateRangeReportController()
        {
            _paymentOnDateRangeReportBuilder = new PaymentOnDateRangeReportBuilder(_EmployeeApiConnectionString, _exchangeRateApiConnectionString);


        }

        [HttpGet("")]
        ActionResult<PaymentOnDateRangeReport> GetOnDateRange(DateTime startDate,DateTime endDate, IEnumerable<Office> includedOffices, IEnumerable<Country> includedCountries)
        {
            _paymentOnDateRangeReportBuilder.SetDateRange(startDate, endDate);
            _paymentOnDateRangeReportBuilder.IncludeOfficeParams(includedOffices);
            _paymentOnDateRangeReportBuilder.IncludeFullCountriesParams(includedCountries);
            _paymentOnDateRangeReportBuilder.LoadData();
            _paymentOnDateRangeReportBuilder.CountMetrics();
            return Ok(_paymentOnDateRangeReportBuilder.);
        }
    }
}
