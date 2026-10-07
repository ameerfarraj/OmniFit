using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmniFit.Infrastructure.Data;
using OmniFit.Core.Entities;
using OmniFit.Application.Services.Identity; // הוספנו את הגישה לשירותים שלנו

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

    [HttpPost("register")] // הגדרנו נתיב ברור יותר להרשמה
    public async Task<IActionResult> CreateUser([FromBody] User newUser)
    {
        // 1. הפעלת שכבת ה-Application: בדיקת חוקים עסקיים לפני שנוגעים במסד!
        if (!_registrationService.IsPasswordStrong(newUser.PasswordHash))
        {
            return BadRequest("הסיסמה חלשה מדי. יש לוודא לפחות 8 תווים, אות גדולה, קטנה, מספר ותו מיוחד.");
        }

        // הערה: את בדיקת הגיל נפעיל בהמשך כשנוסיף DTO (אובייקט העברת נתונים) שמכיל את תאריך הלידה מהלקוח

        // 2. הגדרת נתוני חובה
        newUser.Id = Guid.NewGuid();
        newUser.CreatedAt = DateTime.UtcNow;
        newUser.UpdatedAt = DateTime.UtcNow;

        // 3. שמירה למסד נתונים רק אחרי שהכל תקין
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return Ok(newUser);
    }
}