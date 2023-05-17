using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MedicineReminderAPI.Models
{
    [Microsoft.EntityFrameworkCore.Index(nameof(Email), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }

        [StringLength(120, MinimumLength = 1, ErrorMessage = "Длина имени должна быть не менее 1 символа")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Не указан электронный адрес")]
        [StringLength(64, MinimumLength = 6)]
        [RegularExpression(@"[A-Za-z0-9._-]+@[A-Za-z0-9._-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]        
        public string Email { get; set; }


        [Column(TypeName = "varchar(120)")]
        [Required(ErrorMessage = "Не указан пароль")]
        private string password;
        public string Password
        {
            get { return password; }
            set {                
                string pattern = @"^[A-Za-z0-9]{6,16}$";
                if (Regex.IsMatch(value, pattern))
                        password = BCrypt.Net.BCrypt.HashPassword(value);                    
            }
        }

        public string? Avatar { get; set; }

        [Required]
        public NotificationSetting NotificationSetting { get; set; } = new();

        public bool NotUsed { get; set; } = false;

        //время создания и время изменения
        public DateTime Created { get; }
        public DateTime Updated { get; set; }        

        public List<Remedy>? Remedies { get; set; }


        public User UpdateUser(string? name, string? email, string? avatar, NotificationSetting notifi)
        {
            if (name != null) this.Name = name;
            if (email != null) this.Email = email;
            if (avatar != null) this.Avatar = avatar;
            if (notifi != null) this.NotificationSetting = notifi;
            return this;
        }

        public async Task<User> GetUserAsync(AppApiContext context)
        {
            this.password = "123456";
            this.NotificationSetting = await FindNotificationSettingsAsync(context);
            return this;
        }

        public async Task<NotificationSetting?> FindNotificationSettingsAsync(AppApiContext context)
        {
            return await context.NotificationSettings.Where(n => n.UserId == Id).FirstOrDefaultAsync();
        }

        public List<Remedy> FindRemedies (AppApiContext context)
        {
            return context.Remedies.Where(r => r.UserId == Id && r.NotUsed == false).ToList();
        }

    }
}
