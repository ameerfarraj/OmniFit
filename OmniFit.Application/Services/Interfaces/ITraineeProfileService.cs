using System;
using System.Threading.Tasks;
using OmniFit.Application.DTOs.TraineeProfiles;

namespace OmniFit.Application.Services.Interfaces
{
    public interface ITraineeProfileService
    {
        Task<bool> CreateProfileAsync(Guid userId, CreateTraineeProfileDto profileDto);
    }
}
