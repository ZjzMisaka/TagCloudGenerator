using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace TagCloudGenerator
{
    public class Range 
    {
        public float From { get; set; }
        public float To { get; set; }
        public Range(float from, float to) 
        {
            From = from;
            To = to;
        }
    }

    [SupportedOSPlatform("windows7.0")]
    public class TagCloudOption
    {
        public Range RandomRotateRange { get; set; }
        public List<int> RotateList { get; set; }
        public int TagSpacing { get; set; }
        public int HorizontalOuterMargin { get; set; }
        public int VerticalOuterMargin { get; set; }
        public List<Color> FontColorList { get; set; }
        public ColorOption BackgroundColor { get; set; }
        public FontFamily FontFamily { get; set; }
        public Range FontSizeRange { get; set; }
        public double AngleStep { get; set; }
        public double RadiusStep { get; set; }
        public double AngleDecreaseFactor { get; set; }
        public double RadiusDecreaseFactor { get; set; }
        public bool IsRandomInitAngle { get; set; }
        public int HorizontalCanvasGrowthStep { get; set; }
        public int VerticalCanvasGrowthStep { get; set; }
        public ImgSize OutputSize { get; set; }
        public ImgSize InitSize { get; set; }
        public string MaskPath { get; set; }
        public bool ShowMask { get; set; }
        public bool StretchMask { get; set; }
        public int Accuracy { get; set; }

        public class ColorOption
        {
            public ColorOption(Color color)
            {
                Value = color;
            }

            public Color Value { get; set; }
        }

        public TagCloudOption()
        {
            RotateList = new List<int>() { 0 };
            FontColorList = new List<Color>() { Color.Black };
            FontFamily = new FontFamily("Comic Sans MS");
            TagSpacing = 0;
            HorizontalOuterMargin = 0;
            VerticalOuterMargin = 0;
            AngleStep = 5;
            RadiusStep = 5;
            AngleDecreaseFactor = 1;
            RadiusDecreaseFactor = 0;
            IsRandomInitAngle = false;
            HorizontalCanvasGrowthStep = 10;
            VerticalCanvasGrowthStep = 10;
            Accuracy = 1;
        }
    }

    public class ImgSize
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public ImgSize(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }
    }
}
