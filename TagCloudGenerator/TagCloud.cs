using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace TagCloudGenerator
{
    [SupportedOSPlatform("windows7.0")]
    public class TagCloud
    {
        Bitmap bmp;

        public TagCloud(int width, int height, Dictionary<string, int> tagDic, TagCloudOption tagCloudOption)
        {
            if (tagCloudOption.RotateList == null)
            {
                throw new Exception("RotateList is null");
            }

            if (tagCloudOption.FontColorList == null)
            {
                throw new Exception("FontColorList is null");
            }

            bmp = new Bitmap(width, height);
            Random rnd = new Random();

            Graphics graphicsBmp = Graphics.FromImage(bmp);
            SolidBrush brush = new SolidBrush(Color.AliceBlue);
            string backgroundHex = HexConverter(brush.Color);
            graphicsBmp.FillRectangle(brush, 0, 0, width, height);
            graphicsBmp.Dispose();

            List<(int, int)> usedPointList = new List<(int, int)>();

            foreach (string tag in tagDic.Keys)
            {
                Spiral spiral = new Spiral(5, 5, isRandomInitAngle: true);
                int step = 0;

                float rotate = 0;
                if (tagCloudOption.RotateType == RotateType.FromList)
                {
                    rotate = tagCloudOption.RotateList[rnd.Next(0, tagCloudOption.RotateList.Count)];
                }
                else
                {
                    rotate = rnd.Next((int)tagCloudOption.RandomRotateFrom, (int)tagCloudOption.RandomRotateTo);
                }

                
                Font font = new Font(tagCloudOption.FontFamily != null ? tagCloudOption.FontFamily : new FontFamily("Comic Sans MS"), tagDic[tag]);

                SolidBrush fontBrush = new SolidBrush(tagCloudOption.FontColorList[rnd.Next(0, tagCloudOption.FontColorList.Count)]);

                bool isContinue = true;
                while (isContinue)
                {
                    Bitmap newBmp = new Bitmap(width, height);
                    Graphics graphics = Graphics.FromImage(newBmp);
                    graphics.FillRectangle(brush, 0, 0, width, height);
                    graphics.TranslateTransform(newBmp.Width / 2, newBmp.Height / 2);
                    graphics.RotateTransform(rotate);
                    SizeF textSize = graphics.MeasureString(tag, font);
                    (float, float) point = spiral.GetPoint(step);
                    ++step;
                    graphics.DrawString(tag, font, fontBrush, - textSize.Width / 2 + point.Item1, - textSize.Height / 2 + point.Item2);
                    graphics.Dispose();

                    isContinue = false;
                    List<(int, int)> newPointList = GetPointList(newBmp, width, height, backgroundHex);
                    foreach ((int, int) pointStr in newPointList)
                    {
                        if (usedPointList.Contains(pointStr))
                        {
                            isContinue = true;
                            break;
                        }
                    }
                    if (isContinue == false)
                    {
                        graphicsBmp = Graphics.FromImage(bmp);
                        graphicsBmp.TranslateTransform(newBmp.Width / 2, newBmp.Height / 2);
                        graphicsBmp.RotateTransform(rotate);
                        graphicsBmp.DrawString(tag, font, fontBrush, -textSize.Width / 2 + point.Item1, -textSize.Height / 2 + point.Item2);
                        graphicsBmp.Dispose();
                        usedPointList.AddRange(newPointList);
                    }
                }
            }
        }

        public Bitmap Get()
        {
            return bmp;
        }

        private string HexConverter(System.Drawing.Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        private List<(int, int)> GetPointList(Bitmap newBmp, int width, int height, string backgroundHex)
        {
            List<(int, int)> pointList = new List<(int, int)>();
            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    if (HexConverter(newBmp.GetPixel(i, j)) != backgroundHex)
                    {
                        pointList.Add((i, j));
                    }
                }
            }

            return pointList;
        } 
    }
}
