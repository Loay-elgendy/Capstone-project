using System.ComponentModel.DataAnnotations;

namespace Capstone_project.Models
{
    public class statusmodel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Age is required")]
        public string Age { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        public string? Height { get; set; }
        public string? Weight { get; set; }

        public string? Symptoms { get; set; }
        public string? SymptomsStartDate { get; set; }
        public string? SymptomsPattern { get; set; }

        public int? SeverityLevel { get; set; }
        public string? SeverityCondition { get; set; }

        public string? ChronicDiseases { get; set; }
        public string? TriggerActivities { get; set; }
        public string? PreviousMedications { get; set; }

        public string? FamilyIssues { get; set; }
        public string? CurrentMedications { get; set; }
        public string? Allergies { get; set; }

        public string? SmokingOrDrinking { get; set; }
        public string? SleepPattern { get; set; }
        public string? ActivityLevel { get; set; }

        public string PatientId { get; set; }
        public List<Select> SelectedId { get; set; } = new List<Select>();
    }
}
