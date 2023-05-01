using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using MedicineReminderAPI.Models;
using MedicineReminderAPI.Service;


namespace MedicineReminderAPI.Controllerss
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly AppApiContext _context;
        private readonly IFindAuthorizedUser _autheUser;

        public UsersController(AppApiContext context, IFindAuthorizedUser autheUser)
        {
            _context = context;
            _autheUser = autheUser;
        }

        // POST: api/Users
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (_context.Users == null) return Problem("Entity set 'AppApiContext.Users'  is null.");
           
            //проверка валидации модели на успешность
            if (!ModelState.IsValid) return BadRequest(new ValidationProblemDetails(ModelState));

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            if (_context.Users == null) return NotFound();

            var user = _autheUser.AuthorizedUser(HttpContext, _context);
            if (user == null || user.Id != id)
                return BadRequest(new { errorText = "Login" });

            user.Remedies = user.FindRemedies(_context);

            return user;
        }

        // сделать изменение пароля с подтверждением на почту
        // PUT: api/Users/5
        [HttpPut("{id}, {name}, {avatar}")]
        public async Task<IActionResult> PutUser(int id, string name="-1", string avatar="-1")
        {
            if (_context.Users == null) return NotFound();

            var user = _autheUser.AuthorizedUser(HttpContext, _context);
            if (user == null || user.Id != id) return NotFound();

            if (name != "-1") user.Name = name;
            if (avatar != "-1") user.Avatar = avatar;
            _context.Entry(user).State = EntityState.Modified;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null) return NotFound();

            var user = _autheUser.AuthorizedUser(HttpContext, _context);
            if (user == null || user.Id != id) return NotFound();

            user.NotUsed = true;        
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(u => u.Id == id)).GetValueOrDefault();
        }


    }
}
