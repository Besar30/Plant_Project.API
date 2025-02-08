
using Plant_Project.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependecies(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// url/jobs
app.UseHangfireDashboard("/jobs", new DashboardOptions
{
	Authorization =
	[
		//new HangfireCustomBasicAuthenticationFilter
		//{
		//	User = app.Configuration.GetValue<string>("HangfireSettings:Username"),
		//	Pass = app.Configuration.GetValue<string>("HangfireSettings:Password")
		//}
	],
	DashboardTitle = "PlantOpia Dashboard",
	//IsReadOnlyFunc = (DashboardContext conext) => true
});

app.UseAuthorization();

app.MapControllers();

app.Run();
