using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OJTv2.Models;
using OJTv2.Requests;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace OJT.Controllers
{
    
    [Route("api/v1/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [AllowAnonymous]
    public class loginController : ControllerBase
    {
       
            private readonly SWPOJTContext _context;
        private readonly IConfiguration _configuration;
        public loginController(SWPOJTContext context, IConfiguration configuration)
            {
            _configuration = configuration;
            _context = context;
            
            }


        [HttpPost]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Login gg by jwt firebase")]
        public async Task<IActionResult> loginEmail(tokenfirebase tokenfb)
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(tokenfb.tokenfb);
            List<Claim> claim = jwt.Claims.ToList();
            var email = claim[9].Value.ToString();
            var user = await _context.Users.Where(c => c.Email == email).FirstOrDefaultAsync();
            if (user == null)
                return BadRequest();
          
            
            CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
            UserJWT userjwt = new UserJWT
            {
                UserID = user.UserId,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };
            if (!VerifyPasswordHass(user.Password, userjwt.PasswordHash, userjwt.PasswordSalt))
            {
                return BadRequest("Wrong Password.");
            }
            string tokenjwt = CreateToken(user);
            return Ok(tokenjwt);
        }


        [HttpPost("admin")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Login admin")]
        public async Task<ActionResult<string>> Loginadmin(adminlogin admin)
        {
            var user = await _context.Users.FindAsync(admin.id);
            
            if(user == null)
            {
                return NotFound();
            }
            if (user.RoleId != 1)
                return BadRequest();
            if(user.Password != admin.password)
            {
                return BadRequest();
            }
           
            CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
            UserJWT userjwt = new UserJWT
            {
                UserID = user.UserId,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };


            if (!VerifyPasswordHass(user.Password, userjwt.PasswordHash, userjwt.PasswordSalt))
            {
                return BadRequest("Wrong Password.");
            }
            string token = CreateToken(user);
            return Ok(token);

            

        }
        private bool VerifyPasswordHass(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {

                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }

        }

        [HttpPost("test")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Test token")]
        public  async Task<IActionResult> Createtokentest(string token)
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            List<Claim> claim = jwt.Claims.ToList();
            var email = claim[0].Value.ToString();
            return Ok(claim);//

        }
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                 new Claim(ClaimTypes.Email, user.Email),
                 new Claim(ClaimTypes.Role, user.RoleId.ToString()),
                 new Claim("Roleid",user.RoleId.ToString()),
                 new Claim("Userid",user.UserId.ToString()),
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        
       
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

    }
}
