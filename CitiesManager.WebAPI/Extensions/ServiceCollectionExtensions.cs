using CitiesManager.Core.RepositoryContracts;
using CitiesManager.Core.ServiceContracts;
using CitiesManager.Core.Services;
using CitiesManager.Infrastructure.DatabaseContext;
using CitiesManager.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.WebAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICitiesAdderService, CitiesAdderService>();
            services.AddScoped<ICitiesGetterService, CitiesGetterService>();
            services.AddScoped<ICitiesUpdaterService, CitiesUpdaterService>();
            services.AddScoped<ICitiesDeleterService, CitiesDeleterService>();

            services.AddScoped<ICitiesRepository, CitiesRepository>();

            services.AddControllers();

            services.AddDbContext<ApplicationDbContext>(options =>
             {
                 options.UseSqlServer(configuration.GetConnectionString("Default"));
             });

            return services;
        }
    }
}
