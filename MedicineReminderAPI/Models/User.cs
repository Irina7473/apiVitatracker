using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;

namespace MedicineReminderAPI.Models
{
    public class User : IdentityUser<int>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указано имя")]
        [StringLength(120, MinimumLength = 1, ErrorMessage = "Длина имени должна быть не менее 1 символа")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Не указан электронный адрес")]
        [StringLength(64, MinimumLength = 6)]
        [RegularExpression(@"[A-Za-z0-9._-]+@[A-Za-z0-9._-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        public string Email { get; set; }


        [Column(TypeName = "varchar(120)")]
        [Required(ErrorMessage = "Не указан пароль")]
        //[RegularExpression(@"^[A-Za-z0-9]{6,16}$", ErrorMessage = "Некорректный пароль")]
        private string passwordHash;
        public string PasswordHash
        {
            get { return passwordHash; }
            set {                
                string pattern = @"^[A-Za-z0-9]{6,16}$";
                if (Regex.IsMatch(value, pattern))
                    passwordHash = BCrypt.Net.BCrypt.HashPassword(value);                    
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

        public virtual ICollection<UserClaim>? Claims { get; set; }
        public virtual ICollection<UserLogin>? Logins { get; set; }
        public virtual ICollection<UserToken>? Tokens { get; set; }
    }

    public class UserClaim : IdentityUserClaim<int>
    {
        public virtual User User { get; set; }
    }

    public class UserLogin : IdentityUserLogin<int>
    {
        public virtual User User { get; set; }
    }

    public class UserToken : IdentityUserToken<int>
    {
        public virtual User User { get; set; }
    }

}
