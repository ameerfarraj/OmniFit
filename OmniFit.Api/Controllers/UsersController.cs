using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmniFit.Infrastructure.Data;
using OmniFit.Core.Entities;
using OmniFit.Application.Services.Identity;
using OmniFit.Application.DTOs.Identity;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace OmniFit.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IUserRegistrationService _registrationService;
    private readonly IAuthenticationService _authService;
    private readonly IConfiguration _configuration; // הוספנו את מנהל ההגדרות

    // עדכנו את הבנאי כדי לקבל את IConfiguration
    public UsersController(
        ApplicationDbContext context,
        IUserRegistrationService registrationService,
        IAuthenticationService authService,
        IConfiguration configuration)
    {
        _context = context;
        _registrationService = registrationService;
        _authService = authService;
        _configuration = configuration;
    }
    // ... המשך הקוד נשאר אותו דבר
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
        if (!_registrationService.IsEligibleAge(dto.DateOfBirth))
        {
            return BadRequest("המשתמש חייב להיות בן 16 לפחות כדי להירשם.");
        }

        if (!_registrationService.IsPasswordStrong(dto.Password))
        {
            return BadRequest("הסיסמה חלשה מדי. יש לוודא לפחות 8 תווים, אות גדולה, קטנה, מספר ותו מיוחד.");
        }

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        var responseDto = new UserResponseDto
        {
            Id = newUser.Id,
            Email = newUser.Email,
            CreatedAt = newUser.CreatedAt
        };

        return Ok(responseDto);
    }

    // --- חדש: Endpoint להתחברות ---
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
    {
        // 1. חיפוש המשתמש במסד הנתונים לפי האימייל
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null)
        {
            return Unauthorized("אימייל או סיסמה שגויים."); // לא נותנים רמז שהאימייל לא קיים כדי להקשות על תוקפים
        }

        // 2. בדיקת הסיסמה מול ה-Hash ששמור במסד
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

        // 3. הפעלת מנגנון הנעילה שלנו מה-Application Layer
        bool isLoginSuccessful = _authService.ProcessLoginAttempt(user, isPasswordValid);

        // חייבים לשמור את השינויים במסד (כי אולי העלינו את מונה הכישלונות או נעלנו את החשבון)
        await _context.SaveChangesAsync();

        if (!isLoginSuccessful)
        {
            if (user.IsLockedOut)
            {
                return Unauthorized($"החשבון ננעל עקב מספר רב של ניסיונות כושלים. אנא נסה שוב מאוחר יותר.");
            }
            return Unauthorized("אימייל או סיסמה שגויים.");
        }

        // 4. התחברות מוצלחת! יצירת הטוקן
        var token = GenerateJwtToken(user);

        // 5. הכנת התשובה ללקוח (הטוקן + פרטי המשתמש הבסיסיים)
        var response = new AuthResponseDto
        {
            Token = token,
            User = new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            }
        };

        return Ok(response);
    }

    // פונקציית עזר ליצירת הטוקן (JWT)
    private string GenerateJwtToken(User user)
    {
        // משיכת המפתח הסודי מקובץ ההגדרות (appsettings.json)
        var secretKeyString = _configuration["JwtSettings:SecretKey"];
        if (string.IsNullOrEmpty(secretKeyString))
        {
            throw new InvalidOperationException("JWT Secret Key is missing in configuration.");
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKeyString));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // הוספת מידע לתוך הטוקן (Claims)
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };

        // משיכת שאר ההגדרות מקובץ ה-JSON עם ערכי ברירת מחדל לביטחון
        var issuer = _configuration["JwtSettings:Issuer"] ?? "OmniFitAPI";
        var audience = _configuration["JwtSettings:Audience"] ?? "OmniFitClients";

        // ננסה לקרוא את זמן התפוגה מההגדרות, ואם נכשל נגדיר לשעתיים
        if (!double.TryParse(_configuration["JwtSettings:ExpirationHours"], out double expirationHours))
        {
            expirationHours = 2;
        }

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(expirationHours),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}