using BenchmarkDotNet.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OJTv2.Models;
using OJTv2.Requests;
using Prometheus;
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

    public class applysController : ControllerBase
    {


        private readonly SWPOJTContext _context;

        public applysController(SWPOJTContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "1")]
        [SwaggerOperation(Summary = "Get list Apply in admin")]


        public async Task<ActionResult<List<Apply>>> GetlistapplyinJobPost()
        {
            List<ApplyStudent> list2 = new List<ApplyStudent>();
            List<Apply> list = await _context.Applies.ToListAsync();
            foreach (var apply in list)
            {
                var student = await _context.Students.FindAsync(apply.StudentId);
                var user = await _context.Users.FindAsync(apply.StudentId);
                var business = await _context.Businesses.FindAsync(apply.BusinessId);
                string status = "";
                if (apply.StatusApply == 1)
                {
                    status = "waiting";
                }
                else if (apply.StatusApply == 2)
                {
                    status = "Approve";
                }
                else if (apply.StatusApply == 3)
                {
                    status = "Fail";
                }
                ApplyStudent applyStudent = new ApplyStudent
                {
                    StudentId = student.StudentId,
                    StudentName = student.StudentName,
                    ApplyDate = apply.ApplyDate,
                    Email = user.Email,
                    BusinessName = business.BusinessName,
                    Status = status
                };
                list2.Add(applyStudent);
            }

            return Ok(list2);
            ;
        }
        [HttpGet("business/{id}")]
        [Authorize(Roles = "3")]
        [SwaggerOperation(Summary = "Get list Apply  in jobpost")]
        public async Task<ActionResult<List<Apply>>> Getlistapply(string id)
        {
            try
            {
                var business = await _context.Businesses.FindAsync(id);
                if (business == null)
                    return NotFound();
                List<Apply> list = await _context.Applies.Where(b => b.BusinessId.Equals(id)).ToListAsync();
                List<ApplyinJobPost> list2 = new List<ApplyinJobPost>();
                foreach (Apply apply in list)
                {
                    var student = await _context.Students.FindAsync(apply.StudentId);
                    ApplyinJobPost applyrespone = new ApplyinJobPost
                    {
                        ApplyId = apply.ApplyId,
                        StudentName = student.StudentName,
                        StudentId = apply.StudentId,
                        ApplyDate = apply.ApplyDate,
                        BusinessId = apply.BusinessId,
                        Cv = apply.Cv,
                        StatusApply = apply.StatusApply
                    };
                    if(applyrespone.StatusApply == 1||applyrespone.StatusApply==2)
                    {
                        list2.Add(applyrespone);
                    }                    
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
        [Authorize(Roles = "2")]
        [SwaggerOperation(Summary = "Get Apply by Student id")]
        public async Task<ActionResult<Apply>> GetApply(string id)
        {
            var apply = await _context.Applies.Where(c => c.StudentId == id).FirstOrDefaultAsync();
            if (apply == null)
                return NotFound();
            try
            {
                return Ok(apply);
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
        [HttpPut("{id}/status")]
        [Authorize(Roles = "3")]
        [SwaggerOperation(Summary = "Update Apply status")]
        public async Task<IActionResult> UpdateApplyStatus(applystatus request,int id)
        {
            if (request.id != id)
                return BadRequest();
            var apply = await _context.Applies.FindAsync(request.id);
            if (apply == null)
                return NotFound();
            apply.StatusApply = request.status;
            if (apply.StatusApply == 2)
            {
                _context.Entry(apply).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok("Approve success");

            }
            else if (apply.StatusApply == 3)
            {
                _context.Entry(apply).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Unapprove success");
            }
            else if (apply.StatusApply == 1)
            {
                return Ok();
            }
            else
            {
                return BadRequest("wrong status");
            }
        }

        [HttpPost("{id}")]
        [Authorize(Roles = "2")]
        [SwaggerOperation(Summary = "Create Apply by student into jobpostID")]
        
        public async Task<ActionResult<Apply>> CreateAplly([FromBody]ApplyRequest request,int id)
        { 
            
            try
            {
                var businessinses = await _context.BusinessinSemesters.Where(c=>c.JobPositionId==id).FirstOrDefaultAsync();
                if(businessinses == null)
                {
                    return NotFound();
                }
               var applycheck =  await _context.Applies.Where(c => c.StudentId == request.StudentId).FirstOrDefaultAsync();
                if(applycheck != null)
                {
                    return BadRequest("Just 1 apply in semester");
                };
                Apply apply = new Apply
                {
                    StudentId = request.StudentId,
                    ApplyDate = DateTime.Now,
                    BusinessId = businessinses.BusinessId,
                    Cv = request.Cv,
                    StatusApply = 1                    
                };
                _context.Applies.Add(apply);
                await _context.SaveChangesAsync();
                return Ok(apply);
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
        [Authorize(Roles = "2")]
        [SwaggerOperation(Summary = "Update Apply by id")]
        
        public async Task<IActionResult> UpdateApply(int id,CVRequest request)
        {
            if (id != request.id)
                return BadRequest();
            var apply = await _context.Applies.FindAsync(id);
            if(apply == null)
                return NotFound();
           
            try
            {
                apply.Cv= request.CV;
                _context.Entry(apply).State = EntityState.Modified;
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
        [SwaggerOperation(Summary = "Delete Apply by id")]
        [Authorize(Roles = "2")]
        
        public async Task<IActionResult> DeleteApply(int id)
        {
            var apply = await _context.Applies.FindAsync(id);
            try
            {
                _context.Applies.Remove(apply);
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
