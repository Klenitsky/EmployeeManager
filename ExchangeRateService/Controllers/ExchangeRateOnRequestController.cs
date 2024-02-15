using ExchangeRateService.DAL.BasicStructures.Models;
using ExchangeRateService.DAL.DatabaseStructures.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExchangeRateService.Controllers
{
    [Route("api/ExchangeRate")]
    [ApiController]
    public class ExchangeRateOnRequestController : ControllerBase
    {

        private IExchangeRateRepo _repo;

        public ExchangeRateOnRequestController(IExchangeRateRepo repo)
        {
            _repo = repo;
        }

        [HttpGet("OnDate")]
        public IActionResult GetByDate(DateTime Date)
        {
            try
            {
                return Ok(_repo.GetExchangeRatesByDateOnRequest(Date));
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(503);
            }

        }

        [HttpGet("OnDateRange")]
        public IActionResult GetByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                return Ok(_repo.GetExchangeRatesByDateRangeOnRequest(startDate,endDate));
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(503);
            }

        }

        [HttpGet("OnDateAndCurrency")]
        public IActionResult GetByDateAndCurrency(DateTime date, string currencyAbbreviation)
        {
            try
            {
                return Ok(_repo.GetExchangeRateByDateAndCurrencyOnRequest(currencyAbbreviation,date));
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

        [HttpGet("OnDateRangeAndCurrency")]
        public IActionResult GetByDateRangeAndCurrency(DateTime startDate, DateTime endDate, string currencyAbbreviation)
        {
            try
            {
                return Ok(_repo.GetExchangeRatesByDateRangeAndCurrencyOnRequest(currencyAbbreviation,startDate, endDate));
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
    }
}
