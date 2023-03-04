using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace TagCloudGenerator
{
    [SupportedOSPlatform("windows7.0")]
    public class TagCloud
    {
        Bitmap bmp;
        TagCloudOption tagCloudOption;

        public TagCloud(Dictionary<string, float> tagDic, TagCloudOption tagCloudOption, Dictionary<string, TagOption>? tagOptionDic = null)
        {
            this.tagCloudOption = tagCloudOption;

            int width;
            int height;
            if (tagCloudOption.InitSize != null)
            {
                width = tagCloudOption.InitSize.Width;
                height = tagCloudOption.InitSize.Height;
            }
            else
            {
                width = tagCloudOption.HorizontalCanvasGrowthStep;
                height = tagCloudOption.VerticalCanvasGrowthStep;
            }

            if (tagCloudOption.RotateList == null)
            {
                throw new Exception("RotateList is null");
            }

            if (tagCloudOption.FontColorList == null)
            {
                throw new Exception("FontColorList is null");
            }

            if (tagCloudOption.FontSizeRange != (0, 0))
            {
                float minFontSize = tagCloudOption.FontSizeRange.Item1;
                float maxFontSize = tagCloudOption.FontSizeRange.Item2;

                float range = maxFontSize - minFontSize;

                float min = tagDic.Values.ElementAt(0);
                string minKey = tagDic.Keys.ElementAt(0);
                float max = tagDic.Values.ElementAt(0);
                foreach (string key in tagDic.Keys)
                {
                    float count = tagDic[key];
                    if (count < min)
                    {
                        min = count;
                        minKey = key;
                    }
                    if (count > max)
                    {
                        max = count;
                    }
                }

                if ((max - min) > range)
                {
                    float ratio = range / (max - min);
                    foreach (string key in tagDic.Keys)
                    {
                        tagDic[key] = tagDic[key] * ratio;
                    }
                }

                min = tagDic[minKey];
                if (min < minFontSize)
                {
                    float needAdd = minFontSize - min;
                    foreach (string key in tagDic.Keys)
                    {
                        tagDic[key] = tagDic[key] + needAdd;
                    }
                }
            }

            bmp = new Bitmap(width, height);
            Random rnd = new Random();

            Graphics graphicsBmp = Graphics.FromImage(bmp);

            graphicsBmp.Dispose();

            Dictionary<float, Font> fontDic = new Dictionary<float, Font>();

            foreach (string tag in tagDic.Keys)
            {
                if (tag.Replace(" ", "").Replace("　", "") == "")
                {
                    continue;
                }

                Spiral spiral = new Spiral(tagCloudOption.AngleStep, tagCloudOption.RadiusStep, tagCloudOption.AngleDecreaseFactor, tagCloudOption.RadiusDecreaseFactor, tagCloudOption.IsRandomInitAngle);
                int step = 0;

                float rotate = 0;
                if (tagOptionDic != null && tagOptionDic.ContainsKey(tag) && tagOptionDic[tag].Rotate.HasValue)
                {
                    rotate = tagOptionDic[tag].Rotate.GetValueOrDefault();
                }
                else
                {
                    if (tagCloudOption.RotateType == RotateType.FromList)
                    {
                        rotate = tagCloudOption.RotateList[rnd.Next(0, tagCloudOption.RotateList.Count)];
                    }
                    else
                    {
                        rotate = rnd.Next((int)tagCloudOption.RandomRotateFrom, (int)tagCloudOption.RandomRotateTo);
                    }
                }

                Font font;
                if (fontDic.ContainsKey(tagDic[tag]))
                {
                    font = fontDic[tagDic[tag]];
                }
                else
                {
                    if (tagOptionDic != null && tagOptionDic.ContainsKey(tag) && tagOptionDic[tag].FontFamily != null)
                    {
                        font = new Font(tagOptionDic[tag].FontFamily, tagDic[tag]);
                    }
                    else
                    {
                        font = fontDic[tagDic[tag]] = new Font(tagCloudOption.FontFamily != null ? tagCloudOption.FontFamily : new FontFamily("Comic Sans MS"), tagDic[tag]);
                    }
                }

                SolidBrush fontBrush;
                if (tagOptionDic != null && tagOptionDic.ContainsKey(tag) && tagOptionDic[tag].FontColor.HasValue)
                {
                    fontBrush = new SolidBrush(tagOptionDic[tag].FontColor.GetValueOrDefault());
                }
                else
                {
                    fontBrush = new SolidBrush(tagCloudOption.FontColorList[rnd.Next(0, tagCloudOption.FontColorList.Count)]);
                }

                while (true)
                {
                    Bitmap newBmp = new Bitmap(width, height);
                    Graphics graphics = Graphics.FromImage(newBmp);
                    // graphics.FillRectangle(backgroundColorBrush, 0, 0, width, height);
                    graphics.TranslateTransform(newBmp.Width / 2, newBmp.Height / 2);
                    graphics.RotateTransform(rotate);
                    SizeF textSize = graphics.MeasureString(tag, font);
                    (float, float) point = spiral.GetPoint(step);
                    ++step;
                    graphics.DrawString(tag, font, fontBrush, point.Item1 - textSize.Width / 2, point.Item2 - textSize.Height / 2);

                    float beforeRotatedX = point.Item1 - textSize.Width / 2;
                    float beforeRotatedY = point.Item2 - textSize.Height / 2;
                    PointF[] points = { new PointF(beforeRotatedX, beforeRotatedY), new PointF(beforeRotatedX + textSize.Width, beforeRotatedY), new PointF(beforeRotatedX, beforeRotatedY + textSize.Height), new PointF(beforeRotatedX + textSize.Width, beforeRotatedY + textSize.Height) };
                    points = GetRotatedPoints(points, rotate);

                    for (int i = 1; i <= tagCloudOption.TagSpacing; ++i)
                    {
                        graphics.DrawString(tag, font, fontBrush, point.Item1 - textSize.Width / 2 - i, point.Item2 - textSize.Height / 2);
                        graphics.DrawString(tag, font, fontBrush, point.Item1 - textSize.Width / 2 + i, point.Item2 - textSize.Height / 2);
                        graphics.DrawString(tag, font, fontBrush, point.Item1 - textSize.Width / 2, point.Item2 - textSize.Height / 2 - i);
                        graphics.DrawString(tag, font, fontBrush, point.Item1 - textSize.Width / 2, point.Item2 - textSize.Height / 2 + i);
                    }

                    graphics.Dispose();

                    if (HasOutOfBounds(points, textSize, rotate, width, height))
                    {
                        width = width + tagCloudOption.HorizontalCanvasGrowthStep;
                        height = height + tagCloudOption.VerticalCanvasGrowthStep;
                        Bitmap biggerBitmap = new Bitmap(width, height);
                        Graphics biggerGraphics = Graphics.FromImage(biggerBitmap);
                        //biggerGraphics.FillRectangle(backgroundColorBrush, 0, 0, width, height);
                        biggerGraphics.DrawImage(bmp, (width - bmp.Width) / 2, (height - bmp.Height) / 2, bmp.Width, bmp.Height);
                        graphicsBmp.Dispose();
                        bmp.Dispose();
                        bmp = biggerBitmap;
                        newBmp.Dispose();
                        --step;
                        continue;
                    }

                    if (HasOverlap(bmp, newBmp, width, height, points))
                    {
                        newBmp.Dispose();
                        continue;
                    }

                    graphicsBmp = Graphics.FromImage(bmp);
                    graphicsBmp.TranslateTransform(bmp.Width / 2, bmp.Height / 2);
                    graphicsBmp.RotateTransform(rotate);
                    graphicsBmp.DrawString(tag, font, fontBrush, -textSize.Width / 2 + point.Item1, -textSize.Height / 2 + point.Item2);
                    graphicsBmp.Dispose();
                    newBmp.Dispose();
                    break;
                }
            }
        }

        public Bitmap Get()
        {
            int width = bmp.Width;
            int height = bmp.Height;
            if (tagCloudOption.OutputSize != null)
            {
                width = tagCloudOption.OutputSize.Width;
                height = tagCloudOption.OutputSize.Height;
            }
            Bitmap biggerBitmap = new Bitmap(width, height);
            Graphics biggerGraphics = Graphics.FromImage(biggerBitmap);
            if (tagCloudOption.BackgroundColor != null)
            {
                SolidBrush backgroundColorBrush = new SolidBrush((Color)tagCloudOption.BackgroundColor);
                biggerGraphics.FillRectangle(backgroundColorBrush, 0, 0, width, height);
            }
            biggerGraphics.DrawImage(bmp, 0, 0, width, height);
            bmp = biggerBitmap;
            return bmp;
        }

        private bool HasOutOfBounds(PointF[] rotatedPoints, SizeF textSize, float rotate, int width, int height)
        {
            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;
            foreach (PointF point in rotatedPoints)
            {
                if (point.X > maxX)
                {
                    maxX = point.X;
                }
                if (point.Y > maxY)
                {
                    maxY = point.Y;
                }
                if (point.X < minX)
                {
                    minX = point.X;
                }
                if (point.Y < minY)
                {
                    minY = point.Y;
                }
            }

            if (maxX >= width / 2 - this.tagCloudOption.HorizontalOuterMargin
                || maxY >= height / 2 - this.tagCloudOption.VerticalOuterMargin
                || minX <= -width / 2 + this.tagCloudOption.HorizontalOuterMargin
                || minY <= -height / 2 + this.tagCloudOption.VerticalOuterMargin)
            {
                return true;
            }
            return false;
        }

        unsafe private bool HasOverlap(Bitmap bmp, Bitmap newBmp, int width, int height, PointF[] rotatedPoints)
        {
            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxX = int.MinValue;
            int maxY = int.MinValue;
            foreach (PointF point in rotatedPoints)
            {
                if (point.X > maxX)
                {
                    maxX = (int)point.X;
                }
                if (point.Y > maxY)
                {
                    maxY = (int)point.Y;
                }
                if (point.X < minX)
                {
                    minX = (int)point.X;
                }
                if (point.Y < minY)
                {
                    minY = (int)point.Y;
                }
            }
            minX += width / 2;
            maxX += width / 2;
            minY += height / 2;
            maxY += height / 2;

            Rectangle rect = new Rectangle(minX, minY, maxX - minX, maxY - minY);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData newBmpData = newBmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            bool hasOverlap = false;

            try
            {
                byte* bmpPtr = (byte*)bmpData.Scan0.ToPointer();
                byte* newBmpPtr = (byte*)newBmpData.Scan0.ToPointer();
                int stride = bmpData.Stride;

                for (int y = 0; y < rect.Height; ++y)
                {
                    byte* bmpRow = bmpPtr + y * stride;
                    byte* newBmpRow = newBmpPtr + y * stride;

                    for (int x = 0; x < rect.Width; ++x)
                    {
                        byte a1 = bmpRow[4 * x + 3];
                        byte a2 = newBmpRow[4 * x + 3];

                        if (a1 != 0 && a2 != 0)
                        {
                            hasOverlap = true;
                            break;
                        }
                    }

                    if (hasOverlap)
                    {
                        break;
                    }
                }
            }
            finally
            {
                bmp.UnlockBits(bmpData);
                newBmp.UnlockBits(newBmpData);
            }

            return hasOverlap;
        }

        private PointF[] GetRotatedPoints(PointF[] points, float angleInDegrees)
        {
            PointF[] rotatedPoints = new PointF[points.Length];

            double angleInRadians = angleInDegrees * Math.PI / 180;
            for (int i = 0; i < points.Length; ++i)
            {
                PointF point = points[i];
                float rotatedX = (float)(point.X * Math.Cos(angleInRadians) - point.Y * Math.Sin(angleInRadians));
                float rotatedY = (float)(point.X * Math.Sin(angleInRadians) + point.Y * Math.Cos(angleInRadians));
                rotatedPoints[i] = new PointF(rotatedX, rotatedY);
            }

            return rotatedPoints;
        }
    }
}
