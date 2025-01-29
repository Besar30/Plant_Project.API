using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Plant_Project.API.Authentication;
using Plant_Project.API.Services;
using System.Reflection;
using System.Text;

namespace Plant_Project.API
{
    public static class DependancyInjection
    {
       public static IServiceCollection AddDependecies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddAddSwaggerServices();
            services.AddScoped<IAuthServices, AuthServices>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddAuthConfig(configuration);
            services.AddValidationConfig();
            return services; 
        }
        public static IServiceCollection AddAddSwaggerServices(this IServiceCollection services){
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }
        public static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
            services.AddIdentity<ApplicationUser,IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            var JwtSetting = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();
            services.AddOptions<JwtOptions>()
               .BindConfiguration(JwtOptions.SectionName)
               .ValidateDataAnnotations()
               .ValidateOnStart();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
        ).AddJwtBearer(o =>
        {
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSetting?.Key!)),
                ValidIssuer = JwtSetting?.Issuer,
                ValidAudience = JwtSetting?.Audience
            };

        });
            return services;
        }
        public static IServiceCollection AddValidationConfig(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation().
               AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}