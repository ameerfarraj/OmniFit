using System;
using OmniFit.Core.Entities;

namespace OmniFit.Application.Services.Identity;

public interface IAuthenticationService
{
    bool ProcessLoginAttempt(User user, bool isPasswordValid);
    void ResetFailedAttempts(User user);
}
