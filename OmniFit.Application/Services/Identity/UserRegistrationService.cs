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

        // שימוש בקריאה סטטית שמשתמשת במטמון (Cache) פנימי ומונעת הקצאות זיכרון מיותרות
        return Regex.IsMatch(password, @"[A-Z]+") &&
               Regex.IsMatch(password, @"[a-z]+") &&
               Regex.IsMatch(password, @"[0-9]+") &&
               Regex.IsMatch(password, @"[\W_]+");
    }
}
