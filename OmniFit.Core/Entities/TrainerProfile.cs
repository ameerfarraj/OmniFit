using System;
using System.ComponentModel.DataAnnotations;

namespace OmniFit.Core.Entities
{
    public class TrainerProfile
    {
        //מפתח ראשי שהוא גם מפתח זר למשתמש
        [Key]
        public Guid UserId { get; set; }

        // נתוני פרופיל פומביים
        public string? ProfileImageUrl { get; set; }
        public string? Bio { get; set; }
        
        // יצירת קשר ורשתות חברתיות
        public string? PhoneNumber { get; set; }
        public bool IsPhonePublic { get; set; } = false;
        public string? InstagramUrl { get; set; }
        public string? TikTokUrl { get; set; }

        // מודל עסקי וסליקה
        public string? StripeConnectAccountId { get; set; }
        
        // הגדרות וסטטוס
        public string LanguageCode { get; set; } = "he";
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsVerified { get; set; } = false; // האם האדמין אישר את המאמן

        // Navigation Property לטבלת המשתמשים
        public User? User { get; set; }
    }
}