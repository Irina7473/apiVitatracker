using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineReminderAPI.Models
{
    public class Usage
    {
        public int Id { get; set; }

        [Required]
        public int CourseId { get; set; }
        // public Course? Course { get; set; }

        //Время приема план
        [Required]
        public long UseTime { get; set; }

        //Фактическое время приема
        public long? FactUseTime { get; set; }

        //Количество
        public int? Quantity { get; set; }

        public bool NotUsed { get; set; } = false;

        //время создания и время изменения
        public DateTime Created { get; }
        public DateTime Updated { get; set; }

    }
}
