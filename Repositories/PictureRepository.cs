namespace PictIt.Repositories
{
    using System;

    using PictIt.Models;

    public class PictureRepository : BaseRepository<Picture, Guid>
    {
        public PictureRepository(AppDbContext context) : base(context)
        {
        }
    }
}
