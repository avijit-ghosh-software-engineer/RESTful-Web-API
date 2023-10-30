using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepostiory;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly AppDbContext _context;

        public VillaRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }        

        public async Task<Villa> UpdateAsync(Villa villa)
        {
            _context.Villas.Update(villa);
            await _context.SaveChangesAsync();
            await _context.DisposeAsync();
            return villa;
        }
    }
}
