using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MedicineReminderAPI.Models
{        
    public class NotificationSetting
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool IsFloat { get; set; } = false;
        public bool MedicalControl { get; set; } = false;
        public bool NextCourseStart { get; set; } = true;

        
    }
}
