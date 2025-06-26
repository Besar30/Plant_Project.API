
using Plant_Project.API.Const.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependecies(builder.Configuration);
builder.Services.AddDistributedMemoryCache();
//database
var ConnectionString = builder.Configuration.
    GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>options.UseSqlServer(ConnectionString));
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}
else
{
	app.UseExceptionHandler("/error");
	app.UseHsts();
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapHub<NotificationHub>("/notificationHub");
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();
app.MapHealthChecks("health");
app.Run();