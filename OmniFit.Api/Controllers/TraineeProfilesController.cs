using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmniFit.Infrastructure.Data;
using OmniFit.Core.Entities;

namespace OmniFit.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TraineeProfilesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TraineeProfilesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // יצירת פרופיל מתאמן חדש
    [HttpPost]
    public async Task<IActionResult> CreateProfile([FromBody] TraineeProfile newProfile)
    {
        // 1. בדיקת אבטחה: האם המשתמש הראשי בכלל קיים במערכת?
        var userExists = await _context.Users.AnyAsync(u => u.Id == newProfile.UserId);
        if (!userExists)
        {
            return NotFound("User not found. Cannot create profile without a valid user.");
        }
        // 1.5. בדיקת מניעת כפילויות (הגנה מפני לחיצה כפולה)
        var profileExists = await _context.TraineeProfiles.AnyAsync(p => p.UserId == newProfile.UserId);
        if (profileExists)
        {
            return Conflict("A profile already exists for this user.");
        }

        // 2. עדכון חותמת זמן
        newProfile.UpdatedAt = DateTime.UtcNow;

        // 3. שמירה למסד הנתונים
        _context.TraineeProfiles.Add(newProfile);
        await _context.SaveChangesAsync();

        return Ok(newProfile);
    }
}