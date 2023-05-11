using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OJTv2.Models;
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
    public class industriesController : ControllerBase
    {
        private readonly SWPOJTContext _context;

        public industriesController(SWPOJTContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        [SwaggerOperation(Summary = "Create industry")]
        public async Task<IActionResult> CreateIndustry([FromBody]Industry industry)
        {
           
            try
            {
                _context.Industries.Add(industry);
                await _context.SaveChangesAsync();
                return Ok(industry);
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
        [SwaggerOperation(Summary = "Get list industries")]
        public async Task<ActionResult<List<Industry>>> GetListIndustry()
        {
            try
            {
               List<Industry> list = await _context.Industries.ToListAsync();
               
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
        [Authorize(Roles = "1")]
        [SwaggerOperation(Summary = "Get industry by id")]

        public async Task<ActionResult<List<Industry>>> GetIndustry(string id)
        {
            try
            {
                var industry = await _context.Industries.FindAsync(id);
                if (industry == null)
                    return NotFound();

                return Ok(industry);
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
        [SwaggerOperation(Summary = "Update industry by id")]

        public async Task<IActionResult> UpdateIndustry(string id,Industry industry)
        {
            if(id != industry.IndustryId)
            {
                return NotFound();
            }
            try
            {

                _context.Entry(industry).State = EntityState.Modified;
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
        [SwaggerOperation(Summary = "Delete industry by id")]
        public async Task<IActionResult> DeleteIndustry(string id)
        {
            try
            {
                var industry = await _context.Industries.FindAsync(id);
                _context.Industries.Remove(industry);
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

        [HttpGet("businesses/{id}")]
        [Authorize(Roles = "1,2,3")]
        [SwaggerOperation(Summary = "Get industries in business by id")]

        public async Task<IActionResult> GetBusinessIndustry(string id)
        {
            try
            {
                List<BusinessinSemester> list = new List<BusinessinSemester>();
               var business = await _context.Businesses.Where(b=>b.IndustryId.Equals(id)).ToListAsync();
               foreach(var bu in business)
                {
                    var businessinses = await _context.BusinessinSemesters.FindAsync(bu.BusinessId);
                    if(businessinses != null)
                    {
                        list.Add(businessinses);
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
    }
}
