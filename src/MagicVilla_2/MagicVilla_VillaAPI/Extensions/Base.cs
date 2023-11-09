using MagicVilla_VillaAPI.Repository.IRepostiory;
using MagicVilla_VillaAPI.Repository;

namespace MagicVilla_VillaAPI.Extensions
{
    public static class Base
    {
        public static void BaseContainer(this WebApplicationBuilder builder)
        {
            builder.DependencyContainer();
            builder.Services.AddAutoMapper(typeof(MappingConfig));
            builder.ApiVersionsContainer();
            builder.ConnectionStringContainer();
            builder.SwaggerGenContainer();
			builder.JWTAuthenticationContainer();
            builder.AddHealthChecks();
		}
    }
}
