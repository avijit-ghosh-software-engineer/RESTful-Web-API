using MagicVilla_VillaAPI.Repository.IRepostiory;
using MagicVilla_VillaAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Extensions
{
    public static class ApiVersions
    {
        public static void ApiVersionsContainer(this WebApplicationBuilder builder)
        {
            builder.Services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            builder.Services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
        }
    }
}
