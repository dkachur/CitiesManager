using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using CitiesManager.Core.RepositoryContracts;
using CitiesManager.Core.ServiceContracts;
using CitiesManager.Core.Services;
using CitiesManager.Infrastructure.DatabaseContext;
using CitiesManager.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.WebAPI.Extensions
{
    /// <summary>
    /// Provides extension methods for registering application services into the dependency injection container.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers all application services, repositories, database context, controllers, and Swagger for the API.
        /// </summary>
        /// <param name="services">
        /// The <see cref="IServiceCollection"/> instance to which the application's dependencies will be added.
        /// </param>
        /// <param name="configuration">
        /// The <see cref="IConfiguration"/> instance used to read application settings (e.g., database connection string).
        /// </param>
        /// <returns>
        /// The updated <see cref="IServiceCollection"/> instance for method chaining.
        /// </returns>
        /// <remarks>
        /// This method should be called in <c>Program.cs</c> or <c>Startup.cs</c> 
        /// to register all required services for the CitiesManager Web API.
        /// </remarks>
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICitiesAdderService, CitiesAdderService>();
            services.AddScoped<ICitiesGetterService, CitiesGetterService>();
            services.AddScoped<ICitiesUpdaterService, CitiesUpdaterService>();
            services.AddScoped<ICitiesDeleterService, CitiesDeleterService>();

            services.AddScoped<ICitiesRepository, CitiesRepository>();

            services.AddControllers(options =>
            {
                options.Filters.Add(new ProducesAttribute("application/json"));
                options.Filters.Add(new ConsumesAttribute("application/json"));
            });

            services.AddApiVersioning(options =>
            {
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });


            services.AddDbContext<ApplicationDbContext>(options =>
             {
                 options.UseSqlServer(configuration.GetConnectionString("Default"));
             });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml"));
                options.SwaggerDoc("v1", new() { Title = "Cities Web API", Version = "1.0"});
                options.SwaggerDoc("v2", new() { Title = "Cities Web API", Version = "2.0"});
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policyBuilder =>
                {
                    policyBuilder
                        .WithMethods("GET", "POST", "HEAD", "PUT", "DELETE")
                        .WithHeaders("Authorization", "content-type", "accept", "origin")
                        .WithOrigins(configuration.GetSection("AllowedOrigins")
                                                  .Get<string[]>() ?? []);
                });

                options.AddPolicy("DefClient", policyBuilder =>
                {
                    policyBuilder
                        .WithMethods("GET", "POST", "HEAD")
                        .WithHeaders("Authorization", "accept", "origin")
                        .WithOrigins(configuration.GetSection("AllowedClientOrigins")
                                                  .Get<string[]>() ?? []);
                });
            });
                
            return services;
        }
    }
}
