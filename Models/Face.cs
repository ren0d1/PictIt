namespace PictIt.Models
{
    using System;

    public class Face : IModel<Guid>
    {
        public Guid Id { get; set; }

        public string FaceId { get; set; }

        public FaceRectangle FaceRectangle { get; set; }

        public FaceLandmarks FaceLandmarks { get; set; }

        public FaceAttributes FaceAttributes { get; set; }
    }
}
