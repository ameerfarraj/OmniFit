using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmniFit.Infrastructure.Data;
using OmniFit.Core.Entities;

namespace OmniFit.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrainerProfilesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TrainerProfilesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // יצירת פרופיל מאמן חדש
    [HttpPost]
    public async Task<IActionResult> CreateTrainerProfile([FromBody] TrainerProfile newProfile)
    {
        // 1. בדיקת אבטחה: מוודאים שהמשתמש הראשי קיים
        var userExists = await _context.Users.AnyAsync(u => u.Id == newProfile.UserId);
        if (!userExists)
        {
            return NotFound("User not found. Cannot create trainer profile.");
        }

        // 2. הגנה מכפילויות (שלא נייצר שני פרופילים לאותו מאמן)
        var profileExists = await _context.TrainerProfiles.AnyAsync(p => p.UserId == newProfile.UserId);
        if (profileExists)
        {
            return Conflict("A trainer profile already exists for this user.");
        }

        // 3. עדכון זמן ושמירה
        newProfile.UpdatedAt = DateTime.UtcNow;

        _context.TrainerProfiles.Add(newProfile);
        await _context.SaveChangesAsync();

        return Ok(newProfile);
    }
}