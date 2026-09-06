<div dir="rtl" align="right">

# שרטוט בסיס הנתונים (ERD) - מודול משתמשים והרשאות

בפרויקט זה אנו משתמשים ב-PostgreSQL. להלן מבנה הטבלאות (Schema) בפורמט טבלאי ברור. 
*הערת ארכיטקטורה (Scale): על כל שדות ה-Unique והמפתחות הזרים יוגדרו Indexes ב-DB לשליפה מהירה (O(log n)) תחת עומס גבוה.*

## 1. טבלת משתמשים ואימות (Users)
טבלת הליבה. מכילה רק נתוני התחברות, אבטחה וסטטוס מנוי.

| שם השדה (Field) | סוג נתונים | אילוצים (Keys) | תיאור |
| :--- | :--- | :--- | :--- |
| **Id** | UUID | Primary Key | מזהה ייחודי מוצפן. |
| **Email** | Varchar | Unique, Index | כתובת אימייל (משמשת כשם משתמש). |
| **PasswordHash** | Varchar | | סיסמה מוצפנת. |
| **GoogleAuthId** | Varchar | Unique, Index | מזהה ייחודי להתחברות דרך חשבון Google. |
| **AppleAuthId** | Varchar | Unique, Index | מזהה ייחודי להתחברות דרך חשבון Apple. |
| **RoleId** | Int | Foreign Key | מזהה הרשאה: 1=מתאמן, 2=מאמן, 3=אדמין. |
| **SubscriptionTier**| Varchar | | סטטוס מנוי (למשל: Basic, Premium). קריטי למודל העסקי. |
| **StripeCustomerId** | Varchar | Unique, Index | מזהה לקוח במערכת הסליקה החיצונית (לביצוע חיובים בחנות או מנוי). |
| **FailedLoginAttempts**| Int | | ספירת נסיונות כושלים (לצורך נעילת Brute-Force). |
| **CreatedAt** | DateTime | | תאריך פתיחת החשבון. |
| **UpdatedAt** | DateTime | | חותמת זמן של העדכון האחרון (קריטי לסנכרון Offline). |
| **IsActive** | Boolean | | האם החשבון פעיל (לצורך מחיקת חשבון רכה - Soft Delete). |

## 2. טבלת פרופיל מתאמן (TraineeProfiles)
מכילה את נתוני שאלון ההתאמה (Onboarding), מדדים רפואיים וגיימיפיקציה.

| שם השדה (Field) | סוג נתונים | אילוצים (Keys) | תיאור |
| :--- | :--- | :--- | :--- |
| **UserId** | UUID | PK, FK | מפתח ראשי וגם מפתח זר המקושר לטבלת Users. |
| **FirstName / LastName** | Varchar | | שם מלא. |
| **ProfileImageUrl** | Varchar | | קישור (URL) לתמונת הפרופיל בשרת האחסון (AWS S3). |
| **BirthDate** | Date | | תאריך לידה (נחוץ לבקרת גיל ולנוסחת ה-BMR). |
| **Gender** | Varchar | | מין ביולוגי (לחישובי הוצאה קלורית). |
| **HeightCM** | Decimal | | גובה בסנטימטרים. |
| **CurrentWeightKG** | Decimal | | משקל עדכני (מתעדכן מ-Check-ins). |
| **ActivityLevel** | Int | | רמת פעילות יומית (Multiplier לחישוב TDEE). |
| **DietaryPreference** | Varchar | | העדפת תזונה (טבעוני, צמחוני, ללא גלוטן). |
| **Allergies** | Text | | רגישויות ואלרגיות לסינון במנוע התזונה. |
| **TotalPoints** | Int | | סך הנקודות שנצברו בגיימיפיקציה (עבור ה-Leaderboard). |
| **CurrentStreak** | Int | | רצף ימים של עמידה ביעדים. |
| **LanguageCode** | Varchar | | שפת ממשק מועדפת (למשל: he, en, ar) - תשתית i18n. |
| **Timezone** | Varchar | | אזור זמן (למשל: Asia/Jerusalem) לתזמון התראות פוש. |
| **UpdatedAt** | DateTime | | חותמת זמן של העדכון האחרון בפרופיל. |

## 3. טבלת פרופיל מאמן (TrainerProfiles)
מכילה נתונים ציבוריים שיוצגו בזירת המאמנים (Marketplace).

| שם השדה (Field) | סוג נתונים | אילוצים (Keys) | תיאור |
| :--- | :--- | :--- | :--- |
| **UserId** | UUID | PK, FK | מפתח ראשי וגם מפתח זר המקושר לטבלת Users. |
| **ProfileImageUrl** | Varchar | | קישור (URL) לתמונת הפרופיל של המאמן. |
| **Bio** | Text | | ביוגרפיה / תיאור מקצועי. |
| **PhoneNumber** | Varchar | | מספר טלפון ליצירת קשר (CRM). |
| **IsPhonePublic** | Boolean | | האם הטלפון גלוי לכולם או רק למתאמנים רשומים. |
| **InstagramUrl / TikTokUrl**| Varchar | | קישורים לרשתות חברתיות (Deep Links). |
| **LanguageCode** | Varchar | | שפת ממשק מועדפת. |
| **UpdatedAt** | DateTime | | חותמת זמן של העדכון האחרון בפרופיל. |
| **StripeConnectAccountId** | Varchar | Unique | מזהה חשבון סליקה של המאמן (לקבלת תשלומים אחרי ניכוי עמלת פלטפורמה). |
| **IsVerified** | Boolean | | האם האדמין אימת ואישר את המאמן (חובה כדי לאפשר לו לקבל מתאמנים ולהציג מוצרים). |

</div>