﻿using EmployeeService.DAL.Models;
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
            try
            {
                _exchangeRateOnDateRangeReportBuilder.SetProperties(args.StartDate, args.EndDate, args.IncludedCurrencies);
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
