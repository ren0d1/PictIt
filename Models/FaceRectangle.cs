namespace PictIt.Models
{
    using System;

    public class FaceRectangle : IModel<Guid>
    {
        public Guid Id { get; set; }

        public ushort Top { get; set; }

        public ushort Left { get; set; }

        public ushort Width { get; set; }

        public ushort Height { get; set; }
    }
}
