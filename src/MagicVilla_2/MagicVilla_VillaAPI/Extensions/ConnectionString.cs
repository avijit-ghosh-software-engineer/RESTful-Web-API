using MagicVilla_VillaAPI.Data;
using Microsoft.EntityFrameworkCore;
using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace MagicVilla_VillaAPI.Extensions
{
	public static class ConnectionString
    {
        public static void ConnectionStringContainer(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("VillApiConnectionString")));
			builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

		}
	}
}
