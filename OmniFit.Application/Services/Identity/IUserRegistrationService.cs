using System;

namespace OmniFit.Application.Services.Identity;

public interface IUserRegistrationService
{
    bool IsEligibleAge(DateOnly dateOfBirth);
    bool IsPasswordStrong(string password);
}
