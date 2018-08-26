namespace PictIt.Repositories
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using PictIt.Models;

    public class PictureRepository : BaseRepository<Picture, Guid>
    {
        private readonly AppDbContext _context;

        public PictureRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Picture> GetById(Guid id)
        {
            var pictures = _context.Pictures.Include(p => p.Face).ThenInclude(f => f.FaceAttributes).ThenInclude(fa => fa.Emotion);

            return await pictures.FirstOrDefaultAsync(o => o.Id.Equals(id));
        }
    }
}
