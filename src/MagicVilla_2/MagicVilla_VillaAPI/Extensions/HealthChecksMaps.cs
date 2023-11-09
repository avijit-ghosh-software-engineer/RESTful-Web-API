using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace MagicVilla_VillaAPI.Extensions
{
	public static class HealthChecksMaps
	{
		public static void AddHealthChecksMaps(this WebApplication app)
		{
			app.MapHealthChecks("/health", new HealthCheckOptions
			{
				ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
			});

			app.MapHealthChecks("/health/app", new HealthCheckOptions
			{
				Predicate = reg => reg.Tags.Contains("Application"),
				ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
			});

			app.MapHealthChecks("/health/db", new HealthCheckOptions
			{
				Predicate = reg => reg.Tags.Contains("Database"),
				ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
			});
			app.MapHealthChecks("/health/secure", new HealthCheckOptions
			{
				ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
			}).RequireAuthorization();
			app.MapHealthChecks("/health/cors", new HealthCheckOptions
			{
				ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
			}).RequireCors("AppCorsPolicy");

			app.UseCors("AppCorsPolicy");
			app.MapHealthChecksUI();
		}

	}
}
