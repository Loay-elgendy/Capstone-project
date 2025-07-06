using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Capstone_project.Models
{
    public class PrescriptionForm
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Diagnosis { get; set; }

        [Required]
        public string DoctorID { get; set; }

        [Required]
        public string Medication { get; set; }

        [Required]
        public string Dosage { get; set; }

        [Required]
        public string Tests { get; set; }

        public string? Notes { get; set; }

    }
}
