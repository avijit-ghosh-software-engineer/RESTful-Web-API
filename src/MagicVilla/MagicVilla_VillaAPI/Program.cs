using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Extensions;
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


builder.Services.AddControllers(options=> {
    options.CacheProfiles.Add("Default30",
       new CacheProfile()
       {
           Duration = 30
       });
    //options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddResponseCaching();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Magic_VillaV1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "Magic_VillaV2");
    });
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
