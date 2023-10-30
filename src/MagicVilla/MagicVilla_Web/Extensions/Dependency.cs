

using MagicVilla_Web.Services.IServices;
using MagicVilla_Web.Services;

namespace MagicVilla_Web.Extensions
{
    public static class Dependency
    {
        public static void DependencyContainer(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IVillaService, VillaService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IVillaNumberService, VillaNumberService>();
        }
    }
}
