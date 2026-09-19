using System;
using System.Text.RegularExpressions;

namespace OmniFit.Application.Services.Identity;

public class UserRegistrationService : IUserRegistrationService
{
    // בקרת גיל: מוודא שהמשתמש בן 16 לפחות (COPPA/GDPR)
    public bool IsEligibleAge(DateOnly dateOfBirth)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var age = today.Year - dateOfBirth.Year;
        
        // תיקון למקרה שיום ההולדת עוד לא חל השנה
        if (dateOfBirth > today.AddYears(-age)) 
        {
            age--;
        }
        
        return age >= 16;
    }

    // מדיניות סיסמאות: לפחות 8 תווים, אות גדולה, קטנה, מספר ותו מיוחד
    public bool IsPasswordStrong(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 8) 
            return false;
        
        var hasUpper = new Regex(@"[A-Z]+");
        var hasLower = new Regex(@"[a-z]+");
        var hasNumber = new Regex(@"[0-9]+");
        var hasSpecial = new Regex(@"[\W_]+");

        return hasUpper.IsMatch(password) && 
               hasLower.IsMatch(password) && 
               hasNumber.IsMatch(password) && 
               hasSpecial.IsMatch(password);
    }
}
