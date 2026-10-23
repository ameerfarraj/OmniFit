using System;
using System.ComponentModel.DataAnnotations;

namespace OmniFit.Application.DTOs.TraineeProfiles
{
    public class CreateTraineeProfileDto
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [Range(100, 250)]
        public decimal HeightCM { get; set; }

        [Required]
        [Range(30, 300)]
        public decimal CurrentWeightKG { get; set; }

        [Required]
        [Range(1, 5)]
        public int ActivityLevel { get; set; }

        public string? DietaryPreference { get; set; }

        public string? Allergies { get; set; }

        public string? TrainingGoals { get; set; }

        public string? PreviousExperience { get; set; }

        [Required]
        [Range(typeof(bool), "true", "true")]
        public bool IsAgeVerified { get; set; }
    }
}