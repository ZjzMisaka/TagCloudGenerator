using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace TagCloudGenerator
{
    public enum RotateType { Random, FromList }
    [SupportedOSPlatform("windows7.0")]
    public class TagCloudOption
    {
        public RotateType RotateType { get; set; }
        public float RandomRotateFrom { get; set; }
        public float RandomRotateTo { get; set; }
        public int Margin { get; set; }
        public List<int>? RotateList { get; set; }
        public List<Color>? FontColorList { get; set; }
        public Color BackgroundColor { get; set; }
        public FontFamily FontFamily { get; set; }
        public (float, float) FontSizeRange { get; set; }
        public double AngleStep { get; set; }
        public double RadiusStep { get; set; }
        public double AngleDecreaseFactor { get; set; }
        public double RadiusDecreaseFactor { get; set; }
        public bool IsRandomInitAngle { get; set; }
        public int CanvasHorizontalGrowthStep { get; set; }
        public int CanvasVerticalGrowthStep { get; set; }
        public ImgSize? OutputSize { get; set; }
        public ImgSize? InitSize { get; set; }

        public TagCloudOption()
        {
            RotateType = RotateType.FromList;
            RotateList = new List<int>() { 0 };
            FontColorList = new List<Color>() { Color.Black };
            BackgroundColor = Color.White;
            FontFamily = new FontFamily("Comic Sans MS");
            Margin = 0;
            AngleStep = 5;
            RadiusStep = 5;
            AngleDecreaseFactor = 1;
            RadiusDecreaseFactor = 0;
            IsRandomInitAngle = false;
            CanvasHorizontalGrowthStep = 10;
            CanvasVerticalGrowthStep = 10;
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
