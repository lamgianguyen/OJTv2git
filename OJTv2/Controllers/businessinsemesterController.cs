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
    [Route("api/v1/business-in-semester")]
    [ApiVersion("1.0")]
    [ApiController]
    [AllowAnonymous]
    public class businessinsemesterController : ControllerBase
    {
        private readonly SWPOJTContext _context;

        public businessinsemesterController(SWPOJTContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "2")]
        [SwaggerOperation(Summary = "Get list Business in Student Home page")]
        
        public async Task<ActionResult> getlistBusinesHome()
        {
            try
            {
                List<BusinessSThome> list = new List<BusinessSThome>();
                var bis = await _context.BusinessinSemesters.ToListAsync();

                foreach (var b in bis)
                {
                    var business = await _context.Businesses.FindAsync(b.BusinessId);
                    var jobpost = await _context.JobPositions.FindAsync(b.JobPositionId);
                    if(jobpost != null)
                    {
                        BusinessSThome businesshome = new BusinessSThome
                        {
                            BusinessId = business.BusinessId,
                            BusinessName = business.BusinessName,
                            Image = business.Image,
                            JobPositionId = jobpost.JobPositionId,
                            Salary = jobpost.Salary,
                            WorkLocation = jobpost.WorkLocation,
                            Amount = jobpost.Amount,
                        };
                        list.Add(businesshome);
                    }                 
                }
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

        [HttpGet("job-post/{id}")]
        [Authorize(Roles = "2")]
        [SwaggerOperation(Summary = "Get Jobpost by id")]
       
        public async Task<IActionResult> GetJobPost(int id)
        {
            try
            {
                var jobPosition = await _context.JobPositions.FindAsync(id);
                if(jobPosition == null)
                    return NotFound();
                List<string> list = new List<string>();
                JobpostSThome jobpostSThome = new JobpostSThome
                {
                    JobPositionId = jobPosition.JobPositionId,
                    JobName = jobPosition.JobName,
                    DetailWork= jobPosition.DetailWork,
                    Request= jobPosition.Request,
                    Salary= jobPosition.Salary,
                    WorkLocation= jobPosition.WorkLocation,
                    DetailBusiness= jobPosition.DetailBusiness,
                    Benefit = jobPosition.Benefit,
                    Amount= jobPosition.Amount,                
                };
                var jobdepartment = await _context.JobDepartments.Where(c => c.JobPostionId == id).ToListAsync();
                if (jobdepartment != null) {
                    foreach (var j in jobdepartment)
                    {
                        var department = await _context.Departments.FindAsync(j.DepartmentId);
                        if (department != null) { list.Add(department.DepartmentName); }
                        
                    }
                }
                jobpostSThome.jobDepartmentsName = list;
                   
                return Ok(jobpostSThome);
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
