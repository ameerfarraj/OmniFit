using System;
using System.Threading.Tasks;
using OmniFit.Application.DTOs.TraineeProfiles;
using OmniFit.Application.Services.Interfaces;
using OmniFit.Core.Entities;
using OmniFit.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace OmniFit.Application.Services
{
    public class TraineeProfileService : ITraineeProfileService
    {
        private readonly ApplicationDbContext _context;

        public TraineeProfileService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateProfileAsync(Guid userId, CreateTraineeProfileDto profileDto)
        {
            var profile = new TraineeProfile
            {
                UserId = userId,
                FirstName = profileDto.FirstName,
                LastName = profileDto.LastName,
                BirthDate = profileDto.BirthDate.ToUniversalTime(),
                Gender = profileDto.Gender,
                HeightCM = profileDto.HeightCM,
                CurrentWeightKG = profileDto.CurrentWeightKG,
                ActivityLevel = profileDto.ActivityLevel,
                DietaryPreference = profileDto.DietaryPreference,
                Allergies = profileDto.Allergies,
                TrainingGoals = profileDto.TrainingGoals,
                PreviousExperience = profileDto.PreviousExperience,
                IsAgeVerified = profileDto.IsAgeVerified
            };

            _context.TraineeProfiles.Add(profile);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}