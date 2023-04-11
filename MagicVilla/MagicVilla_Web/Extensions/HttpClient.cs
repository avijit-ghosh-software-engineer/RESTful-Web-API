using MagicVilla_Web.Services.IServices;
using MagicVilla_Web.Services;

namespace MagicVilla_Web.Extensions
{
    public static class HttpClient
    {
        public static void HttpClientContainer(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpClient<IVillaService, VillaService>();
            builder.Services.AddHttpClient<IAuthService, AuthService>();
            builder.Services.AddHttpClient<IVillaNumberService, VillaNumberService>();
        }
    }
}
