using Microsoft.EntityFrameworkCore;
using OmniFit.Application.Services.Identity;
using OmniFit.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OmniFit.Application;

var builder = WebApplication.CreateBuilder(args);

// הגדרת הקונטרולרים
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

// המנגנון המובנה של .NET 9 לתיעוד (בלי Swashbuckle)
builder.Services.AddOpenApi();

// חיבור למסד הנתונים
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


// הזרקת השירותים
builder.Services.AddScoped<IUserRegistrationService, UserRegistrationService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// טעינת כל השירותים של שכבת הלוגיקה יחד (דרך קובץ ה-DependencyInjection שיצרנו)
builder.Services.AddApplication();
// הגדרות אימות ו-JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT Secret Key is missing.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// הפעלת הממשק המובנה של .NET 9
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// שומרי הסף שמוודאים את הטוקן
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();