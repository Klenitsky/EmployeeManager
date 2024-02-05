using EmployeeService.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReportService.Structures.ParameterModels;
using ReportService.Structures.Reports.ExchangeRateOnDateRange;
using ReportService.Structures.Reports.PaymentOnDateRange;
using ReportService.Structures.Reports.SalarySummary;

namespace ReportService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentOnDateRangeReportController : ControllerBase
    {

       PaymentOnDateRangeReportBuilder _paymentOnDateRangeReportBuilder;

        public PaymentOnDateRangeReportController(PaymentOnDateRangeReportBuilder builder)
        {
            _paymentOnDateRangeReportBuilder = builder;


        }

        [HttpPost]
        public IActionResult SetParameters(PaymentParametersModel args)
        {
            _paymentOnDateRangeReportBuilder.SetDateRange(args.StartDate, args.EndDate);
            _paymentOnDateRangeReportBuilder.IncludeOfficeParams(args.IncludedOffices);
            _paymentOnDateRangeReportBuilder.IncludeFullCountriesParams(args.IncludedCountries);
            return Ok();
        }

        [HttpGet("")]
        public ActionResult<PaymentOnDateRangeReport> GetOnDateRange()
        {  
            _paymentOnDateRangeReportBuilder.LoadData();
            _paymentOnDateRangeReportBuilder.CountMetrics();
            return Ok(_paymentOnDateRangeReportBuilder.GetResult());
        }
    }
}
