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
        public ActionResult<IEnumerable<ExchangeRate>> GetExchangeRates()
        {
            try
            {
                return Ok(_repo.GetExchangeRates());
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }


        [HttpGet("Currencies")]
        public ActionResult<IEnumerable<ActiveCurrency>> GetActiveCurrencies()
        {
            try
            {
                return Ok(_repo.GetActiveCurrencies());
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet("OnCurrency")]
        public ActionResult<IEnumerable<ExchangeRate>> GetRatesByCurrency(string currencyAbbreviation)
        {
            try
            {
                return Ok(_repo.GetExchangeRatesByCurrencyInSystem(currencyAbbreviation));
            }
            catch (Exception ex)
            {
                return NotFound();
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
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("OnDate")]
        public ActionResult<IEnumerable<ExchangeRate>> GetByDate(DateTime Date)
        {
            try
            {
                return Ok(_repo.GetExchangeRatesByDateOnRequest(Date));
            }
            catch (Exception ex)
            {
                return NotFound();
            }

        }

        [HttpGet("OnDateRange")]
        public ActionResult<IEnumerable<ExchangeRate>> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                return Ok(_repo.GetExchangeRatesByDateRangeInSystem(startDate, endDate));
            }
            catch (Exception ex)
            {
                return NotFound();
            }

        }

        [HttpGet("OnDateAndCurrency")]
        public ActionResult<ExchangeRate> GetByDateAndCurrency(DateTime date, string currencyAbbreviation)
        {
            try
            {
                return Ok(_repo.GetExchangeRateByDateAndCurrencyInSystem(currencyAbbreviation, date));
            }
            catch (Exception ex)
            {
                return NotFound();
            }

        }

        [HttpGet("OnDateRangeAndCurrency")]
        public ActionResult<IEnumerable<ExchangeRate>> GetByDateRangeAndCurrency(DateTime startDate, DateTime endDate, string currencyAbbreviation)
        {
            try
            {
                return Ok(_repo.GetExchangeRatesByDateRangeAndCurrencyInSystem(currencyAbbreviation, startDate, endDate));
            }
            catch (Exception ex)
            {
                return NotFound();
            }

        }
    }
}
