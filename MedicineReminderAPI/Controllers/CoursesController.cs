using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class CoursesController : ControllerBase
    {
        private readonly AppApiContext _context;
        private readonly IFindAuthorizedUser _autheUser;

        public CoursesController(AppApiContext context, IFindAuthorizedUser autheUser)
        {
            _context = context;
            _autheUser = autheUser;
        }

        // POST: api/Courses
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            if (_context.Courses == null) return Problem("Entity set 'AppApiContext.Courses'  is null.");

            // проверка авторизации
            var user = _autheUser.AuthorizedUser(HttpContext, _context);
            var remedy = _context.Remedys.Where(c => c.Id == course.RemedyId).FirstOrDefault();
            if (user == null || remedy == null || remedy.NotUsed == true || remedy.UserId != user.Id) 
                    return BadRequest(new { errorText = "Incorrect data" });

            //проверка валидации модели на успешность
            if (!ModelState.IsValid) return BadRequest(new ValidationProblemDetails(ModelState));

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = course.Id }, course);
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
           if (_context.Courses == null) return NotFound();

            var user = _autheUser.AuthorizedUser(HttpContext, _context);
            if (user == null) return BadRequest(new { errorText = "Login" });

            user.Remedies = await _context.Remedys.Where(r => r.UserId == user.Id && r.NotUsed == false).ToListAsync();
            List<Course> courses = new();
            foreach (Remedy r in user.Remedies)
                courses.AddRange (await _context.Courses.Where(c => c.RemedyId == r.Id && c.NotUsed == false).ToListAsync());

            return courses;
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            if (_context.Courses == null) return NotFound();

            var course = await CourseFindAsync(id);
            if (course == null) return NotFound();

            course.Usages = await _context.Usages.Where(u => u.CourseId == course.Id && u.NotUsed == false).ToListAsync();

            return course;
        }

        // PUT: api/Courses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {            
            if (_context.Courses == null) return NotFound();

            var existCourse = await CourseFindAsync(id);
            if (existCourse == null || existCourse.Id != course.Id) return NotFound();
            //Отсоединение: сущность не отслеживается контекстом
            _context.Entry(existCourse).State = EntityState.Detached;

            // проверка валидации модели на успешность
            if (!ModelState.IsValid) return BadRequest(new ValidationProblemDetails(ModelState));

            _context.Entry(course).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }        

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            if (_context.Courses == null) return NotFound();

            var course = await CourseFindAsync(id);
            if (course == null) return NotFound();           

            course.NotUsed = true;

            _context.Entry(course).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();                     
        }

        private bool CourseExists(int id)
        {
            return (_context.Courses?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task<Course?> CourseFindAsync(int id)
        {            
            var course = await _context.Courses.FindAsync(id);
            var user = _autheUser.AuthorizedUser(HttpContext, _context);
            if (user == null || course == null || course.NotUsed == true) return null;
            var remedy = _context.Remedys.Where(c => c.Id == course.RemedyId).FirstOrDefault();
            if (remedy == null || remedy.UserId != user.Id) return null;

            return course;
        }
        
    }
}
