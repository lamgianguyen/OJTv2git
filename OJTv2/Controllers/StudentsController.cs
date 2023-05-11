using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OJTv2.Models;
using OJTv2.Requests;
using Swashbuckle.AspNetCore.Annotations;

namespace OJTv2.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    [AllowAnonymous]
    public class StudentsController : ControllerBase
    {
        private readonly SWPOJTContext _context;

        public StudentsController(SWPOJTContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        [Authorize(Roles = "1")]
        [SwaggerOperation(Summary = "Get list students in admin")]
        public async Task<ActionResult<IEnumerable<StudentRequest>>> GetStudents()
        {
            try { 
            List<Student> students = await _context.Students.ToListAsync();
            List<StudentRequest> studentRequestsList = new List<StudentRequest>();
            foreach (Student student in students)
            {
                StudentRequest studentRequest = new StudentRequest
                {
                    StudentId = student.StudentId,
                    StudentName = student.StudentName,
                    PhoneNumber = student.PhoneNumber,
                    Avatar = student.Avatar,
                    SemesterId = student.SemesterId,
                    DeparmentId = student.DeparmentId,
                };
                studentRequestsList.Add(studentRequest);
            }   
            return Ok(studentRequestsList);
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

        // GET: api/Students/5
        [HttpGet("{id}")]
        [Authorize(Roles = "1,2,3")]
        [SwaggerOperation(Summary = "View detail student by id")]
        public async Task<ActionResult<StudentRequest>> GetStudent(string id)
        {
            try { 
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            StudentRequest request = new StudentRequest
            {
                StudentId = student.StudentId,
                StudentName= student.StudentName,
                PhoneNumber= student.PhoneNumber,
                Avatar = student.Avatar,
                SemesterId= student.SemesterId,
                DeparmentId= student.DeparmentId,
            };          
            return Ok(request);
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

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]

        public async Task<IActionResult> PutStudent(string id, StudentRequest studentRequest)
        {
            if (id != studentRequest.StudentId)
            {
                return BadRequest();
            }
            try { 
                var student = await _context.Students.FindAsync(id);
                student.StudentName = studentRequest.StudentName;
                student.PhoneNumber = studentRequest.PhoneNumber;
                student.Avatar = studentRequest.Avatar;
                
            _context.Entry(student).State = EntityState.Modified;        
                await _context.SaveChangesAsync();
            return Ok("Update success");
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

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _context.Students.Add(student);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StudentExists(student.StudentId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStudent", new { id = student.StudentId }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(string id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(string id)
        {
            return _context.Students.Any(e => e.StudentId == id);
        }
    }
}
