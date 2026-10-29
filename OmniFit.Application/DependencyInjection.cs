using Microsoft.Extensions.DependencyInjection;
using OmniFit.Application.Services;
using OmniFit.Application.Services.Interfaces;

namespace OmniFit.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // כאן אנחנו רושמים את כל השירותים של שכבת האפליקציה
            services.AddScoped<ITraineeProfileService, TraineeProfileService>();

            return services;
        }
    }
}