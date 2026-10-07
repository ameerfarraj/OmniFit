using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmniFit.Api.Controllers;
using OmniFit.Application.Services.Identity;
using OmniFit.Core.Entities;
using OmniFit.Infrastructure.Data;
using Xunit;
using Microsoft.Extensions.Configuration;

namespace OmniFit.Tests;

public class UsersControllerTests
{
    [Fact]
    public async Task GetAllUsers_ReturnsOk_WithEagerLoading()
    {
        // 1. Arrange - הכנת השטח ומסד נתונים וירטואלי נקי
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);

        // נייצר משתמש מזויף עם פרופיל מתאמן ונשמור בזיכרון
        var fakeUserId = Guid.NewGuid();
        context.Users.Add(new User
        {
            Id = fakeUserId,
            Email = "test@omnifit.com",
            PasswordHash = "hash123",
            TraineeProfile = new TraineeProfile { UserId = fakeUserId, FirstName = "Test", LastName = "User" }
        });
        await context.SaveChangesAsync();

        // יצירת השירותים שהקונטרולר דורש
        var registrationService = new UserRegistrationService();
        var authService = new AuthenticationService();

        // התיקון: יצירת קונפיגורציה מזויפת לטובת הטסט כדי שהקונטרולר לא יקרוס
        var inMemorySettings = new Dictionary<string, string> {
            {"JwtSettings:SecretKey", "TestSecretKeyForUnitTestsOnlyDoNotUseInProd!!"},
            {"JwtSettings:Issuer", "TestIssuer"},
            {"JwtSettings:Audience", "TestAudience"}
        };
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        // הזרקת כל ארבעת הפרמטרים (מסד נתונים, שני השירותים, והגדרות) לקונטרולר
        var controller = new UsersController(context, registrationService, authService, configuration);

        // 2. Act - הפעלת הפונקציה (שליפת המשתמשים)
        var result = await controller.GetAllUsers();

        // 3. Assert - בדיקה שהכל עבד בדיוק כמו שצריך
        var okResult = Assert.IsType<OkObjectResult>(result); // מוודא שחזר סטטוס 200 OK
        var users = Assert.IsAssignableFrom<IEnumerable<User>>(okResult.Value); // מוודא שחזרה רשימת משתמשים
        var singleUser = Assert.Single(users); // מוודא שיש בדיוק משתמש אחד ומחזיר אותו
        Assert.NotNull(singleUser.TraineeProfile); // הקסם: מוודא שה-Include עבד
        Assert.Equal("Test", singleUser.TraineeProfile!.FirstName); // ה-! מרגיע את המהדר // מוודא שהנתונים נכונים
    }
}