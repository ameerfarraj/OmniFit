using System;
using OmniFit.Core.Entities;

namespace OmniFit.Application.Services.Identity;

public class AuthenticationService : IAuthenticationService
{
    private const int MaxFailedAccessAttempts = 5;
    private readonly TimeSpan DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

    public bool ProcessLoginAttempt(User user, bool isPasswordValid)
    {
        if (user.IsLockedOut && user.LockoutEnd > DateTime.UtcNow)
        {
            return false;
        }

        if (user.IsLockedOut && user.LockoutEnd <= DateTime.UtcNow)
        {
            ResetFailedAttempts(user);
        }

        if (isPasswordValid)
        {
            ResetFailedAttempts(user);
            return true;
        }

        user.FailedLoginAttempts++;
        
        if (user.FailedLoginAttempts >= MaxFailedAccessAttempts)
        {
            user.IsLockedOut = true;
            user.LockoutEnd = DateTime.UtcNow.Add(DefaultLockoutTimeSpan);
        }

        return false;
    }

    public void ResetFailedAttempts(User user)
    {
        user.FailedLoginAttempts = 0;
        user.IsLockedOut = false;
        user.LockoutEnd = null;
    }
}
