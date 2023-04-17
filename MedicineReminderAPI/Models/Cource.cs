using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineReminderAPI.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        public int RemedyId { get; set; }
        // public Remedy? Remedy { get; set; }

        public string? Comment { get; set; }

        //Частота приема
        [Required]
        public int Regime { get; set; }

        [Required]
        public long StartDate { get; set; }

        public long? EndDate { get; set; }

        //Время напминания о след.курсе
        public long? RemindDate { get; set; }
        //Перерыв меду курсами
        public long? Interval { get; set; }
                
        //Завершен
        public bool IsFinished { get; set; } = false;
        //Бесконечный
        public bool IsInfinite { get; set; } = false;

        public bool NotUsed { get; set; } = false;

        //время создания и время изменения
        public DateTime Created { get; }
        public DateTime Updated { get; }

        public List<Usage>? Usages { get; set; } 

    }
}
