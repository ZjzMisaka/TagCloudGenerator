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
        public FontFamily FontFamily { get; set; }
        public (float, float) FontSizeRange { get; set; }

        public TagCloudOption()
        {
            RotateType = RotateType.FromList;
            RotateList = new List<int>() { 0 };
            FontColorList = new List<Color>() { Color.Black };
            FontFamily = new FontFamily("Comic Sans MS");
            Margin = 0;
        }
    }
}
