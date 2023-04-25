using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using MedicineReminderAPI.Models;
using BC = BCrypt.Net.BCrypt;

namespace UMRapi.Controllers
{
    [Route("api/login")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticateController : ControllerBase
    {
        private readonly AppApiContext _context;
        //private readonly ILogger<AuthenticateController> _logger;

        public AuthenticateController(AppApiContext context)
        {
            _context = context;
           // _logger.LogInformation($"{DateTime.Now.ToLongTimeString()} Hello AuthenticateController");
        }

        // POST: api/login
        [HttpPost("{email}, {password}")]        
        public IActionResult Token (string email, string password)
        {
            if (_context.Users == null) return NotFound();
                       
             var user = _context.Users.FirstOrDefault(u => u.Email == email );

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password) || user.NotUsed == true )
            {
                return BadRequest(new { errorText = "Invalid username or password" });
            }
             
            //создаю объекты Claim для авторизации
            var claims = new List<Claim> { 
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Actor, user.Id.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Bearer");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);           

            // сделать Refresh Token
            // создаю JWT-токен - вынести отдельно
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(1440)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return Content(token);
        }
      

    }
}
