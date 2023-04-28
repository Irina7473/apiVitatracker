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
using MedicineReminderAPI.Service;
using BC = BCrypt.Net.BCrypt;

namespace UMRapi.Controllers
{
    [Route("api/login")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticateController : ControllerBase
    {
        private readonly AppApiContext _context;

        public AuthenticateController(AppApiContext context)
        {
            _context = context;
        }

        // POST: api/login
        [HttpPost]        
        public IActionResult Token (Authenticate auth)
        {
            if (_context.Users == null) return NotFound();
                       
             var user = _context.Users.FirstOrDefault(u => u.Email == auth.Email );

            if (user == null || !BCrypt.Net.BCrypt.Verify(auth.Password, user.Password) || user.NotUsed == true )
            {
                return BadRequest(new { errorText = "Invalid username or password" });
            }                        

            var token = new MyToken().GenerateToken(user);

            return Content(token);
        }
      

    }
}
