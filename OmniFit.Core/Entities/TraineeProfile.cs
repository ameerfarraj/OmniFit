using System;
using System.ComponentModel.DataAnnotations;

namespace OmniFit.Core.Entities
{
    public class TraineeProfile
    {
        // מפתח ראשי שהוא גם מפתח זר למשתמש
        [Key]
        public Guid UserId { get; set; }

        // נתונים אישיים
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; } = string.Empty;

        // מדדים פיזיים
        public decimal HeightCM { get; set; }
        public decimal CurrentWeightKG { get; set; }
        public int ActivityLevel { get; set; }

        // תזונה
        public string? DietaryPreference { get; set; }
        public string? Allergies { get; set; }

        // גיימיפיקציה
        public int TotalPoints { get; set; } = 0;
        public int CurrentStreak { get; set; } = 0;

        // הגדרות מערכת
        public string LanguageCode { get; set; } = "he";
        public string Timezone { get; set; } = "Asia/Jerusalem";
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Property לטבלת המשתמשים
        public User? User { get; set; }
    }
}