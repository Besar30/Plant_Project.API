
using Plant_Project.API.Authentication.Filters;
using Plant_Project.API.Settings;

namespace Plant_Project.API
{
    public static class DependancyInjection
    {
       public static IServiceCollection AddDependecies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();

            services.AddAddSwaggerServices();

			services.AddAuthConfig(configuration);

			//database
			var ConnectionString = configuration.GetConnectionString("DefaultConnection");

			services.AddDbContext<ApplicationDbContext>(options => 
                options.UseSqlServer(ConnectionString));
            

			services.AddBackgroundJobsConfig(configuration);

			services.AddScoped<IAuthServices, AuthServices>();
			services.AddScoped<IEmailSender, EmailService>();
            services.AddScoped<IUserService, UserService>();

            services.AddValidationConfig();

			services.AddHttpContextAccessor();

			services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));

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
            //services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

            services.AddIdentity<ApplicationUser,ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
			services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

			services.AddSingleton<IJwtProvider, JwtProvider>();

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
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
            });
            return services;
        }
        public static IServiceCollection AddValidationConfig(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation().
               AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }
		private static IServiceCollection AddBackgroundJobsConfig(this IServiceCollection services,
		IConfiguration configuration)
		{
			services.AddHangfire(config => config
				.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
				.UseSimpleAssemblyNameTypeSerializer()
				.UseRecommendedSerializerSettings()
				.UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));

			services.AddHangfireServer();

			return services;
		}
	}
}