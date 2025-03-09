using MapsterMapper;
using Plant_Project.API.Settings;

namespace Plant_Project.API
{
    public static class DependancyInjection
    {
       public static IServiceCollection AddDependecies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();

            services.AddSwaggerServices();
            services.AddMapsterConfig();

			services.AddAuthConfig(configuration);

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
			//services.AddAuthentication()
			//     .AddGoogle(options =>
			//     {
			//      options.ClientId = configuration["Authentication:Google:ClientId"];
			//      options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
			//     });

			var ConnectionString = configuration.GetConnectionString("DefaultConnection");

			services.AddDbContext<ApplicationDbContext>(options => 
                options.UseSqlServer(ConnectionString));
            

			services.AddBackgroundJobsConfig(configuration);

			services.AddScoped<IAuthServices, AuthServices>();
			services.AddScoped<IPlantServices, PlantServices>();
			services.AddScoped<ICategoryServices, CategoryServices>();
			services.AddScoped<ICartServices, CartServices>();
			services.AddScoped<IPaymentService, PaymentService>();
			services.AddScoped<IEmailSender, EmailService>();
			services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
			services.AddScoped<PayAuthService>();

			services.AddValidationConfig();

			services.AddHttpContextAccessor();

			services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));

            services.AddHealthChecks()
                    .AddSqlServer(name: "database", connectionString: configuration.GetConnectionString("DefaultConnection")!)
                    .AddHangfire(options => { options.MinimumAvailableServers = 1; });

			return services; 
        }
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services){
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }
        
        public static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
        {
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
		private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
		{
			var mappingConfig = TypeAdapterConfig.GlobalSettings;
			mappingConfig.Scan(Assembly.GetExecutingAssembly());

			services.AddSingleton<IMapper>(new Mapper(mappingConfig));

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