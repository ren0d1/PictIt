namespace PictIt.Models
{
    using System;

    public class Picture : IModel<Guid>
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Face Face { get; set; }

        public string Extension { get; set; }
    }
}
