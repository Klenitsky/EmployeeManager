using EmployeeService.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using ReportService.Structures.ParameterModels;
using ReportService.Structures.Reports.PaymentOnDateRange;
using ReportService.Structures.Reports.SalarySummary;

namespace ReportService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalarySummaryReportController : ControllerBase
    {

        SalarySummaryReportBuilder _salarySummaryReportBuilder;

        public SalarySummaryReportController(SalarySummaryReportBuilder builder)
        {
            _salarySummaryReportBuilder = builder;

            
        }

        [HttpPost]
        public IActionResult SetParameters(SalarySummaryParametersModel args)
        {
            _salarySummaryReportBuilder.SetDate(args.Date);
            _salarySummaryReportBuilder.IncludeOfficeParams(args.IncludedOffices);
            _salarySummaryReportBuilder.IncludeFullCountriesParams(args.IncludedCountries);
            return Ok();
        }

        [HttpGet("")]
        public ActionResult<SalarySummaryReport> GetOnDateRange()
        {
                _salarySummaryReportBuilder.LoadData();
                _salarySummaryReportBuilder.CountMetrics();
                return Ok(_salarySummaryReportBuilder.GetResult());
        }
    }
}
