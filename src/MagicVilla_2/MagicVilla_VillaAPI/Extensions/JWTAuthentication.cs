using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MagicVilla_VillaAPI.Extensions
{
	public static class JWTAuthentication
    {
        public static void JWTAuthenticationContainer(this WebApplicationBuilder builder)
        {
			var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
					ValidateIssuer = true,
					ValidIssuer = "https://magicvilla-api.com",
					ValidAudience = "dotnetmastery.com",
					ValidateAudience = true,
					ClockSkew = TimeSpan.Zero
				};
            });
        }
    }
}
