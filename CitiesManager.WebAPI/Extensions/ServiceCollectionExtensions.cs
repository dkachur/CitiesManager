using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using CitiesManager.Core.RepositoryContracts;
using CitiesManager.Core.ServiceContracts;
using CitiesManager.Core.Services;
using CitiesManager.Infrastructure.DatabaseContext;
using CitiesManager.Infrastructure.Identity;
using CitiesManager.Infrastructure.Options;
using CitiesManager.Infrastructure.Repositories;
using CitiesManager.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
            var jwtSection = configuration.GetSection("Jwt");

            services.Configure<JwtOptions>(jwtSection);
            services.Configure<RefreshTokenOptions>(configuration.GetSection("RefreshToken"));

            services.AddScoped<ICitiesAdderService, CitiesAdderService>();
            services.AddScoped<ICitiesGetterService, CitiesGetterService>();
            services.AddScoped<ICitiesUpdaterService, CitiesUpdaterService>();
            services.AddScoped<ICitiesDeleterService, CitiesDeleterService>();

            services.AddScoped<ICitiesRepository, CitiesRepository>();

            services.AddScoped<IJwtService, JwtService>();

            services.AddControllers(options =>
            {
                options.Filters.Add(new ProducesAttribute("application/json"));
                options.Filters.Add(new ConsumesAttribute("application/json"));

                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.Filters.Add(new AuthorizeFilter(policy));
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

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
                .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    JwtOptions jwtOptions = jwtSection.Get<JwtOptions>()
                        ?? throw new InvalidOperationException("Jwt configuration section is missing.");

                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
                    };
                });

            services.AddAuthorization();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml"));
                options.SwaggerDoc("v1", new() { Title = "Cities Web API", Version = "1.0" });
                options.SwaggerDoc("v2", new() { Title = "Cities Web API", Version = "2.0" });
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
