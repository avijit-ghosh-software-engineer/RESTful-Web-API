using MagicVilla_Web.Services.IServices;
using MagicVilla_Web.Services;

namespace MagicVilla_Web.Extensions
{
    public static class Dependency
    {
        public static void DependencyContainer(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IApiMessageRequestBuilder, ApiMessageRequestBuilder>();
            builder.Services.AddScoped<IBaseService, BaseService>();
            builder.Services.AddScoped<IVillaService, VillaService>();
            builder.Services.AddScoped<ITokenProvider, TokenProvider>();
            builder.Services.AddScoped<IVillaNumberService, VillaNumberService>();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<IAuthService, AuthService>();
        }
    }
}
