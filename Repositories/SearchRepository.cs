namespace PictIt.Repositories
{
    using System;

    using PictIt.Models;

    public class SearchRepository : BaseRepository<Search, Guid>
    {
        public SearchRepository(AppDbContext context) : base(context)
        {
        }
    }
}
