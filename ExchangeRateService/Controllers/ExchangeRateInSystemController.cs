using EmployeeService.DAL.Models;
using ExchangeRateService.DAL.BasicStructures.Models;
using ExchangeRateService.DAL.DatabaseStructures.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExchangeRateService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRateInSystemController : ControllerBase
    {
        private IExchangeRateRepo _repo;

        public ExchangeRateInSystemController(IExchangeRateRepo repo)
        {
            _repo = repo;
        }

        [HttpGet("Rates")]
        public IActionResult GetExchangeRates()
        {
            try
            {
                return Ok(_repo.GetExchangeRates());
            }
            catch (Exception ex)
            {
                return StatusCode(503);
            }
        }


        [HttpGet("Currencies")]
        public IActionResult GetActiveCurrencies()
        {
            try
            {
                return Ok(_repo.GetActiveCurrencies());
            }
            catch (Exception ex)
            {
                return StatusCode(503);
            }
        }

        [HttpGet("OnCurrency")]
        public IActionResult GetRatesByCurrency(string currencyAbbreviation)
        {
            try
            {
                return Ok(_repo.GetExchangeRatesByCurrencyInSystem(currencyAbbreviation));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(503);
            }
        }

        [HttpPost("LoadCurrencies")]
        public IActionResult Add()
        {
            try
            {
                _repo.LoadActiveCurrencies();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(503);
            }
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
                return Ok(_repo.GetExchangeRatesByDateRangeInSystem(startDate, endDate));
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
                return Ok(_repo.GetExchangeRateByDateAndCurrencyInSystem(currencyAbbreviation, date));
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
                return Ok(_repo.GetExchangeRatesByDateRangeAndCurrencyInSystem(currencyAbbreviation, startDate, endDate));
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
