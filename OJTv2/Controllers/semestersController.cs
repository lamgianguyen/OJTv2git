using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OJTv2.Models;
using OJTv2.Requests;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OJTv2.Controllers
{
    
    [Route("api/v1/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [AllowAnonymous]
    public class semestersController : ControllerBase
    {
        private readonly SWPOJTContext _context;

        public semestersController(SWPOJTContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        [SwaggerOperation(Summary = "Create Semester")]
        public async Task<ActionResult<Semester>> CreateSemester(semestercreate semesterrequest)
        {
            try {
                Semester semester = new Semester
                {
                    SemesterId = semesterrequest.SemesterId,
                    SemesterName = semesterrequest.SemesterName,
                    StartDate = semesterrequest.StartDate,
                    EndDate = semesterrequest.EndDate,
                    StatusId = semesterrequest.StatusId,

                };
                _context.Semesters.Add(semester);
               await _context.SaveChangesAsync();
                return Ok(semester);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal server exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }



        }
        [HttpGet]
        [Authorize(Roles = "1")]
        [SwaggerOperation(Summary = "Get list Semesters")]
        public async Task<ActionResult<List<Semester>>> GetListSemester()
        {
            try
            {
                List<SemesterRequest> list2 = new List<SemesterRequest>();
                List<Semester> list = await _context.Semesters.ToListAsync();
                foreach (var semester in list)
                {
                    SemesterRequest semesterRequest = new SemesterRequest
                    {
                        SemesterId = semester.SemesterId,
                        SemesterName = semester.SemesterName,
                        StartDate = semester.StartDate,
                        EndDate = semester.EndDate,

                    };
                    if (semester.StatusId == 1)
                    {
                        semesterRequest.StatusId = "Past";
                    }
                    if (semester.StatusId == 2)
                    {
                        semesterRequest.StatusId = "On going";
                    }
                    if(semester.StatusId == 3)
                    {
                        semesterRequest.StatusId = "Future";
                    }
                    list2.Add(semesterRequest);

                }
                return Ok(list2);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal server exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }

        }
        [HttpGet("{id}")]
        [Authorize(Roles = "1")]
        [SwaggerOperation(Summary = "Get Semester by id")]
        public async Task<ActionResult<Semester>> GetSemester(string id)
        {
            try
            {
                var semester = await _context.Semesters.FindAsync(id);
                if (semester == null)
                    return NotFound();
                return Ok(semester);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal server exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }



        }
        [HttpPut("{id}")]
        [Authorize(Roles = "1")]
        [SwaggerOperation(Summary = "Update Semester by id")]
        public async Task<IActionResult> UpdateSemester(string id,Semester semester)
        {
            if (id != semester.SemesterId)
                return NotFound();
            try
            {
                _context.Entry(semester).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal server exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }

        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "1")]
        [SwaggerOperation(Summary = "Delete Semester by id")]
        public async Task<IActionResult> DeleteSemester(string id)
        {
            var semester = await _context.Semesters.FindAsync(id);
            try
            {
                _context.Semesters.Remove(semester);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal server exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }

        }
    }


     

}
