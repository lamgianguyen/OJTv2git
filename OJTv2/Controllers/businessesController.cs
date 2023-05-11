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
    public class businessesController : ControllerBase
    {
        private readonly SWPOJTContext _context;

        public businessesController(SWPOJTContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        [SwaggerOperation(Summary = "Create Business")]
        
        public async Task<IActionResult> CreateBusiness(BusinessRequest request)
        {
            try
            {
                User user = new User
                {
                    UserId = request.BusinessId,
                    Password = "1",
                    RoleId = 3,
                    Email = request.ContactEmail
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                Business business = new Business
                {
                    BusinessId = request.BusinessId,
                    BusinessName = request.BusinessName,
                    Website = request.Website,
                    ContactEmail = request.ContactEmail,
                    ContactPhone = request.ContactPhone,
                    IndustryId = request.IndustryId,
                    SemesterId = request.SemesterId,
                    Image = "",
                };

                _context.Businesses.Add(business);
                await _context.SaveChangesAsync();
                return Ok(business);
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
        [SwaggerOperation(Summary = "Get list Businesses")]
        
        public async Task<ActionResult<List<Business>>> GetListBusiness()
        {
            try
            {               
                List<Business> list = await _context.Businesses.ToListAsync();
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
        
        [HttpGet("Search")]
        [Authorize(Roles = "1")]
        [SwaggerOperation(Summary = "Search Business by name")]      
        public async Task<ActionResult<List<Business>>> GetListBusinessbyName(string name)
        {
            try
            {
                if(name == null|| name.Length == 0)
                {
                    List<Business> list1 = await _context.Businesses.ToListAsync();
                    return Ok(list1);
                }
                List<Business> list2 = await _context.Businesses.Where(i => i.BusinessName.Contains(name)).ToListAsync();
                return Ok(list2);
            } catch (ArgumentNullException ex)
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
        [Authorize(Roles = "1,3")]
        [SwaggerOperation(Summary = "Get Business by id")]
        
        public async Task<IActionResult> GetBusiness(string id)
        {
            try
            {
                var business = await _context.Businesses.FindAsync(id);
                return Ok(business);
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
        [SwaggerOperation(Summary = "Update Business by id")]
        
        
        public async Task<IActionResult> UpdateBusiness(string id,businessUpdate request)
        {
            if (id != request.BusinessId)
                return BadRequest();
            try
            {
                var business = await _context.Businesses.FindAsync(id);
                if (business == null)
                    return NotFound();
                business.BusinessName= request.BusinessName;
                business.Website = request.Website;
                business.ContactPhone = request.ContactPhone;
                business.ContactEmail = request.ContactEmail;
                business.Image = request.Image;
                _context.Entry(business).State = EntityState.Modified;
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
        [SwaggerOperation(Summary = "Delete Business by id")]
        
        public async Task<IActionResult> DeleteBusiness(string id)
        {
            var business = await _context.Businesses.FindAsync(id);
            try
            {
                _context.Businesses.Remove(business);
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
