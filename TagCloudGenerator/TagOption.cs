using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagCloudGenerator
{
    public class TagOption
    {
        public Rotate Rotate { get; set; }
        public FontColor FontColor { get; set; }
        public FontFamily FontFamily { get; set; }
    }

    public class Rotate
    {
        public Rotate(float rotate)
        {
            Value = rotate;
        }

        public float Value { get; set; }
    }

    public class FontColor
    {
        public FontColor(Color fontColor)
        {
            Value = fontColor;
        }

        public Color Value { get; set; }
    }
}
