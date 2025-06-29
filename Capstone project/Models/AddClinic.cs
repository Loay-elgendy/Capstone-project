using System.ComponentModel.DataAnnotations;

namespace Capstone_project.Models
{
    public class AddClinic
    {
        [Key]
        public int Id { get; set; }

        public string DoctorId { get; set; } // New for linking with login

        public string DoctorName { get; set; }

        public string ConsultationFee { get; set; }

        public string AvailableDays { get; set; }

        public string AvailableTimes { get; set; }

        public string Specialty { get; set; }

        public string Location { get; set; }
    }
}
