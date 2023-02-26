using System;
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

            Dictionary<string, int> tagDic = new Dictionary<string, int>();
            tagDic["yuri"] = 80;
            tagDic["CGDCT"] = 80;
            tagDic["NO"] = 70;
            tagDic["Hello"] = 60;
            tagDic["World"] = 50;
            tagDic["Apple"] = 40;
            tagDic["1024"] = 40;
            tagDic["1"] = 20;
            tagDic["or"] = 18;
            tagDic["WTF"] = 16;
            tagDic["2048"] = 16;
            tagDic["and"] = 12;
            tagDic["can't"] = 8;
            tagDic["ok"] = 7;
            
            DateTime dt = DateTime.Now;
            TagCloudOption tagCloudOption = new TagCloudOption();
            tagCloudOption.RotateList = new List<int> { 15, -15 };
            tagCloudOption.FontColorList = new List<Color> { Color.Red, Color.Orange, Color.OrangeRed };
            PrivateFontCollection collection = new PrivateFontCollection();
            collection.AddFontFile(@"H:\AnimeReport\Fonts\Lolita.ttf");
            FontFamily fontFamily = new FontFamily("Lolita", collection);
            tagCloudOption.FontFamily = fontFamily;
            Bitmap bmp = new TagCloud(1000, 1000, tagDic, tagCloudOption).Get();
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
