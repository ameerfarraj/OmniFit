<div dir="rtl">



\# תרשים מחלקות (UML Class Diagram) - מודול משתמשים



\## 1. תרשים ויזואלי (Mermaid)

להלן תרשים המחלקות המייצג את שכבת הליבה (Core Entities) של המערכת, הממפה את היחסים בין המשתמש לפרופילים השונים.



```mermaid

classDiagram

&#x20;   class User {

&#x20;       +Guid Id

&#x20;       +string Email

&#x20;       +string PasswordHash

&#x20;       +int RoleId

&#x20;       +string SubscriptionTier

&#x20;   }

&#x20;   class TraineeProfile {

&#x20;       +Guid UserId

&#x20;       +string FirstName

&#x20;       +decimal CurrentWeightKG

&#x20;       +int TotalPoints

&#x20;   }

&#x20;   class TrainerProfile {

&#x20;       +Guid UserId

&#x20;       +string Bio

&#x20;       +string StripeConnectAccountId

&#x20;       +bool IsVerified

&#x20;   }

&#x20;   User "1" -- "0..1" TraineeProfile : has

&#x20;   User "1" -- "0..1" TrainerProfile : has

