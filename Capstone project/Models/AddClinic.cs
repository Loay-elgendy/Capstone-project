using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone_project.Models
{
    public class AddClinic
    {
        [Key]
        public int Id { get; set; }

        public string DoctorName { get; set; }

        public string ConsultationFee { get; set; }

        public string AvailableDays { get; set; }

        public string AvailableTimes { get; set; }

        public string Specialty { get; set; }

        public string Location { get; set; }

        // Foreign key to patient (SignUp)
        [Display(Name = "Patient ID")]
        public string PatID { get; set; }

        [ForeignKey("PatID")]
        public SignUp Patient { get; set; }
    }
}
