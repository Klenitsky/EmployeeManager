using EmployeeService.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using ReportService.Structures.Reports.SalarySummary;

namespace ReportService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalarySummaryReportController : ControllerBase
    {

        private string _exchangeRateApiConnectionString = "https://localhost:44341/";
        private string _EmployeeApiConnectionString = "https://localhost:44316/";

        SalarySummaryReportBuilder _salarySummaryReportBuilder;

        public SalarySummaryReportController()
        {
            _salarySummaryReportBuilder = new SalarySummaryReportBuilder(_EmployeeApiConnectionString, _exchangeRateApiConnectionString);

            
        }

        [HttpGet("")]
        ActionResult<SalarySummaryReport> GetOnDateRange(DateTime date, IEnumerable<Office> includedOffices, IEnumerable<Country> includedCountries)
        {
                _salarySummaryReportBuilder.SetDate(date);
                _salarySummaryReportBuilder.IncludeOfficeParams(includedOffices);
                _salarySummaryReportBuilder.IncludeFullCountriesParams(includedCountries);
                _salarySummaryReportBuilder.LoadData();
                _salarySummaryReportBuilder.CountMetrics();
                return Ok(_salarySummaryReportBuilder.GetResult());
        }
    }
}
