﻿using System;
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
        Bitmap maskBmp;
        Bitmap resizedMaskBmp;
        TagCloudOption tagCloudOption;

        enum HasOutOfBoundsResult { Inside, Outside, MaskOutside }

        public TagCloud(Dictionary<string, float> tagDic, TagCloudOption tagCloudOption, Dictionary<string, TagOption> tagOptionDic = null)
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

            if (tagCloudOption.FontSizeRange != null)
            {
                float minFontSize = tagCloudOption.FontSizeRange.From;
                float maxFontSize = tagCloudOption.FontSizeRange.To;

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
            Bitmap baseEmptyBitmap = (Bitmap)bmp.Clone();
            if (tagCloudOption.MaskPath != null)
            {
                maskBmp = new Bitmap(tagCloudOption.MaskPath);
                float newHeight;
                float newWidth;
                if (tagCloudOption.StretchMask)
                {
                    newHeight = height;
                    newWidth = width;
                }
                else
                {
                    newHeight = height;
                    newWidth = ((float)maskBmp.Width / maskBmp.Height) * height;
                    if (newWidth > width)
                    {
                        newWidth = width;
                        newHeight = ((float)maskBmp.Height / maskBmp.Width) * width;
                    }
                }
                resizedMaskBmp = new Bitmap(width, height);
                Graphics resizedMaskGraphics = Graphics.FromImage(resizedMaskBmp);
                resizedMaskGraphics.DrawImage(maskBmp, (width - newWidth) / 2, (height - newHeight) / 2, newWidth, newHeight);
            }
            

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
                if (tagOptionDic != null && tagOptionDic.ContainsKey(tag) && tagOptionDic[tag].Rotate != null)
                {
                    rotate = tagOptionDic[tag].Rotate.Value;
                }
                else
                {
                    if (tagCloudOption.RandomRotateRange == null)
                    {
                        rotate = tagCloudOption.RotateList[rnd.Next(0, tagCloudOption.RotateList.Count)];
                    }
                    else
                    {
                        rotate = rnd.Next((int)tagCloudOption.RandomRotateRange.From, (int)tagCloudOption.RandomRotateRange.To);
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
                if (tagOptionDic != null && tagOptionDic.ContainsKey(tag) && tagOptionDic[tag].FontColor != null)
                {
                    fontBrush = new SolidBrush(tagOptionDic[tag].FontColor.Value);
                }
                else
                {
                    fontBrush = new SolidBrush(tagCloudOption.FontColorList[rnd.Next(0, tagCloudOption.FontColorList.Count)]);
                }

                Bitmap bmpForNextTag = null;
                Graphics graphicsForNextTag = null;
                bool sizeChanged = true;
                PointF[] rotatedPoints = null;
                while (true)
                {
                    if (sizeChanged)
                    {
                        bmpForNextTag = (Bitmap)baseEmptyBitmap.Clone();
                        graphicsForNextTag = Graphics.FromImage(bmpForNextTag);

                        sizeChanged = false;
                    }
                    else
                    {
                        ClearBmp(bmpForNextTag, width, height, rotatedPoints);
                        graphicsForNextTag = Graphics.FromImage(bmpForNextTag);
                    }
                    
                    graphicsForNextTag.TranslateTransform(bmpForNextTag.Width / 2, bmpForNextTag.Height / 2);
                    graphicsForNextTag.RotateTransform(rotate);
                    SizeF textSize = graphicsForNextTag.MeasureString(tag, font);
                    (float, float) point = spiral.GetPoint(step);
                    ++step;

                    float beforeRotatedX = point.Item1 - textSize.Width / 2;
                    float beforeRotatedY = point.Item2 - textSize.Height / 2;
                    PointF[] points = { new PointF(beforeRotatedX, beforeRotatedY), new PointF(beforeRotatedX + textSize.Width, beforeRotatedY), new PointF(beforeRotatedX, beforeRotatedY + textSize.Height), new PointF(beforeRotatedX + textSize.Width, beforeRotatedY + textSize.Height) };
                    rotatedPoints = GetRotatedPoints(points, rotate);

                    graphicsForNextTag.DrawString(tag, font, fontBrush, point.Item1 - textSize.Width / 2, point.Item2 - textSize.Height / 2);

                    bool maskMode = tagCloudOption.MaskPath != null;
                    HasOutOfBoundsResult hasOutOfBounds = HasOutOfBounds(rotatedPoints, textSize, rotate, width, height, maskMode);
                    if (hasOutOfBounds == HasOutOfBoundsResult.Outside)
                    {
                        width = width + tagCloudOption.HorizontalCanvasGrowthStep;
                        height = height + tagCloudOption.VerticalCanvasGrowthStep;
                        sizeChanged = true;
                        graphicsForNextTag.Dispose();

                        baseEmptyBitmap = new Bitmap(width, height);
                        Bitmap biggerBitmap = (Bitmap)baseEmptyBitmap.Clone();
                        PasteBmp(biggerBitmap, bmp, (width - bmp.Width) / 2, (height - bmp.Height) / 2, bmp.Width, bmp.Height);
                        graphicsBmp.Dispose();
                        bmp.Dispose();
                        bmp = biggerBitmap;
                        bmpForNextTag.Dispose();

                        if (resizedMaskBmp != null && maskBmp != null)
                        {
                            resizedMaskBmp.Dispose();
                            float newHeight;
                            float newWidth;
                            
                            if (tagCloudOption.StretchMask)
                            {
                                newHeight = height;
                                newWidth = width;
                            }
                            else
                            {
                                newHeight = height;
                                newWidth = ((float)maskBmp.Width / maskBmp.Height) * height;
                                if (newWidth > width)
                                {
                                    newWidth = width;
                                    newHeight = ((float)maskBmp.Height / maskBmp.Width) * width;
                                }
                            }

                            resizedMaskBmp.Dispose();
                            resizedMaskBmp = (Bitmap)baseEmptyBitmap.Clone();

                            Graphics resizedMaskGraphics = Graphics.FromImage(resizedMaskBmp);
                            resizedMaskGraphics.DrawImage(maskBmp, (width - newWidth) / 2, (height - newHeight) / 2, newWidth, newHeight);
                            resizedMaskGraphics.Dispose();
                        }
                        if (resizedMaskBmp != null && maskBmp != null)
                        {
                            step = 0;
                        }
                        else
                        {
                            --step;
                        }
                        graphicsForNextTag.Dispose();
                        continue;
                    }
                    else if (hasOutOfBounds == HasOutOfBoundsResult.MaskOutside)
                    {
                        graphicsForNextTag.Dispose();
                        continue;
                    }

                    if (HasOverlap(bmp, bmpForNextTag, width, height, rotatedPoints))
                    {
                        graphicsForNextTag.Dispose();
                        continue;
                    }

                    if (resizedMaskBmp != null)
                    {
                        if (HasOverlap(resizedMaskBmp, bmpForNextTag, width, height, rotatedPoints, true))
                        {
                            graphicsForNextTag.Dispose();
                            continue;
                        }
                    }

                    graphicsBmp = Graphics.FromImage(bmp);
                    graphicsBmp.TranslateTransform(bmp.Width / 2, bmp.Height / 2);
                    graphicsBmp.RotateTransform(rotate);
                    graphicsBmp.DrawString(tag, font, fontBrush, -textSize.Width / 2 + point.Item1, -textSize.Height / 2 + point.Item2);
                    graphicsBmp.Dispose();
                    graphicsForNextTag.Dispose();
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
                SolidBrush backgroundColorBrush = new SolidBrush((Color)tagCloudOption.BackgroundColor.Value);
                biggerGraphics.FillRectangle(backgroundColorBrush, 0, 0, width, height);
            }
            if (resizedMaskBmp != null && tagCloudOption.ShowMask)
            {
                biggerGraphics.DrawImage(resizedMaskBmp, 0, 0, resizedMaskBmp.Width, resizedMaskBmp.Height);
            }
            biggerGraphics.DrawImage(bmp, 0, 0, width, height);
            bmp = biggerBitmap;
            return bmp;
        }

        private HasOutOfBoundsResult HasOutOfBounds(PointF[] rotatedPoints, SizeF textSize, float rotate, int width, int height, bool maskMode = false)
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

            if (maskMode)
            {
                if (width > height)
                { 
                    height = width;
                }
                else if (height > width)
                {
                    width = height;
                }
            }

            if (maskMode)
            {
                if (maxX >= width / 2 - this.tagCloudOption.HorizontalOuterMargin
                    || maxY >= height / 2 - this.tagCloudOption.VerticalOuterMargin
                    || minX <= -width / 2 + this.tagCloudOption.HorizontalOuterMargin
                    || minY <= -height / 2 + this.tagCloudOption.VerticalOuterMargin)
                {
                    if (minX >= width / 2 - this.tagCloudOption.HorizontalOuterMargin
                    || minY >= height / 2 - this.tagCloudOption.VerticalOuterMargin
                    || maxX <= -width / 2 + this.tagCloudOption.HorizontalOuterMargin
                    || maxY <= -height / 2 + this.tagCloudOption.VerticalOuterMargin)
                        {
                            return HasOutOfBoundsResult.Outside;
                        }
                    return HasOutOfBoundsResult.MaskOutside;
                }
            }
            else
            {
                if (maxX >= width / 2 - this.tagCloudOption.HorizontalOuterMargin
                    || maxY >= height / 2 - this.tagCloudOption.VerticalOuterMargin
                    || minX <= -width / 2 + this.tagCloudOption.HorizontalOuterMargin
                    || minY <= -height / 2 + this.tagCloudOption.VerticalOuterMargin)
                {
                    return HasOutOfBoundsResult.Outside;
                }
            }

            return HasOutOfBoundsResult.Inside;
        }

        unsafe private void PasteBmp(Bitmap bmp, Bitmap copiedBmp, int px, int py, int width, int height)
        {
            Rectangle rect = new Rectangle(px, py, width, height);

            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData copiedBmpData = copiedBmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            try
            {
                byte* bmpPtr = (byte*)bmpData.Scan0.ToPointer();
                byte* copiedBmpPtr = (byte*)copiedBmpData.Scan0.ToPointer();
                int bmpStride = bmpData.Stride;
                int copiedBmpStride = copiedBmpData.Stride;

                byte* bmpRow = null;
                byte* copiedBmpRow = null;

                for (int y = 0; y < rect.Height; ++y)
                {
                    bmpRow = bmpPtr + y * bmpStride;
                    copiedBmpRow = copiedBmpPtr + y * copiedBmpStride;

                    for (int x = 0; x < rect.Width; ++x)
                    {
                        byte alpha = copiedBmpRow[4 * x + 3];
                        if (alpha != 0)
                        {
                            int* bmpPixel = (int*)(bmpRow + 4 * x);
                            int* copiedBmpPixel = (int*)(copiedBmpRow + 4 * x);
                            *bmpPixel = *copiedBmpPixel;
                        }
                    }
                }
            }
            finally
            {
                bmp.UnlockBits(bmpData);
                copiedBmp.UnlockBits(copiedBmpData);
            }
        }

        unsafe private void ClearBmp(Bitmap bmp, int width, int height, PointF[] rotatedPoints)
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

            try
            {
                byte* bmpPtr = (byte*)bmpData.Scan0.ToPointer();
                int stride = bmpData.Stride;

                byte* bmpRow = null;

                for (int y = 0; y < rect.Height; ++y)
                {
                    bmpRow = bmpPtr + y * stride;

                    for (int x = 0; x < rect.Width; ++x)
                    {
                        bmpRow[4 * x + 3] = 0;
                    }
                }
            }
            finally
            {
                bmp.UnlockBits(bmpData);
            }
        }

        unsafe private bool HasOverlap(Bitmap bmp, Bitmap newBmp, int width, int height, PointF[] rotatedPoints, bool isCheckMask = false)
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
            if (!isCheckMask)
            {
                minX += width / 2 - tagCloudOption.TagSpacing;
                maxX += width / 2 + tagCloudOption.TagSpacing;
                minY += height / 2 - tagCloudOption.TagSpacing;
                maxY += height / 2 + tagCloudOption.TagSpacing;

                minX = minX < 0 ? 0 : minX;
                maxX = maxX >= width ? width : maxX;
                minY = minY < 0 ? 0 : minY;
                maxY = maxY >= height ? height : maxY;
            }
            else
            {
                minX += width / 2;
                maxX += width / 2;
                minY += height / 2;
                maxY += height / 2;
            }

            Rectangle rect = new Rectangle(minX, minY, maxX - minX, maxY - minY);

            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData newBmpData = newBmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            bool hasOverlap = false;

            try
            {
                byte* bmpPtr = (byte*)bmpData.Scan0.ToPointer();
                byte* newBmpPtr = (byte*)newBmpData.Scan0.ToPointer();
                int stride = bmpData.Stride;

                byte* bmpRow = null;
                byte* newBmpRow = null;

                for (int y = 0; y < rect.Height; ++y)
                {
                    bmpRow = bmpPtr + y * stride;
                    newBmpRow = newBmpPtr + y * stride;

                    for (int x = 0; x < rect.Width; ++x)
                    {
                        byte a1 = bmpRow[4 * x + 3];
                        byte a2 = newBmpRow[4 * x + 3];
                        if (a2 != 0)
                        {
                            if (CheckAlphaValues(bmpPtr, rect.Width, rect.Height, x, y, tagCloudOption.TagSpacing, stride, isCheckMask))
                            {
                                hasOverlap = true;
                                break;
                            }
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

        private unsafe bool CheckAlphaValues(byte* bmpPtr, int width, int height, int x, int y, int n, int stride, bool isCheckMask)
        {
            if (isCheckMask)
            {
                n = 0;
            }
            int k = n;
            int step = tagCloudOption.Accuracy + 1;
            for (int j = -n; j <= n; j += step)
            {
                int absK = Math.Abs(k);
                if (n - j < step)
                {
                    j = n;
                }
                for (int i = -n + absK; i <= n - absK; i += step)
                {
                    if (n - i < step)
                    {
                        i = n;
                    }

                    int nx = x + i;
                    int ny = y + j;

                    if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                    {
                        int pixelPos = (ny * stride) + nx * 4;
                        byte alpha = bmpPtr[pixelPos + 3];
                        if (isCheckMask)
                        {
                            if (alpha == 0)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            if (alpha != 0)
                            {
                                return true;
                            }
                        }
                    }
                }
                --k;
            }

            return false;
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
