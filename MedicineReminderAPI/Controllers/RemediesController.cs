using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicineReminderAPI.Models;
using MedicineReminderAPI.Service;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MedicineReminderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RemediesController : ControllerBase
    {
        private readonly AppApiContext _context;
        private readonly IFindAuthorizedUser _autheUser;

        public RemediesController(AppApiContext context, IFindAuthorizedUser autheUser)
        {
            _context = context;
            _autheUser = autheUser;
        }

        // POST: api/Remedies
        [HttpPost]
        public async Task<ActionResult<Remedy>> PostRemedy(Remedy remedy)
        {
            if (_context.Remedys == null) return Problem("Entity set 'AppApiContext.Remedys'  is null.");
            // проверка авторизации
            var user = _autheUser.AuthorizedUser(HttpContext, _context);
            if (user == null || remedy.UserId != user.Id) return BadRequest(new { errorText = "Incorrect data" });

            //проверка валидации модели на успешность
            if (!ModelState.IsValid) return BadRequest(new ValidationProblemDetails(ModelState));
            
            _context.Remedys.Add(remedy);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRemedy", new { id = remedy.Id }, remedy);
        }

        // GET: api/Remedies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Remedy>>> GetRemedys()
        {
            if (_context.Remedys == null) return NotFound();

            var user = _autheUser.AuthorizedUser(HttpContext, _context);
            if (user == null) return BadRequest(new { errorText = "Login" });
            var remedys = await _context.Remedys.Where(r => r.UserId == user.Id && r.NotUsed == false).ToListAsync();
                                    
            return remedys;
        }

        // GET: api/Remedies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Remedy>> GetRemedy(int id)
        {
            if (_context.Remedys == null) return NotFound();

            var remedy = await RemedyFindAsync(id);
            if (remedy == null) return NotFound();
            remedy.Courses = await _context.Courses.Where(c => c.RemedyId == remedy.Id && c.NotUsed == false).ToListAsync();
            
            return remedy;
        }

        // PUT: api/Remedies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRemedy(int id, Remedy remedy)
        {
            if (_context.Remedys == null) return NotFound();

            var existRemedy = await RemedyFindAsync(id);
            if (existRemedy == null || existRemedy.Id != remedy.Id ) return NotFound();
            //Отсоединение: сущность не отслеживается контекстом
            _context.Entry(existRemedy).State = EntityState.Detached;

            // проверка валидации модели на успешность
            if (!ModelState.IsValid) return BadRequest(new ValidationProblemDetails(ModelState));

             _context.Entry(remedy).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!RemedyExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }       

        // DELETE: api/Remedies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRemedy(int id)
        {
            if (_context.Remedys == null) return NotFound();

            var remedy = await RemedyFindAsync(id);
            if (remedy == null) return NotFound();

            remedy.NotUsed = true;

            _context.Entry(remedy).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        private bool RemedyExists(int id)
        {
            return (_context.Remedys?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task<Remedy?> RemedyFindAsync(int id)
        {
            var remedy = await _context.Remedys.FindAsync(id);
            var user = _autheUser.AuthorizedUser(HttpContext, _context);
            if (remedy == null || remedy.NotUsed == true || user == null || remedy.UserId != user.Id)
                return null;
            return remedy;
        }
    }
}
