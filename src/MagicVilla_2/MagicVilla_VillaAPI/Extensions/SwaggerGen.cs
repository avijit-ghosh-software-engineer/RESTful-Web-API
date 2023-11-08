using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MagicVilla_VillaAPI.Extensions
{
	public static class SwaggerGen
    {
        public static void SwaggerGenContainer(this WebApplicationBuilder builder)
        {
			builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
			builder.Services.AddSwaggerGen();
		}
    }
}
