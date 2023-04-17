using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineReminderAPI.Models
{
    public class HistoryRemedy
    {
        public int Id { get; set; }

        [Required]
        public int RemedyId { get; set; }

        [Required]
        public int Dose { get; set; }

        [Required]
        public int MeasureUnit { get; set; }

        //время создания 
        public DateTime Created { get;} 

    }
}
