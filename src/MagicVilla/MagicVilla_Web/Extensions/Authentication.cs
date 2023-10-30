using Microsoft.AspNetCore.Authentication.Cookies;

namespace MagicVilla_Web.Extensions
{
    public static class Authentication
    {
        public static void AuthenticationContainer(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "oidc";
            }).AddCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = "/Auth/Login";
                options.AccessDeniedPath = "/Auth/AccessDenied";
                options.SlidingExpiration = true;
                //}).AddOpenIdConnect("oidc", options => {
                //options.Authority = builder.Configuration["ServiceUrls:IdentityAPI"];
                //options.GetClaimsFromUserInfoEndpoint = true;
                //options.ClientId = "magic";
                //options.ClientSecret = "secret";
                //options.ResponseType = "code";

                //options.TokenValidationParameters.NameClaimType = "name";
                //options.TokenValidationParameters.RoleClaimType = "role";
                //options.Scope.Add("magic");
                //options.SaveTokens = true;

                //options.ClaimActions.MapJsonKey("role", "role");

                //options.Events = new OpenIdConnectEvents
                //{
                //    OnRemoteFailure = context =>
                //    {
                //        context.Response.Redirect("/");
                //        context.HandleResponse();
                //        return Task.FromResult(0);
                //    }
                //};
            });
        }
    }
}
