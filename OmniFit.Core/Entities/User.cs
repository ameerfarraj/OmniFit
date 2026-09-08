using System;

namespace OmniFit.Core.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? GoogleAuthId { get; set; }
        public string? AppleAuthId { get; set; }
        public int RoleId { get; set; }
        public string? SubscriptionTier { get; set; }
        public string? StripeCustomerId { get; set; }
        public int FailedLoginAttempts { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public TraineeProfile? TraineeProfile { get; set; }
        public TrainerProfile? TrainerProfile { get; set; }
    }
}