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

namespace OJT.Controllers
{
    
    [Route("api/v1/job-post")]
    [ApiVersion("1.0")]
    [ApiController]
    [AllowAnonymous]
    public class jobPostsController : ControllerBase
    {
        private readonly SWPOJTContext _context;

        public jobPostsController(SWPOJTContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "3")]
        [SwaggerOperation(Summary = "Create jobpost")]

        public async Task<ActionResult<JobPosition>> PostJobPosition(JobPostRequest request)
        {
            try
            {
                var business = await _context.Businesses.FindAsync(request.businessID);
                if (business == null)
                    return BadRequest("Not found");
                var semester = await _context.Semesters.FindAsync(business.SemesterId);
                if (semester == null)
                    return BadRequest("Not found");

                if (semester.StatusId != 2)
                    return BadRequest("Business is not in semester");
                var BusinessinSemester = await _context.BusinessinSemesters.FindAsync(request.businessID);

                if (BusinessinSemester == null)
                    return BadRequest("Business not in Semester!!!");
                if (BusinessinSemester.JobPositionId != null)
                {
                    return BadRequest("Business just have 1 job post");
                }

              //add jobpost
                JobPosition jobPosition = new JobPosition
                {
                    JobName = request.JobName,
                    DetailWork = request.DetailWork,
                    Request = request.Request,
                    Salary = request.Salary,
                    WorkLocation = request.WorkLocation,
                    DetailBusiness = request.DetailBusiness,
                    Benefit = request.Benefit,
                    Amount = request.Amount,
                };
                
                _context.JobPositions.Add(jobPosition);
                await _context.SaveChangesAsync();

                //add list department in to jobpost
                var jobpositionbyname = await _context.JobPositions.FindAsync(jobPosition.JobPositionId);
                List<string> list = request.ListDepartment;
                
                foreach (var departmentid in list)
                {
                    var deparmentid = await _context.Departments.FindAsync(departmentid);
                    if (deparmentid == null)
                    {
                        return NotFound();
                    }
                    
                    JobDepartment jobDepartment = new JobDepartment
                    {
                        JobPostionId = jobpositionbyname.JobPositionId,
                        DepartmentId = deparmentid.DepartmentId,
                    };
                    _context.JobDepartments.Add(jobDepartment);     
                }

                //update jobpost in business semester
                var businessinsemester = await _context.BusinessinSemesters.FindAsync(request.businessID);
                businessinsemester.JobPositionId = jobPosition.JobPositionId;
                _context.Entry(businessinsemester).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(jobPosition + "Create Sucesses!!!");
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
        [HttpGet("Department/{id}")]
        [Authorize(Roles = "2")]
        [SwaggerOperation(Summary = "Get list Businesses by departmentID")]

        public async Task<IActionResult> GetListBusinessbyDepartmentID(string id)
        {
            try
            {
                List<object> list = new List<object>();
                var department = await _context.Departments.FindAsync(id);
                if (department == null)
                    return NotFound();
                var jobDepartments = await _context.JobDepartments.Where(c => c.DepartmentId == department.DepartmentId).ToListAsync();
                foreach (var job in jobDepartments)
                {
                    var jobpost = await _context.JobPositions.FindAsync(job.JobPostionId);
                    JobpostSThome jobrespone = new JobpostSThome
                    {

                        JobPositionId = jobpost.JobPositionId,
                        JobName = jobpost.JobName,
                        DetailBusiness = jobpost.DetailBusiness,
                        DetailWork = jobpost.DetailWork,
                        Request = jobpost.Request,
                        Salary = jobpost.Salary,
                        WorkLocation = jobpost.WorkLocation,
                        Benefit = jobpost.Benefit,
                        Amount = jobpost.Amount,
                        
                    };
                    list.Add(jobrespone);
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

        //[HttpPost("jobdepartment/{id}")]
        //[Authorize(Roles = "3")]


        //public async Task<ActionResult<JobDepartment>> PostJobDepartment([FromBody]string businessID,string DepartmentID)
        //{
        //    try
        //    {
        //        var businessinsemester = await _context.BusinessinSemesters.FindAsync(businessID);
        //        if (businessinsemester == null)
        //        {
        //            return BadRequest("Not found");
        //        }

        //        if (businessinsemester.JobPositionId == null)
        //            return BadRequest("Business no have jobpost");
        //        var jobposition = await _context.JobPositions.FindAsync(businessinsemester.JobPositionId);
        //        JobDepartment jobDepartment = new JobDepartment
        //        {
        //            JobPostionId = jobposition.JobPositionId,
        //            DepartmentId = DepartmentID
        //        };
        //        _context.JobDepartments.Add(jobDepartment);
        //        await _context.SaveChangesAsync();
        //        return Ok(jobDepartment);
        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (DbUpdateException)
        //    {
        //        return StatusCode(500, "Internal server exception");
        //    }
        //    catch (SqlException)
        //    {
        //        return StatusCode(500, "Internal server exception");
        //    }

        //}

        [HttpGet]
        [Authorize(Roles = "1,2,3")]
        [SwaggerOperation(Summary = "Get list jobposts")]
        public async Task<ActionResult<List<JobPosition>>> GetListJobPost()
        {
            try { 
            List<JobPosition> list = await _context.JobPositions.ToListAsync();

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

        [HttpGet("{id}")]
        [Authorize(Roles = "1,2,3")]
        [SwaggerOperation(Summary = "Get jobpost by jobpost id")]

        public async Task<ActionResult<JobPosition>> GetJobPost(int id)
        {
            try
            {
                
                var jobPosition = await _context.JobPositions.FindAsync(id);
                List<JobDepartment> list = await _context.JobDepartments.Where(c=>c.JobPostionId==jobPosition.JobPositionId).ToListAsync();
                foreach(JobDepartment jobdepartment in list)
                {
                    if (jobdepartment.JobPostionId == id)
                    {
                        jobPosition.JobDepartments.Add(jobdepartment);
                    }

                }
              
                return Ok(jobPosition);
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
        [Authorize(Roles = "3")]
        [SwaggerOperation(Summary = "Update jobpost by id")]

        public async Task<IActionResult> UpdateJobPost(int id, JobPostRequest request)
        {
            
            try
            {
                if (id != request.JobPositionId)
                    return NotFound();
                var jobPosition = await _context.JobPositions.FindAsync(request.JobPositionId);
                if (jobPosition == null)
                    return NotFound();
                jobPosition.JobName = request.JobName;
                jobPosition.DetailBusiness = request.DetailBusiness;
                jobPosition.DetailWork = request.DetailWork;
                jobPosition.Request = request.Request;
                jobPosition.Salary = request.Salary;
                jobPosition.WorkLocation = request.WorkLocation;
                jobPosition.Benefit = request.Benefit;
                jobPosition.Amount = request.Amount;
                _context.Entry(jobPosition).State = EntityState.Modified;
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
        [Authorize(Roles = "3")]
        [SwaggerOperation(Summary = "Delete jobpost by id")]

        public async Task<IActionResult> DeleteJobPost(int id)
        {
            try
            {
                var businessinses = await _context.BusinessinSemesters.Where(c => c.JobPositionId.Equals(id)).FirstOrDefaultAsync();
                if (businessinses == null)
                    return NotFound("Business no have jobpost");
                businessinses.JobPositionId = null;
                var apply = await _context.Applies.Where(c => c.BusinessId == businessinses.BusinessId).FirstOrDefaultAsync();
                if (apply != null)
                {
                    return BadRequest("Jobpost still have applies!!!");
                }
                _context.Entry(businessinses).State = EntityState.Modified;
                var jobposition = await _context.JobPositions.FindAsync(id);
                if (jobposition == null)
                {
                    return NotFound("can't find jobpost");
                }
                _context.JobPositions.Remove(jobposition);
                var jobdepartment = await _context.JobDepartments.Where(d => d.JobPostionId == jobposition.JobPositionId).ToListAsync();
                foreach(var job in jobdepartment)
                {
                    _context.JobDepartments.Remove(job);
                }
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
        [HttpGet("apply/{id}")]
        [Authorize(Roles = "1,2,3")]
        [SwaggerOperation(Summary = "find jobpostid by applyid")]
        public async Task<IActionResult> findjobpostid(int id)
        {
            try {
                var apply = await _context.Applies.FindAsync(id);
                if (apply == null)
                    return NotFound();
                var businessinsemester = await _context.BusinessinSemesters.FindAsync(apply.BusinessId);
                if (businessinsemester == null)
                    return NotFound();
                return Ok(businessinsemester.JobPositionId);
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
        [HttpGet("business/{id}")]
        [Authorize(Roles = "3")]
        [SwaggerOperation(Summary = "Get jobpost by business id")]

        public async Task<ActionResult<JobPosition>> GetJobPostbybusinessid(string id)
        {
            try
            {
                var businessinsemester = await _context.BusinessinSemesters.FindAsync(id);
                if (businessinsemester == null)
                    return NotFound();
                var jobPosition = await _context.JobPositions.FindAsync(businessinsemester.JobPositionId);
                if (jobPosition == null)
                    return NotFound("No have jobpost");
                List<JobDepartment> list = await _context.JobDepartments.Where(c => c.JobPostionId == jobPosition.JobPositionId).ToListAsync();
                foreach (JobDepartment jobdepartment in list)
                {
                    if (jobdepartment.JobPostionId == businessinsemester.JobPositionId)
                    {
                        jobPosition.JobDepartments.Add(jobdepartment);
                    }
                }

                return Ok(jobPosition);
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
