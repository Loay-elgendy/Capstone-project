using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone_project.Models
{
    public class AddClinic
    {
        [Key]
        public int Id { get; set; }

        public string DoctorName { get; set; }

        public string DoctorID { get; set; }

        public string ConsultationFee { get; set; }

        public List<string> AvailableDays { get; set; } = new List<string>();

        public List<string> AvailableTimes { get; set; } = new List<string>();

        public string Specialty { get; set; }

        public string Location { get; set; }
    }
}
