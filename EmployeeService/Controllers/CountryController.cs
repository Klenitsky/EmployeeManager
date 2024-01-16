using EmployeeService.DAL.Contexts;
using EmployeeService.DAL.Repositories;
using EmployeeService.DAL.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeeService.Controllers
{
    [Route("api/EmployeeService/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        CountryRepo _countryRepo;

        public CountryController(ApplicationDbContext dbContext) 
        {
            _countryRepo= new CountryRepo(dbContext);
        }


        [HttpGet("")]
        public ActionResult<IEnumerable<Currency>> GetAll()
        {
            try
            {
                return Ok(_countryRepo.GetAll());
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet("Find/{id}")]
        public ActionResult<Currency> Find(int id)
        {
            Country answer = _countryRepo.Find(id);
            if (answer != null)
            {
                return Ok(answer);
            }
            else
            {
                return NotFound();
            }
        }


        [HttpPost("Add")]
        public IActionResult Add(Country country)
        {
            try
            {
                _countryRepo.Add(country);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return CreatedAtAction(nameof(Find), new { id = country.Id }, country);

        }


        [HttpPost("AddRange")]
        public IActionResult AddRange(IEnumerable<Country> countries)
        {
            try
            {
                _countryRepo.AddRange(countries);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return CreatedAtAction(nameof(GetAll), countries);
        }

        [HttpPut("Update")]
        public IActionResult Update(Country country)
        {
            try
            {
                _countryRepo.Update(country);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpPut("UpdateRange")]
        public IActionResult UpdateRange(IEnumerable<Country> countries)
        {
            try
            {
                _countryRepo.UpdateRange(countries);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }


        [HttpDelete("Delete")]
        public IActionResult Delete(Country country)
        {
            try
            {
                _countryRepo.Delete(country);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpDelete("DeleteRange")]
        public IActionResult DeleteRange(IEnumerable<Country> countries)
        {
            try
            {
                _countryRepo.DeleteRange(countries);
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
                _countryRepo.Delete(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
    }
}
