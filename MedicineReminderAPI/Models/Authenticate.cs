using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MedicineReminderAPI.Models
{
    public class Authenticate
    {
        [Required(ErrorMessage = "Не указан электронный адрес")]        
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]        
        public string Password { get; set;  }
    }
}

