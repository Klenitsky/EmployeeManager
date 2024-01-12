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
        public ActionResult<IEnumerable<ExchangeRate>> GetByDate(DateTime Date)
        {
            try
            {
                return Ok(_repo.GetExchangeRatesByDate(Date));
            }
            catch(Exception ex) 
            {
                return NotFound();
            }

        }

        [HttpGet("OnDateRange")]
        public ActionResult<IEnumerable<ExchangeRate>> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                return Ok(_repo.GetExchangeRatesByDateRange(startDate,endDate));
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
                return Ok(_repo.GetExchangeRateByDateAndCurrency(currencyAbbreviation,date));
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
                return Ok(_repo.GetExchangeRateByDateRangeAndCurrency(currencyAbbreviation,startDate, endDate));
            }
            catch (Exception ex)
            {
                return NotFound();
            }

        }
    }
}
