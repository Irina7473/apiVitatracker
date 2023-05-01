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
        public DateTime Updated { get; }

        public List<Remedy>? Remedies { get; set; }

        public List<Remedy> FindRemedies (AppApiContext context)
        {
            return context.Remedys.Where(r => r.UserId == this.Id && r.NotUsed == false).ToList();
        }
    }
}
