﻿using EmployeeService.DAL.Contexts;
using EmployeeService.DAL.Repositories;
using EmployeeService.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeeService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeController : ControllerBase
    {

        private OfficeRepo _officeRepo;

        public OfficeController(ApplicationDbContext dbContext)
        {
            _officeRepo = new OfficeRepo(dbContext);
        }


        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<Office>> GetAll()
        {
            try
            {
                return Ok(_officeRepo.GetAll());
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet("find/{id}")]
        public ActionResult<Office> Find(int id)
        {
            Office answer = _officeRepo.Find(id);
            if (answer != null)
            {
                return Ok(answer);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("FindByCountry/{countryId}")]
        public ActionResult<IEnumerable<Office>> FindByCountry(int countryId)
        {
            IEnumerable<Office> offices = _officeRepo.GetAllByCountry(countryId);
            if (offices != null)
            {
                return Ok(offices);
            }
            else
            {
                return NotFound();
            }
        }
        

        [HttpPost("Add")]
        public IActionResult Add(Office office)
        {
            try
            {
                _officeRepo.Add(office);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return CreatedAtAction(nameof(Find), new { id = office.Id }, office);

        }


        [HttpPost("AddRange")]
        public IActionResult AddRange(IEnumerable<Office> offices)
        {
            try
            {
                _officeRepo.AddRange(offices);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return CreatedAtAction(nameof(GetAll), offices);
        }

        [HttpPut("Update")]
        public IActionResult Update(Office office)
        {
            try
            {
                _officeRepo.Update(office);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpPut("UpdateRange")]
        public IActionResult UpdateRange(IEnumerable<Office> offices)
        {
            try
            {
                _officeRepo.UpdateRange(offices);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }


        [HttpDelete("Delete")]
        public IActionResult Delete(Office office)
        {
            try
            {
                _officeRepo.Delete(office);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpDelete("DeleteRange")]
        public IActionResult DeleteRange(IEnumerable<Office> offices)
        {
            try
            {
                _officeRepo.DeleteRange(offices);
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
                _officeRepo.Delete(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

    }
}