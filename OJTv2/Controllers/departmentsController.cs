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
using System.Linq;
using System.Threading.Tasks;

namespace OJTv2.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [AllowAnonymous]

    public class departmentsController : ControllerBase
    {
        private readonly SWPOJTContext _context;

        public departmentsController(SWPOJTContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "1,2,3")]
        [SwaggerOperation(Summary = "Get list Departments")]
        
        public async Task<ActionResult<List<Department>>> GetListDepartment()
        {
            try
            {
                var list = await _context.Departments.ToListAsync();
                return Ok(list);
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


        [HttpPost]
        [Authorize(Roles = "1")]
        [SwaggerOperation(Summary = "Create Department")]
        
        public async Task<ActionResult<Department>> CreateDepartment(DepartmentRequest request)
        {

            try
            {
                Department department = new Department
                {
                    DepartmentId = request.id,
                    DepartmentName = request.name,
                };
                _context.Departments.Add(department);
                await _context.SaveChangesAsync();
                return Ok(department);
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
        [SwaggerOperation(Summary = "Get Department by id")]
        
        public async Task<ActionResult<Department>> GetDepartment(string id)
        {
            try
            {
                var department = await _context.Departments.FindAsync(id);
                return Ok(department);
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
        [SwaggerOperation(Summary = "Delete Department by id")]
        
        public async Task<IActionResult> DeleteDepartment(string id)
        {
            try
            {
                var department = await _context.Departments.FindAsync(id);
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
                return Ok(department);
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
        [SwaggerOperation(Summary = "Update Department by id")]
        
        public async Task<IActionResult> UpdateDepartment(string id,Department department)
        {
            if (id != department.DepartmentId)
                return NotFound();
            try
            {
                _context.Entry(department).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(department);
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
