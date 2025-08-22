using System.ComponentModel.DataAnnotations;

namespace Capstone_project.Models
{
    public class Prescription
    {
        [Key]
        public int Id { get; set; }

        public List<statusmodel> Prescriptions { get; set; } = new List<statusmodel>();
        public List<PrescriptionForm> Prescriptionforms { get; set; } = new List<PrescriptionForm>();
        public List<Select> Reservations { get; set; } = new List<Select>();
        public List<home> HomeId { get; set; } = new List<home>();
    }
}
