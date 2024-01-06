using EmployeeService.DAL.Contexts;
using EmployeeService.DAL.Repositories;
using EmployeeService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeService.Controllers
{
    [Route("api/EmployeeService/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private static string[] args = { };
        CurrencyRepo currencyRepo = new CurrencyRepo(new ApplicationDbContextFactory().CreateDbContext(args));


        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<Currency>> GetAll()
        {
            try
            {
                return Ok(currencyRepo.GetAll());
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet("find/{id}")]
        public ActionResult<Currency> Find(int id)
        {
           Currency answer = currencyRepo.Find(id);
            if(answer != null)
            {
                return Ok(answer);
            }
            else
            {
                return NotFound();
            }
        }


        [HttpPost("Add")]
        public IActionResult Add(Currency currency) 
        {
            try
            {
                currencyRepo.Add(currency);
            }
            catch (Exception ex) 
            { 
                return BadRequest(ex.Message);
            }
            return CreatedAtAction(nameof(Find), new { id = currency.Id }, currency);

        }


        [HttpPost("AddRange")]
        public IActionResult AddRange(IEnumerable<Currency> currencies)
        {
            try
            {
                currencyRepo.AddRange(currencies);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return CreatedAtAction(nameof(GetAll),currencies);
        }

        [HttpPut("Update")]
        public IActionResult Update(Currency currency)
        {
            try
            {
                currencyRepo.Update(currency);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpPut("UpdateRange")]
        public IActionResult UpdateRange(IEnumerable<Currency> currencies)
        {
            try
            {
                currencyRepo.UpdateRange(currencies);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }


        [HttpDelete("Delete")]
        public IActionResult Delete(Currency currency)
        {
            try
            {
                currencyRepo.Delete(currency);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpDelete("DeleteRange")]
        public IActionResult DeleteRange(IEnumerable<Currency> currency)
        {
            try
            {
                currencyRepo.DeleteRange(currency);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpDelete("DeleteById/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                currencyRepo.Delete(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

    }
}
