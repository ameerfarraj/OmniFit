using System;
using Xunit;
using OmniFit.Application.Services.Identity;

namespace OmniFit.Tests.Services.Identity;

public class UserRegistrationServiceTests
{
    private readonly UserRegistrationService _service;

    public UserRegistrationServiceTests()
    {
        _service = new UserRegistrationService();
    }

    [Fact]
    public void IsEligibleAge_Under16_ReturnsFalse()
    {
        var dateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-15));
        var result = _service.IsEligibleAge(dateOfBirth);
        Assert.False(result);
    }

    [Fact]
    public void IsEligibleAge_Exactly16_ReturnsTrue()
    {
        var dateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-16));
        var result = _service.IsEligibleAge(dateOfBirth);
        Assert.True(result);
    }

    [Theory]
    [InlineData("short1!")] 
    [InlineData("NoSpecialChar1")] 
    [InlineData("nouppercase1!")] 
    [InlineData("NOLOWERCASE1!")] // הטעות תוקנה: רק אותיות גדולות
    [InlineData("NoNumber!")] 
    public void IsPasswordStrong_WeakPasswords_ReturnsFalse(string password)
    {
        var result = _service.IsPasswordStrong(password);
        Assert.False(result);
    }

    [Fact]
    public void IsPasswordStrong_ValidPassword_ReturnsTrue()
    {
        var result = _service.IsPasswordStrong("StrongPass123!");
        Assert.True(result);
    }
}
