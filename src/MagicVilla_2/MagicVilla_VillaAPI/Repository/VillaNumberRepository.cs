using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepostiory;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly AppDbContext _context;

        public VillaNumberRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<VillaNumber> UpdateAsync(VillaNumber villaNumber)
        {
            _context.VillaNumbers.Update(villaNumber);
            await _context.SaveChangesAsync();
            await _context.DisposeAsync();
            return villaNumber;
        }
    }
}
