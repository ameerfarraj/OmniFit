using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmniFit.Infrastructure.Data;
using OmniFit.Core.Entities;
using OmniFit.Application.Services.Identity; // הוספנו את הגישה לשירותים שלנו
using OmniFit.Application.DTOs.Identity;

namespace OmniFit.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IUserRegistrationService _registrationService; // הוספת החוזה של שירות ההרשמה

    // הזרקת תלויות: המערכת תספק לנו גם את המסד וגם את הלוגיקה העסקית שרשמנו ב-Program.cs
    public UsersController(ApplicationDbContext context, IUserRegistrationService registrationService)
    {
        _context = context;
        _registrationService = registrationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _context.Users
            .Include(u => u.TraineeProfile)
            .Include(u => u.TrainerProfile)
            .ToListAsync();

        return Ok(users);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        // 1. הגנה לוגית: בדיקת גיל (COPPA/GDPR)
        if (!_registrationService.IsEligibleAge(dto.DateOfBirth))
        {
            return BadRequest("המשתמש חייב להיות בן 16 לפחות כדי להירשם.");
        }

        // 2. הגנה לוגית: בדיקת חוזק סיסמה
        if (!_registrationService.IsPasswordStrong(dto.Password))
        {
            return BadRequest("הסיסמה חלשה מדי. יש לוודא לפחות 8 תווים, אות גדולה, קטנה, מספר ותו מיוחד.");
        }

        // 3. מיפוי בטוח והצפנת הסיסמה (Hashing) עם BCrypt
        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password), // הפונקציה מצפינה את הסיסמה ללא דרך חזרה
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };

        // 4. שמירה בטוחה למסד
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        // 5. המרה ל-DTO תגובה כדי לא לחשוף את ה-Hash או שדות פנימיים החוצה
        var responseDto = new UserResponseDto
        {
            Id = newUser.Id,
            Email = newUser.Email,
            CreatedAt = newUser.CreatedAt
        };

        return Ok(responseDto);
    }
}