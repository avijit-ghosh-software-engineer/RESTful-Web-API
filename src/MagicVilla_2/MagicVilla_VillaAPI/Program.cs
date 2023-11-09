using MagicVilla_VillaAPI.Extensions;
using MagicVilla_VillaAPI.Filters;
using MagicVilla_VillaAPI.Middlewares;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// using serilog for logging into a file
//Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
//    .WriteTo.File("log/villa_logs.txt",rollingInterval:RollingInterval.Day).CreateLogger();
//builder.Host.UseSerilog();
// using serilog for logging into a file

// Add services to the container.

// Custom logging
//builder.Services.AddSingleton<ILogging, LoggingV2>();

builder.BaseContainer();

builder.Services.AddControllers(options => {
	//options.CacheProfiles.Add("Default30",
	//   new CacheProfile()
	//   {
	//	   Duration = 30
	//   });
	//options.ReturnHttpNotAcceptable = true;
	options.Filters.Add<CustomExceptionFilter>();
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters().
ConfigureApiBehaviorOptions(option =>
{
	option.ClientErrorMapping[StatusCodes.Status500InternalServerError] = new ClientErrorData
	{
		Link = "https://dotnetmastery.com/500"
	};
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddResponseCaching();




var app = builder.Build();

app.AddHealthChecksMaps();

app.UseSwagger();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwaggerUI(options => {
		options.SwaggerEndpoint("/swagger/v2/swagger.json", "Magic_VillaV2");
		options.SwaggerEndpoint("/swagger/v1/swagger.json", "Magic_VillaV1");
	});
}
else
{
	app.UseSwaggerUI(options => {
		options.SwaggerEndpoint("/swagger/v2/swagger.json", "Magic_VillaV2");
		options.SwaggerEndpoint("/swagger/v1/swagger.json", "Magic_VillaV1");
		options.RoutePrefix = "";
	});
}

//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//    dbContext.Database.EnsureCreated();
//}

//app.UseExceptionHandler("/ErrorHandling/ProcessError");
//app.HandleError(app.Environment.IsDevelopment());
app.UseMiddleware<CustomExceptionMiddleware>();

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.ApplyMigration();

app.Run();

