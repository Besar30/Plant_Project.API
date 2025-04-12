using Plant_Project.API;
using Serilog;

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
// Configure the HTTP request pipeline.
app.UseCors("AllowSpecificOrigins");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();
app.Run();