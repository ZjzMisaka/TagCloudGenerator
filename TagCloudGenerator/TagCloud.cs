using System;
using System.Collections.Generic;
using System.Drawing;
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

        public TagCloud(int width, int height, Dictionary<string, float> tagDic, TagCloudOption tagCloudOption)
        {
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
                float max = tagDic.Values.ElementAt(0);
                foreach (int count in tagDic.Values)
                {
                    if (count < min)
                    {
                        min = count;
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
            SolidBrush brush = new SolidBrush(Color.AliceBlue);
            (int, int, int) backgroundRgb = (brush.Color.R, brush.Color.G, brush.Color.B);
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

                int outOfBoundsFailedCount = 0;

                while (true)
                {
                    Bitmap newBmp = new Bitmap(width, height);
                    Graphics graphics = Graphics.FromImage(newBmp);
                    graphics.FillRectangle(brush, 0, 0, width, height);
                    graphics.TranslateTransform(newBmp.Width / 2, newBmp.Height / 2);
                    graphics.RotateTransform(rotate);
                    SizeF textSize = graphics.MeasureString(tag, font);
                    (float, float) point = spiral.GetPoint(step);
                    ++step;
                    graphics.DrawString(tag, font, fontBrush, point.Item1 - textSize.Width / 2, point.Item2 - textSize.Height / 2);

                    for (int i = 1; i <= tagCloudOption.Margin; ++i)
                    {
                        graphics.DrawString(tag, font, fontBrush, point.Item1 - textSize.Width / 2 - i, point.Item2 - textSize.Height / 2);
                        graphics.DrawString(tag, font, fontBrush, point.Item1 - textSize.Width / 2 + i, point.Item2 - textSize.Height / 2);
                        graphics.DrawString(tag, font, fontBrush, point.Item1 - textSize.Width / 2, point.Item2 - textSize.Height / 2 - i);
                        graphics.DrawString(tag, font, fontBrush, point.Item1 - textSize.Width / 2, point.Item2 - textSize.Height / 2 + i);
                    }

                    graphics.Dispose();

                    if (HasOutOfBounds(point, textSize, width, height))
                    {
                        ++outOfBoundsFailedCount;
                        if (outOfBoundsFailedCount == 10000)
                        {
                            throw new Exception("Image size is too small");
                        }
                        continue;
                    }

                    if (HasOverlap(bmp, newBmp, width, height, backgroundRgb))
                    {
                        continue;
                    }

                    graphicsBmp = Graphics.FromImage(bmp);
                    graphicsBmp.TranslateTransform(newBmp.Width / 2, newBmp.Height / 2);
                    graphicsBmp.RotateTransform(rotate);
                    graphicsBmp.DrawString(tag, font, fontBrush, -textSize.Width / 2 + point.Item1, -textSize.Height / 2 + point.Item2);
                    graphicsBmp.Dispose();
                    break;
                }
            }
        }

        public Bitmap Get()
        {
            return bmp;
        }

        private bool HasOutOfBounds((float, float) point, SizeF textSize, int width, int height)
        {
            if (point.Item1 >= 0)
            {
                if (point.Item1 + textSize.Width / 2 > width / 2)
                {
                    return true;
                }
            }
            else if (point.Item1 < 0)
            {
                if (point.Item1 - textSize.Width / 2 < -width / 2)
                {
                    return true;
                }
            }
            if (point.Item2 >= 0)
            {
                if (point.Item2 + textSize.Height / 2 > height / 2)
                {
                    return true;
                }
            }
            else if (point.Item2 < 0)
            {
                if (point.Item2 - textSize.Height / 2 < -height / 2)
                {
                    return true;
                }
            }

            return false;
        }

        unsafe private bool HasOverlap(Bitmap origBmp, Bitmap newBmp, int width, int height, (int, int, int) backgroundRgb)
        {
            byte[] data1 = GetByteArray(origBmp);
            byte[] data2 = GetByteArray(newBmp);

            // 判断两个字节数组中红色像素部分是否重叠
            bool hasOverlap = false;
            fixed (byte* ptr1 = data1, ptr2 = data2)
            {
                // 按行遍历像素
                for (int y = 0; y < height; y++)
                {
                    // 获取指向当前行的指针
                    byte* row1 = ptr1 + y * width * 3;
                    byte* row2 = ptr2 + y * width * 3;

                    // 按像素遍历
                    for (int x = 0; x < width; x++)
                    {
                        // 获取当前像素的RGB值
                        byte r1 = row1[x * 3 + 2];
                        byte g1 = row1[x * 3 + 1];
                        byte b1 = row1[x * 3];
                        byte r2 = row2[x * 3 + 2];
                        byte g2 = row2[x * 3 + 1];
                        byte b2 = row2[x * 3];

                        // 判断当前像素是否为底色
                        if ((r1 != backgroundRgb.Item1 || g1 != backgroundRgb.Item2 || b1 != backgroundRgb.Item3)
                            && (r2 != backgroundRgb.Item1 || g2 != backgroundRgb.Item2 || b2 != backgroundRgb.Item3))
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

            return hasOverlap;
        }

        // 将Bitmap转换为字节数组的函数
        private byte[] GetByteArray(Bitmap bmp)
        {
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            IntPtr ptr = bmpData.Scan0;
            int size = bmpData.Stride * bmp.Height;
            byte[] data = new byte[size];
            Marshal.Copy(ptr, data, 0, size);
            bmp.UnlockBits(bmpData);
            return data;
        }
    }
}
