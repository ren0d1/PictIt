namespace PictIt.Models
{
    using System;
    public class Search : IModel<Guid>
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid PictureId { get; set; }

        public DateTime Date { get; set; }
    }
}
