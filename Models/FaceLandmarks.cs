namespace PictIt.Models
{
    using System;

    public class FaceLandmarks : IModel<Guid>
    {
        public Guid Id { get; set; }

        public Coords PupilLeft { get; set; }

        public Coords PupilRight { get; set; }

        public Coords NoseTip { get; set; }

        public Coords MouthLeft { get; set; }

        public Coords MouthRight { get; set; }

        public Coords EyebrowLeftOuter { get; set; }

        public Coords EyebrowLeftInner { get; set; }

        public Coords EyeLeftOuter { get; set; }

        public Coords EyeLeftTop { get; set; }

        public Coords EyeLeftBottom { get; set; }

        public Coords EyeLeftInner { get; set; }

        public Coords EyebrowRightOuter { get; set; }

        public Coords EyebrowRightInner { get; set; }

        public Coords EyeRightOuter { get; set; }

        public Coords EyeRightTop { get; set; }

        public Coords EyeRightBottom { get; set; }

        public Coords EyeRightInner { get; set; }

        public Coords NoseRootLeft { get; set; }

        public Coords NoseRootRight { get; set; }

        public Coords NoseLeftAlarTop { get; set; }

        public Coords NoseRightAlarTop { get; set; }

        public Coords NoseLeftAlarOutTip { get; set; }

        public Coords NoseRightAlarOutTip { get; set; }

        public Coords UpperLipTop { get; set; }

        public Coords UpperLipBottom { get; set; }

        public Coords UnderLipTop { get; set; }

        public Coords UnderLipBottom { get; set; }
    }

    public class Coords : IModel<Guid>
    {
        public Guid Id { get; set; }

        public decimal X { get; set; }

        public decimal Y { get; set; }
    }
}
