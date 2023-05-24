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
            //проверка email
            if ((_context.Users?.Any(u => u.Email == user.Email)).GetValueOrDefault())
                return BadRequest(new { errorText = "User exists" });

            //проверка валидации модели на успешность
            if (!ModelState.IsValid) return BadRequest(new ValidationProblemDetails(ModelState));

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = new MyToken().GenerateToken(user);

            return CreatedAtAction("GetUser", new { id = user.Id, token=token });
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<User>> GetUser()
        {
            if (_context.Users == null) return NotFound();

            var user = _autheUser.AuthorizedUser(HttpContext, _context);
            if (user == null) return BadRequest(new { errorText = "Login" });

            return await user.GetUserAsync(_context);
        }
                
        // PUT: api/Users/5
        [HttpPut]
        public async Task<IActionResult> PutUser(User user)
        {
            if (_context.Users == null) return NotFound();

            var auth = _autheUser.AuthorizedUser(HttpContext, _context);
            if (auth == null || auth.Id != user.Id) return NotFound();

            user.NotificationSetting.Id = auth.FindNotificationSettings(_context).Id;
            auth.UpdateUser(user.Name, user.Email, user.Avatar, user.NotificationSetting);

            // проверка валидации модели на успешность
            if (!ModelState.IsValid) return BadRequest(new ValidationProblemDetails(ModelState));
            _context.Entry(auth).State = EntityState.Modified;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)  { throw;}

            return NoContent();
        }

        //нужно техзадание на удаление пользователя
        //нужно ли помечать notused все для этого user??
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
            return (_context.Users?.Any(u => u.Id == id && u.NotUsed==false)).GetValueOrDefault();
        }


    }
}
