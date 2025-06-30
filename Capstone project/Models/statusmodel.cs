using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public double? Height { get; set; }
        public double? Weight { get; set; }

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

        [NotMapped]
        public List<IFormFile>? UploadedFiles { get; set; }
        public string? UploadedFilePaths { get; set; }

        public string? SmokingOrDrinking { get; set; }
        public string? SleepPattern { get; set; }
        public string? ActivityLevel { get; set; }
    }

}
