namespace PictIt.Models
{
    using System;
    using System.Collections.Generic;

    public class FaceAttributes : IModel<Guid>
    {
        public Guid Id { get; set; }

        public ushort Age { get; set; }

        public string Gender { get; set; }

        public decimal Smile { get; set; }

        public FacialHair FacialHair { get; set; }

        public string Glasses { get; set; }

        public Emotion Emotion { get; set; }

        public Hair Hair { get; set; }

        public Makeup Makeup { get; set; }

        public Occlusion Occlusion { get; set; }

        public IEnumerable<Accessory> Accessories { get; set; }

        public Blur Blur { get; set; }

        public Exposure Exposure { get; set; }

        public Noise Noise { get; set; }
    }

    public class FacialHair : IModel<Guid>
    {
        public Guid Id { get; set; }

        public decimal Moustache { get; set; }

        public decimal Beard { get; set; }

        public decimal Sideburns { get; set; }
    }

    public class Emotion : IModel<Guid>
    {
        public Guid Id { get; set; }

        public decimal Anger { get; set; }

        public decimal Contempt { get; set; }

        public decimal Disgust { get; set; }

        public decimal Fear { get; set; }

        public decimal Happiness { get; set; }

        public decimal Neutral { get; set; }

        public decimal Sadness { get; set; }

        public decimal Surprise { get; set; }
    }

    public class Hair : IModel<Guid>
    {
        public Guid Id { get; set; }

        public decimal Bald { get; set; }
        
        public bool Invisible { get; set; }

        public IEnumerable<HairColor> HairColor { get; set; }
    }

    public class HairColor : IModel<Guid>
    {
        public Guid Id { get; set; }

        public string Color { get; set; }

        public decimal Confidence { get; set; }
    }

    public class Makeup : IModel<Guid>
    {
        public Guid Id { get; set; }

        public bool EyeMakeup { get; set; }

        public bool LipMakeup { get; set; }
    }

    public class Occlusion : IModel<Guid>
    {
        public Guid Id { get; set; }

        public bool ForeheadOccluded { get; set; }

        public bool EyeOccluded { get; set; }

        public bool MouthOccluded { get; set; }
    }

    public class Accessory : IModel<Guid>
    {
        public Guid Id { get; set; }

        public string Type { get; set; }

        public decimal Confidence { get; set; }
    }

    public class Blur : IModel<Guid>
    {
        public Guid Id { get; set; }

        public string BlurLevel { get; set; }

        public decimal Value { get; set; }
    }

    public class Exposure : IModel<Guid>
    {
        public Guid Id { get; set; }

        public string ExposureLevel { get; set; }

        public decimal Value { get; set; }
    }

    public class Noise : IModel<Guid>
    {
        public Guid Id { get; set; }

        public string NoiseLevel { get; set; }

        public decimal Value { get; set; }
    }
}
