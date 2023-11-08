using MagicVilla_VillaAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Extensions
{
	public static class DbMigration
	{
		public static void ApplyMigration(this WebApplication app)
		{
			using (var scope = app.Services.CreateScope())
			{
				var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

				if (_db.Database.GetPendingMigrations().Count() > 0)
				{
					_db.Database.Migrate();
				}
			}
		}
	}
}
