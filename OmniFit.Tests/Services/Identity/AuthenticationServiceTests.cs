using System;
using Xunit;
using OmniFit.Application.Services.Identity;
using OmniFit.Core.Entities;

namespace OmniFit.Tests.Services.Identity;

public class AuthenticationServiceTests
{
    private readonly AuthenticationService _service;

    public AuthenticationServiceTests()
    {
        _service = new AuthenticationService();
    }

    [Fact]
    public void ProcessLoginAttempt_ValidPassword_ResetsAttemptsAndReturnsTrue()
    {
        var user = new User { FailedLoginAttempts = 3, IsLockedOut = false };
        var result = _service.ProcessLoginAttempt(user, isPasswordValid: true);
        
        Assert.True(result);
        Assert.Equal(0, user.FailedLoginAttempts);
    }

    [Fact]
    public void ProcessLoginAttempt_InvalidPassword_IncrementsAttempts()
    {
        var user = new User { FailedLoginAttempts = 2, IsLockedOut = false };
        var result = _service.ProcessLoginAttempt(user, isPasswordValid: false);
        
        Assert.False(result);
        Assert.Equal(3, user.FailedLoginAttempts);
        Assert.False(user.IsLockedOut);
    }

    [Fact]
    public void ProcessLoginAttempt_FifthFailedAttempt_LocksAccount()
    {
        var user = new User { FailedLoginAttempts = 4, IsLockedOut = false };
        var result = _service.ProcessLoginAttempt(user, isPasswordValid: false);
        
        Assert.False(result);
        Assert.Equal(5, user.FailedLoginAttempts);
        Assert.True(user.IsLockedOut);
        Assert.NotNull(user.LockoutEnd);
    }
}
