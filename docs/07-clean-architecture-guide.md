<div dir="rtl">

# מדריך ארכיטקטורת שכבות (Clean Architecture) - גישת Bottom-Up

מסמך זה מגדיר את מבנה הפיתוח של פרויקט OmniFit. כדי להבטיח קוד נקי, מודולרי ומוכן לטסטים (xUnit), אנו עובדים בגישת **Bottom-Up** (מלמטה למעלה) ו-**Domain-Driven Design (DDD)**. הגישה הזו שומרת על צימוד נמוך (Low Coupling) ולכידות גבוהה (High Cohesion).

## 1. שכבת הליבה (Domain Layer / Entities) - הבסיס התחתון
* **הסבר:** השכבה שבה אנחנו מתחילים את הפיתוח. אלו מחלקות נתונים טהורות (POCOs) המייצגות את העצמים העסקיים של המערכת (לדוגמה: `NutritionPlan`, `DailyLog`).
* **חוקי ברזל (Constraints):**
  * אין לוגיקה עסקית או חישובים מורכבים במחלקות אלו.
  * המחלקות סטטלסיות (Stateless) לחלוטין ואין להן תלויות (Dependencies) בספריות חיצוניות או במסד הנתונים.
  * הן מתקשרות אחת עם השנייה באמצעות Navigation Properties בלבד.

## 2. שכבת התשתיות (Infrastructure / Data Layer)
* **הסבר:** השכבה שאחראית על ניהול מסד הנתונים והתקשורת איתו (Entity Framework Core ו-PostgreSQL). 
* **חוקי ברזל (Constraints):**
  * כאן מוגדר ה-`OmniFitDbContext`.
  * השכבה הזו מייבאת את שכבה 1 (ה-Entities) וממפה אותן לטבלאות (DbSet).
  * השכבה הזו מכירה את הליבה, אך הליבה לעולם לא מכירה אותה (מונע תלויות מעגליות).

## 3. שכבת הלוגיקה העסקית (Application / Services Layer) - המוח
* **הסבר:** המקום שבו מתבצעת העבודה האמיתית, החישובים, והאלגוריתמים (לדוגמה: `NutritionCalculatorService`, `HealthAnalyzerService`).
* **חוקי ברזל (Constraints):**
  * עובדים בהפרדה של **ממשק מול מימוש (Interfaces vs. Implementations)**. תמיד ניצור קודם `INutritionCalculatorService` ורק אז את המחלקה שמממשת אותו.
  * השכבה מקבלת בקשות משכבת הקצה, פונה לשכבת התשתיות כדי לשלוף/לשמור Entities משכבה 1, ומבצעת עליהם את הלוגיקה (כמו חישוב TDEE).
  * שכבה זו אינה מכירה פרוטוקולי אינטרנט (כמו HTTP או JSON).

## 4. שכבת הקצה (Presentation / API Controllers) - העטיפה העליונה
* **הסבר:** השכבה שחשופה לעולם החיצון (אפליקציית המובייל). כאן יושבים ה-Controllers והראוטים (Endpoints).
* **חוקי ברזל (Constraints):**
  * ה-Controllers מכירים **אך ורק את ה-Interfaces** של שכבה 3 (באמצעות הזרקת תלויות - Dependency Injection).
  * תפקיד השכבה הוא רק לקבל בקשת HTTP, להעביר אותה לשירות הרלוונטי, ולהחזיר סטטוס קוד (200 OK / 400 Bad Request).
  * **איסור מוחלט:** ה-Controllers לעולם לא יבצעו חישובים מתמטיים ולעולם לא יפנו ישירות ל-`DbContext`.

</div>
