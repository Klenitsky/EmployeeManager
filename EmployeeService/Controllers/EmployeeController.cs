using EmployeeService.DAL.Contexts;
using EmployeeService.DAL.Repositories;
using EmployeeService.DAL.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeeService.Controllers
{
    [Route("api/EmployeeService/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
       
        private EmployeeRepo _employeeRepo;

        public EmployeeController(ApplicationDbContext dbContext)
        {
            _employeeRepo = new EmployeeRepo(dbContext);
        }


        [HttpGet]
        [Route("")]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_employeeRepo.GetAll());
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet("Find/{id}")]
        public IActionResult Find(int id)
        {
            Employee answer = _employeeRepo.Find(id);
            if (answer != null)
            {
                return Ok(answer);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("FindByOffice/{officeId}")]
        public IActionResult FindByOffice(int officeId)
        {
            IEnumerable<Employee> answer = new List<Employee>();
            try
            {
               answer = _employeeRepo.GetAllByOffice(officeId);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(503);
            }

            if (answer != null && answer.Count() != 0 )
            {
                return Ok(answer);
            }
            else
            {
                return NotFound();
            }
        }


        [HttpPost("Add")]
        public IActionResult Add(Employee employee)
        {
            try
            {
                _employeeRepo.Add(employee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return CreatedAtAction(nameof(Find), new { id = employee.Id }, employee);

        }


        [HttpPost("AddRange")]
        public IActionResult AddRange(IEnumerable<Employee> employees)
        {
            try
            {
                _employeeRepo.AddRange(employees);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return CreatedAtAction(nameof(GetAll), employees);
        }

        [HttpPut("Update")]
        public IActionResult Update(Employee employee)
        {
            try
            {
                _employeeRepo.Update(employee);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(503);
            }
            return Ok();
        }

        [HttpPut("UpdateRange")]
        public IActionResult UpdateRange(IEnumerable<Employee> employees)
        {
            try
            {
                _employeeRepo.UpdateRange(employees);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(503);
            }
            return Ok();
        }


        [HttpDelete("Delete")]
        public IActionResult Delete(Employee employees)
        {
            try
            {
                _employeeRepo.Delete(employees);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(503);
            }
            return Ok();
        }

        [HttpDelete("DeleteRange")]
        public IActionResult DeleteRange(IEnumerable<Employee> employees)
        {
            try
            {
                _employeeRepo.DeleteRange(employees);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(503);
            }
            return Ok();
        }

        [HttpDelete("DeleteById/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _employeeRepo.Delete(id);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(503);
            }
            return Ok();
        }

    }
}
