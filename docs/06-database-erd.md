<div dir="rtl" align="right">

# שרטוט בסיס הנתונים (ERD) - מודול משתמשים והרשאות

בפרויקט זה אנו משתמשים ב-PostgreSQL. להלן מבנה הטבלאות (Schema) בפורמט טבלאי ברור:

## 1. טבלת משתמשים ואימות (Users)
טבלת הליבה. מכילה רק נתוני התחברות ואבטחה (ללא נתונים פיזיים)[cite: 2].

| שם השדה (Field) | סוג נתונים | אילוצים (Keys) | תיאור |
| :--- | :--- | :--- | :--- |
| **Id** | UUID | Primary Key | מזהה ייחודי מוצפן[cite: 2]. |
| **Email** | Varchar | Unique | כתובת אימייל (משמשת כשם משתמש)[cite: 2]. |
| **PasswordHash** | Varchar | | סיסמה מוצפנת (לא נשמור סיסמה גלויה לעולם)[cite: 2]. |
| **RoleId** | Int | Foreign Key | מזהה הרשאה: 1=מתאמן, 2=מאמן, 3=אדמין[cite: 2]. |
| **FailedLoginAttempts**| Int | | ספירת נסיונות כושלים (לצורך נעילת Brute-Force)[cite: 2]. |
| **CreatedAt** | DateTime | | תאריך פתיחת החשבון[cite: 2]. |
| **IsActive** | Boolean | | האם החשבון פעיל (לצורך מחיקת חשבון רכה - Soft Delete)[cite: 2]. |

## 2. טבלת פרופיל מתאמן (TraineeProfiles)
מכילה את נתוני שאלון ההתאמה (Onboarding) לטובת מנוע התזונה והאימונים[cite: 2].

| שם השדה (Field) | סוג נתונים | אילוצים (Keys) | תיאור |
| :--- | :--- | :--- | :--- |
| **UserId** | UUID | PK, FK | מפתח ראשי וגם מפתח זר המקושר לטבלת Users[cite: 2]. |
| **FirstName / LastName** | Varchar | | שם מלא[cite: 2]. |
| **BirthDate** | Date | | תאריך לידה (נחוץ לבקרת גיל ולנוסחת ה-BMR)[cite: 2]. |
| **Gender** | Varchar | | מין ביולוגי (לחישובי הוצאה קלורית)[cite: 2]. |
| **HeightCM** | Decimal | | גובה בסנטימטרים[cite: 2]. |
| **CurrentWeightKG** | Decimal | | משקל עדכני (יתעדכן אוטומטית מטבלת ה-Check-ins בהמשך)[cite: 2]. |
| **ActivityLevel** | Int | | רמת פעילות יומית (Multiplier לחישוב TDEE)[cite: 2]. |
| **DietaryPreference** | Varchar | | העדפת תזונה (לדוגמה: טבעוני, צמחוני, ללא גלוטן)[cite: 2]. |
| **Allergies** | Text | | רגישויות ואלרגיות לסינון אוטומטי במנוע התזונה[cite: 2]. |

## 3. טבלת פרופיל מאמן (TrainerProfiles)
מכילה נתונים ציבוריים שיוצגו בזירת המאמנים (Marketplace)[cite: 2].

| שם השדה (Field) | סוג נתונים | אילוצים (Keys) | תיאור |
| :--- | :--- | :--- | :--- |
| **UserId** | UUID | PK, FK | מפתח ראשי וגם מפתח זר המקושר לטבלת Users[cite: 2]. |
| **Bio** | Text | | ביוגרפיה / תיאור מקצועי[cite: 2]. |
| **PhoneNumber** | Varchar | | מספר טלפון ליצירת קשר (CRM)[cite: 2]. |
| **IsPhonePublic** | Boolean | | האם הטלפון גלוי לכולם או רק למתאמנים רשומים[cite: 2]. |
| **InstagramUrl / TikTokUrl**| Varchar | | קישורים לרשתות חברתיות (Deep Links)[cite: 2]. |

</div>