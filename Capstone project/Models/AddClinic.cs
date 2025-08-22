using System.ComponentModel.DataAnnotations;

namespace Capstone_project.Models
{
    public class AddClinic
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }   

        [Required(ErrorMessage = "Doctor name is required.")]
        public string DoctorName { get; set; }

        [Required(ErrorMessage = "Consultation fee is required.")]
        public string ConsultationFee { get; set; }

        [Required(ErrorMessage = "Please select at least one available day.")]
        public List<string> AvailableDays { get; set; } = new List<string>();

        [Required(ErrorMessage = "Please select at least one available time.")]
        public List<string> AvailableTimes { get; set; } = new List<string>();

        [Required(ErrorMessage = "Specialty is required.")]
        public string Specialty { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public string Location { get; set; }
    }
}
