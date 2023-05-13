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
using MedicineReminderAPI.Migrations;

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
        public async Task<ActionResult<List<Remedy>>> PostRemedies(List <Remedy> remedies)
        {
            if (_context.Remedies == null) return Problem("Entity set 'AppApiContext.Remedies'  is null.");
            if (remedies == null || remedies.Count == 0) return BadRequest(new { errorText = "No data" });
            // получение авторизированного пользователя
            var user = _autheUser.AuthorizedUser(HttpContext, _context);

            foreach (var remedy in remedies)
            {
                if (remedy.UserId != user.Id) return BadRequest(new { errorText = "Incorrect data" });
                //проверка валидации модели на успешность
                if (!ModelState.IsValid) return BadRequest(new ValidationProblemDetails(ModelState));
                _context.Remedies.Add(remedy);
            }

            await _context.SaveChangesAsync();
            return CreatedAtAction("GetRemedies", remedies);
        }
        
        // GET: api/Remedies/strategy?= "haveAttach"
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Remedy>>> GetRemedies(string strategy = "noAttach")
        {      
            if (_context.Remedies == null) return NotFound();
            var user = _autheUser.AuthorizedUser(HttpContext, _context);
            if (user == null) return BadRequest(new { errorText = "Login" });
            var remedies = user.FindRemedies(_context);
            
            if (remedies == null || remedies.Count == 0) return NotFound();
            if (strategy != "haveAttach") strategy = "noAttach";
            else foreach (var remedy in remedies) FindRemedyWithCoursesAndUsages(remedy);

            return remedies;
        }

        // GET: api/Remedies/5 strategy?= "noAttach"
        [HttpGet("{id}")]
        public async Task<ActionResult<Remedy>> GetRemedy(int id, string strategy = "haveAttach")
        {
            if (_context.Remedies == null) return NotFound();

            var remedy = await FindRemedyAsync(id);
            if (remedy == null) return NotFound();
            if (strategy != "noAttach") strategy = "haveAttach";
            else return remedy;
            return FindRemedyWithCoursesAndUsages(remedy);
        }

        // PUT: api/Remedies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRemedy(int id, Remedy remedy)
        {
            if (_context.Remedies == null) return NotFound();

            var existRemedy = await FindRemedyAsync(id);
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
            if (_context.Remedies == null) return NotFound();

            var remedy = await FindRemedyAsync(id);
            if (remedy == null) return NotFound();
            remedy = FindRemedyWithCoursesAndUsages(remedy);

            if (remedy.Courses != null)
                foreach (var course in remedy.Courses)
                {
                    if (course.Usages != null)
                        foreach (var usage in course.Usages)
                            usage.NotUsed = true;
                    course.NotUsed = true;
                }             
            remedy.NotUsed = true;

            _context.Entry(remedy).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        private bool RemedyExists(int id)
        {
            return (_context.Remedies?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        /*
        private async Task<List<Remedy>?> FindRemediesAsync()
        {
            if (_context.Remedies == null) return null;
            var user = _autheUser.AuthorizedUser(HttpContext, _context);
            if (user == null) return null;
            var remedies = user.FindRemedies(_context);
            return remedies;
        }
        */

        private async Task<Remedy?> FindRemedyAsync(int id)
        {
            var remedy = await _context.Remedies.FindAsync(id);
            var user = _autheUser.AuthorizedUser(HttpContext, _context);
            if (remedy == null || remedy.NotUsed == true || user == null || remedy.UserId != user.Id)
                return null;

            return remedy;
        }

        private Remedy FindRemedyWithCoursesAndUsages(Remedy remedy)
        {
            remedy.Courses = remedy.FindCourses(_context);
            foreach (Course course in remedy.Courses) course.Usages = course.FindUsages(_context);
            return remedy;
        }
    }
}
