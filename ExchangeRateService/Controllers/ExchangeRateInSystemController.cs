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

        [HttpGet("rates")]
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


        [HttpGet("currencies")]
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
        ActionResult<IEnumerable<ExchangeRate>> GetRatesByCurrency(string currencyAbbreviation)
        {
            try
            {
                return Ok(_repo.GetExchangeRatesByCurrency(currencyAbbreviation));
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
    }
}
