using MagicVilla_VillaAPI.Repository.IRepostiory;
using MagicVilla_VillaAPI.Repository;
using MagicVilla_VillaAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Extensions
{
    public static class ConnectionString
    {
        public static void ConnectionStringContainer(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("VillApiConnectionString")));
        }
    }
}
