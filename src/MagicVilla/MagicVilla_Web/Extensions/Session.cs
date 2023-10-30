namespace MagicVilla_Web.Extensions
{
    public static class Session
    {
        public static void SessionContainer(this WebApplicationBuilder builder)
        {
            builder.Services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(100);
                options.Cookie.IsEssential = true;
                options.Cookie.HttpOnly = true;
            });
        }
    }
}
