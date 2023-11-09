using MagicVilla_VillaAPI.HealthChecks;

namespace MagicVilla_VillaAPI.Extensions
{
	public static class HealthChecks
	{
		public static void AddHealthChecks(this WebApplicationBuilder builder)
		{
			builder.Services.AddCors(opt =>
			{
				opt.AddPolicy("AppCorsPolicy", builder =>
				{
					builder.AllowAnyOrigin()
						   .AllowAnyMethod()
						   .AllowAnyHeader();
				});
			});

			builder.Services.AddHealthChecks()
				.AddSqlServer(builder.Configuration.GetConnectionString("VillApiConnectionString"), tags: new[] { "Database_HealthChecks" })
				.AddCheck<AppHealthChecks>("AppHealthChecks", tags: new[] { "Application_HealthChecks" });

			builder.Services.AddHealthChecksUI().AddInMemoryStorage();
		}
	}
}
