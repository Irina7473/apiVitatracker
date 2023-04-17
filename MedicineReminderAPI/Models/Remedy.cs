using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MedicineReminderAPI.Models
{
    public class Remedy
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        //public User? User { get; set; }             

        [Required(ErrorMessage = "Не указано название")]
        [StringLength(128, MinimumLength = 3, ErrorMessage = "Длина названия лекарства должна быть не менее 3 символов")]
        public string Name { get; set; }

        public string? Description { get; set; }

        public string? Comment { get; set; }

        //Форма препарата
        [Required]
        public int Type { get; set; }

        [Required]
        public int Icon { get; set; } = 0;

        [Required]
        public int Color { get; set; } = 0;

        public int? Dose { get; set; }

        public int? MeasureUnit { get; set; }

        public int? BeforeFood { get; set; }

        public string? Photo { get; set; }

        public bool NotUsed { get; set; } = false;

        //время создания и время изменения
        public DateTime Created { get; }
        public DateTime Updated { get; }

        public List<HistoryRemedy>? HistoryRemedys { get; set; }
        public List<Course>? Courses { get; set; } 
    }
}
