using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicineReminderAPI.Models;
using Microsoft.AspNetCore.Authorization;
using MedicineReminderAPI.Service;

namespace MedicineReminderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationSettingsController : ControllerBase
    {
        private readonly AppApiContext _context;
        private readonly IFindAuthorizedUser _autheUser;

        public NotificationSettingsController(AppApiContext context, IFindAuthorizedUser autheUser)
        {
            _context = context;
            _autheUser = autheUser;
        }

        // GET: api/NotificationSettings
        [HttpGet]
        public async Task<ActionResult<NotificationSetting>> GetNotificationSettings()
        {
            if (_context.NotificationSettings == null) return NotFound();
            // получение авторизированного пользователя
            var user = _autheUser.AuthorizedUser(HttpContext, _context);
            var notifi = await user.FindNotificationSettingsAsync(_context);
            if (notifi == null || user.NotUsed == true) return NotFound();

            return notifi;
        }

        // PUT: api/NotificationSettings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotificationSetting(int id, NotificationSetting notificationSetting)
        {
            if (_context.NotificationSettings == null) return NotFound();

            if (id != notificationSetting.Id) return BadRequest();
            // проверка валидации модели на успешность
            if (!ModelState.IsValid) return BadRequest(new ValidationProblemDetails(ModelState));

            _context.Entry(notificationSetting).State = EntityState.Modified;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificationSettingExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        private bool NotificationSettingExists(int id)
        {
            return (_context.NotificationSettings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
