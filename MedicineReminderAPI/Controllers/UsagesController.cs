using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicineReminderAPI.Models;
using MedicineReminderAPI.Service;

namespace MedicineReminderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsagesController : ControllerBase
    {
        private readonly AppApiContext _context;
        private readonly IFindAuthorizedUser _autheUser;

        public UsagesController(AppApiContext context, IFindAuthorizedUser autheUser)
        {
            _context = context;
            _autheUser = autheUser;
        }


        // POST: api/Usages
        [HttpPost]
        public async Task<ActionResult<Usage>> PostUsage(Usage usage)
        {
            if (_context.Usages == null) return Problem("Entity set 'AppApiContext.Usages'  is null.");

            // проверка авторизации
            var course = _context.Courses.Where(c => c.Id == usage.CourseId).FirstOrDefault();
            if (course == null || course.NotUsed == true) return BadRequest(new { errorText = "Incorrect data" });

            var user = _autheUser.AuthorizedUser(HttpContext, _context);
            var remedy = _context.Remedys.Where(c => c.Id == course.RemedyId).FirstOrDefault();
            if (user == null || remedy == null || remedy.NotUsed == true || remedy.UserId != user.Id)
                return BadRequest(new { errorText = "Incorrect data" });

            //проверка валидации модели на успешность
            if (!ModelState.IsValid) return BadRequest(new ValidationProblemDetails(ModelState));

            _context.Usages.Add(usage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsage", new { id = usage.Id }, usage);
        }

        // GET: api/Usages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usage>>> GetUsages()
        {
            if (_context.Usages == null) return NotFound();

            var user = _autheUser.AuthorizedUser(HttpContext, _context);
            if (user == null) return BadRequest(new { errorText = "Login" });

            var remedies = await _context.Remedys.Where(r => r.UserId == user.Id && r.NotUsed == false).ToListAsync();
            List<Course> courses = new();
            foreach (Remedy r in remedies)
                courses.AddRange(await _context.Courses.Where(c => c.RemedyId == r.Id && c.NotUsed == false).ToListAsync());
            List<Usage> usages = new();
            foreach (Course c in courses)
                usages.AddRange(await _context.Usages.Where(u => u.CourseId == c.Id && u.NotUsed == false).ToListAsync());

            return usages;
        }

        // GET: api/Usages/5  
        [HttpGet("{id}")]
        public async Task<ActionResult<Usage>> GetUsage(int id)
        {
            if (_context.Usages == null) return NotFound();

            var usage = await UsageFindAsync(id);
            if (usage == null) return NotFound();

            return usage;
        }
                       
        // PUT: api/Usages/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsage(int id, Usage usage)
        {
            if (_context.Usages == null) return NotFound();

            var existUsage = await UsageFindAsync(id);
            if (existUsage == null || existUsage.Id != usage.Id) return NotFound();
            //Отсоединение: сущность не отслеживается контекстом
            _context.Entry(existUsage).State = EntityState.Detached;

            // проверка валидации модели на успешность
            if (!ModelState.IsValid) return BadRequest(new ValidationProblemDetails(ModelState));

            _context.Entry(usage).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsageExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // DELETE: api/Usages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsage(int id)
        {
            if (_context.Usages == null) return NotFound();          

            var usage = await UsageFindAsync(id);
            if (usage == null) return NotFound();

            usage.NotUsed = true;

            _context.Entry(usage).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsageExists(int id)
        {
            return (_context.Usages?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task<Usage?> UsageFindAsync(int id)
        {           
            var usage = await _context.Usages.FindAsync(id);
            var user = _autheUser.AuthorizedUser(HttpContext, _context);
            if (user == null || usage == null || usage.NotUsed == true) return null;

            var course = _context.Courses.Where(c => c.Id == usage.CourseId).FirstOrDefault();
            if (course == null || course.NotUsed == true) return null;

            var remedy = _context.Remedys.Where(c => c.Id == course.RemedyId).FirstOrDefault();
            if (remedy == null || remedy.NotUsed == true || remedy.UserId != user.Id) return null;

            return usage;
        }

    }
}
