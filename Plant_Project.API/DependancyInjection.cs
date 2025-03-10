
using Microsoft.AspNetCore.Http.Features;
using Plant_Project.API.Authentication.Filters;

namespace Plant_Project.API
{
    public static class DependancyInjection
    {
       public static IServiceCollection AddDependecies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();

            var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", builder =>
                {
                    builder.WithOrigins(allowedOrigins!) // ✅ Allow only defined origins
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials(); // ✅ Allow credentials like cookies/tokens
                });
            });

            services.AddAddSwaggerServices();
            services.AddScoped<IAuthServices, AuthServices>();
            services.AddScoped<IEmailSender,EmailService>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<ICategoryServices, CategoryServices>();
            services.AddScoped<IplantServices,plantServices>();
            services.AddScoped<IRoleServices, RoleServices>();
            services.AddAuthConfig(configuration);
            var config = TypeAdapterConfig.GlobalSettings;
            config.NewConfig<ApplicationUser, UserProfileResponse>();
            services.AddValidationConfig();
            services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));
            services.AddHttpContextAccessor();
            return services; 
        }
        public static IServiceCollection AddAddSwaggerServices(this IServiceCollection services){
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }
        //sdcnsdnc 
        public static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

            services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

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
            o.Events = new JwtBearerEvents
            {
                OnMessageReceived = ctx =>
                {
                    ctx.Request.Cookies.TryGetValue("accessToken", out var accessToken);
                    if (!string.IsNullOrEmpty(accessToken))
                        ctx.Token = accessToken;
                    return Task.CompletedTask;
                }
            };

        });
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                //options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
            });
            services.Configure<DataProtectionTokenProviderOptions>(options => {
                options.TokenLifespan = TimeSpan.FromHours(2);
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