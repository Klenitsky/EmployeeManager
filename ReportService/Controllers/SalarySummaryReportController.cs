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
            try
            {
                _salarySummaryReportBuilder.SetDate(args.Date);
                _salarySummaryReportBuilder.IncludeOfficeParams(args.IncludedOffices);
                _salarySummaryReportBuilder.IncludeFullCountriesParams(args.IncludedCountries);
                return Ok();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(503);
            }

        }

        [HttpGet("")]
        public IActionResult GetOnDateRange()
        {
            try 
            { 
                _salarySummaryReportBuilder.LoadData();
                _salarySummaryReportBuilder.CountMetrics();
                return Ok(_salarySummaryReportBuilder.GetResult());
            }
            catch (Exception ex)
            {
                return StatusCode(503);
            }
        }
    }
}
