using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmniFit.Infrastructure.Data;
using OmniFit.Core.Entities;

namespace OmniFit.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    // הזרקת תלויות - המערכת מעבירה לנו את מסד הנתונים
    public UsersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Endpoint ראשון: קריאת כל המשתמשים בבקשת רשת
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _context.Users.ToListAsync();
        return Ok(users); // מחזיר סטטוס 200 ואת הנתונים
    }

    // Endpoint שני: יצירת משתמש חדש
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] User newUser)
    {
        // הגדרת נתוני חובה מאחורי הקלעים
        newUser.Id = Guid.NewGuid();
        newUser.CreatedAt = DateTime.UtcNow;
        newUser.UpdatedAt = DateTime.UtcNow;

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return Ok(newUser); // מחזיר את המשתמש שנוצר
    }
}