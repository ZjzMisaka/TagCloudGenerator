﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TagCloudGenerator;
using Color = System.Drawing.Color;
using FontFamily = System.Drawing.FontFamily;

namespace TagCloudGeneratorTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Dictionary<string, float> tagDic = new Dictionary<string, float>();
            tagDic["yuri"] = 80;
            tagDic["CGDCT"] = 80;
            tagDic["NO"] = 70;
            tagDic["InitializeComponent"] = 70;
            tagDic["oyama mahiro"] = 65;
            tagDic["Hello"] = 60;
            tagDic["World"] = 50;
            tagDic["Apple"] = 40;
            tagDic["1024"] = 40;
            tagDic["1"] = 20;
            tagDic["or"] = 18;
            tagDic["MainWindow"] = 16;
            tagDic["Window"] = 16;
            tagDic["PrivateFontCollection"] = 16;
            tagDic["WTF"] = 16;
            tagDic["2048"] = 16;
            tagDic["memory"] = 12;
            tagDic["bitmapimage"] = 12;
            tagDic["tagCloudOption"] = 12;
            tagDic["BitmapImage bitmapimage"] = 12;
            tagDic["可视化QT应用软件打包发布程序"] = 12;
            tagDic["FontFamily"] = 12;
            tagDic["Following"] = 12;
            tagDic["For you Beta"] = 12;
            tagDic["static-binaries"] = 12;
            tagDic["and"] = 12;
            tagDic["RTX2080"] = 10;
            tagDic["RTX3080"] = 10;
            tagDic["RTX4080"] = 10;
            tagDic["TagCloudOption tagCloudOption"] = 10;
            tagDic["Title"] = 10;
            tagDic["TotalSeconds"] = 10;
            tagDic["ToString"] = 10;
            tagDic["can't"] = 8;
            tagDic["okokoookokokok"] = 7;
            tagDic["nonononnononnonononononon"] = 2;

            DateTime dt = DateTime.Now;
            TagCloudOption tagCloudOption = new TagCloudOption();
            tagCloudOption.IsRandomInitAngle = true;
            tagCloudOption.RotateList = new List<int> { -10, 10 };
            tagCloudOption.FontColorList = new List<Color> { Color.FromArgb(66, 141, 194), Color.FromArgb(42, 131, 194), Color.FromArgb(10, 100, 164) };
            PrivateFontCollection collection = new PrivateFontCollection();
            //collection.AddFontFile(@"H:\AnimeReport\Fonts\Lolita.ttf");
            //FontFamily fontFamily = new FontFamily("Lolita", collection);
            //tagCloudOption.FontFamily = fontFamily;
            tagCloudOption.Margin = 3;
            tagCloudOption.FontSizeRange = (16, 90);
            Bitmap bmp = new TagCloud(1200, 1200, tagDic, tagCloudOption).Get();
            using (MemoryStream memory = new MemoryStream())
            {
                bmp.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                img.Source =  bitmapimage;
            }

            this.Title = (DateTime.Now - dt).TotalSeconds.ToString();
        }
    }
}
