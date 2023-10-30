namespace MagicVilla_Web.Extensions
{
    public static class Base
    {
        public static void BaseContainer(this WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(typeof(MappingConfig));
            builder.HttpClientContainer();
            builder.DependencyContainer();
            builder.SessionContainer();
            builder.AuthenticationContainer();
        }
    }
}
