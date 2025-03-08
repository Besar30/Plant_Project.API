using Plant_Project.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependecies(builder.Configuration);
//database
var ConnectionString = builder.Configuration.
    GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>options.UseSqlServer(ConnectionString));

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
//app.UseStaticFiles();
app.MapControllers();
app.Run();