using MagicVilla_VillaAPI.Repository.IRepostiory;
using MagicVilla_VillaAPI.Repository;

namespace MagicVilla_VillaAPI.Extensions
{
    public static class Dependency
    {
        public static void DependencyContainer(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IVillaRepository, VillaRepository>();
            builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
