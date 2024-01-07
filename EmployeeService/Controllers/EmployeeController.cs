using EmployeeService.DAL.Contexts;
using EmployeeService.DAL.Repositories;
using EmployeeService.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeeService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private static string[] args = { };
        EmployeeRepo employeeRepo = new EmployeeRepo(new ApplicationDbContextFactory().CreateDbContext(args));


        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<Employee>> GetAll()
        {
            try
            {
                return Ok(employeeRepo.GetAll());
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet("find/{id}")]
        public ActionResult<Employee> Find(int id)
        {
            Employee answer = employeeRepo.Find(id);
            if (answer != null)
            {
                return Ok(answer);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("findByOffice/{officeId}")]
        public ActionResult<IEnumerable<Employee>> FindByOffice(int officeId)
        {
            IEnumerable<Employee> answer = employeeRepo.GetAllByOffice(officeId);
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
        public IActionResult Add(Employee employee)
        {
            try
            {
                employeeRepo.Add(employee);
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
                employeeRepo.AddRange(employees);
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
                employeeRepo.Update(employee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpPut("UpdateRange")]
        public IActionResult UpdateRange(IEnumerable<Employee> employees)
        {
            try
            {
                employeeRepo.UpdateRange(employees);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }


        [HttpDelete("Delete")]
        public IActionResult Delete(Employee employees)
        {
            try
            {
                employeeRepo.Delete(employees);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpDelete("DeleteRange")]
        public IActionResult DeleteRange(IEnumerable<Employee> employees)
        {
            try
            {
                employeeRepo.DeleteRange(employees);
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
                employeeRepo.Delete(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

    }
}
