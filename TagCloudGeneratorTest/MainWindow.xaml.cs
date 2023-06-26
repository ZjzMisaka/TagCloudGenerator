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

            Dictionary<string, float> tagDic = new Dictionary<string, float>();
            tagDic["J.C.STAFF"] = 32;
            tagDic["A-1 Pictures"] = 23;
            tagDic["动画工房"] = 22;
            tagDic["SILVER LINK."] = 20;
            tagDic["京都动画"] = 16;
            tagDic["SHAFT"] = 12;
            tagDic["CloverWorks"] = 11;
            tagDic["project No.9"] = 10;
            tagDic["WHITE FOX"] = 9;
            tagDic["8bit"] = 9;
            tagDic["Studio五组"] = 9;
            tagDic["feel."] = 9;
            tagDic["ufotable"] = 8;
            tagDic["STUDIO DEEN"] = 8;
            tagDic["Lerche"] = 7;
            tagDic["日昇动画"] = 7;
            tagDic["BONES"] = 7;
            tagDic["LIDENFILMS"] = 6;
            tagDic["XEBEC"] = 6;
            tagDic["C2C"] = 5;
            tagDic["EMT Squared"] = 5;
            tagDic["P.A.WORKS"] = 5;
            tagDic["SATELIGHT"] = 5;
            tagDic["TRIGGER"] = 5;
            tagDic["OLM"] = 4;
            tagDic["Kinema Citrus"] = 4;
            tagDic["Seven"] = 4;
            tagDic["ENGI"] = 3;
            tagDic["CONNECT"] = 3;
            tagDic["三次元"] = 3;
            tagDic["Passione"] = 3;
            tagDic["PINE JAM"] = 3;
            tagDic["Diomedéa"] = 3;
            tagDic["SHIN-EI动画"] = 3;
            tagDic["Production IMS"] = 3;
            tagDic["MADHOUSE"] = 3;
            tagDic["GONZO"] = 3;
            tagDic["TMS Entertainment"] = 3;
            tagDic["SUNRISE"] = 3;
            tagDic["david production"] = 3;
            tagDic["AIC Build"] = 3;
            tagDic["WIT STUDIO、CloverWorks"] = 2;
            tagDic["LIDENFILMS京都工作室"] = 2;
            tagDic["Studio Bind"] = 2;
            tagDic["Bibury Animation Studios"] = 2;
            tagDic["studio A-CAT"] = 2;
            tagDic["ENCOURAGE FILMS"] = 2;
            tagDic["手冢制作公司"] = 2;
            tagDic["TMS娱乐"] = 2;
            tagDic["DMM.futureworks、W-Toon Studio"] = 2;
            tagDic["Studio 3Hz"] = 2;
            tagDic["手冢Production"] = 2;
            tagDic["C-Station"] = 2;
            tagDic["Studio五组、AXsiZ"] = 2;
            tagDic["Creators in Pack"] = 2;
            tagDic["旭Production"] = 2;
            tagDic["Polygon Pictures"] = 2;
            tagDic["vap"] = 2;
            tagDic["KINEMA CITRUS"] = 2;
            tagDic["Brain's Base"] = 2;
            tagDic["manglobe"] = 2;
            tagDic["AIC Classic"] = 2;
            tagDic["AIC"] = 2;
            tagDic["GoHands"] = 2;
            tagDic["P.A.Works"] = 2;
            tagDic["Actas"] = 2;
            tagDic["Studio Flad"] = 1;
            tagDic["MAPPA"] = 1;
            tagDic["feel.、GAINA"] = 1;
            tagDic["新锐动画、SynergySP"] = 1;
            tagDic["OLM Team Kojima"] = 1;
            tagDic["GEEKTOYS"] = 1;
            tagDic["KINEMA CITRUS、DRMOVIE"] = 1;
            tagDic["FelixFilm"] = 1;
            tagDic["OLM Team Yoshioka"] = 1;
            tagDic["梦太公司、Graphinica"] = 1;
            tagDic["新锐动画"] = 1;
            tagDic["晓"] = 1;
            tagDic["W-Toon Studio"] = 1;
            tagDic["Synergy SP"] = 1;
            tagDic["Synergy SP、VEGA entertainment、Studio A-CAT"] = 1;
            tagDic["Telecom Animation Film"] = 1;
            tagDic["REVOROOT"] = 1;
            tagDic["SIGNAL.MD"] = 1;
            tagDic["TNK"] = 1;
            tagDic["Acca effe、Giga Production"] = 1;
            tagDic["Hoods Entertainment"] = 1;
            tagDic["STUDIO KAI"] = 1;
            tagDic["Seven Arcs"] = 1;
            tagDic["DMM.Futureworks/W-Toon Studio、三次元"] = 1;
            tagDic["横滨动画研究所"] = 1;
            tagDic["Nomad"] = 1;
            tagDic["Children's Playground Entertainment"] = 1;
            tagDic["YAOYOROZU"] = 1;
            tagDic["Lesprit"] = 1;
            tagDic["龙之子"] = 1;
            tagDic["Pierrot PLUS"] = 1;
            tagDic["TRIGGER、A-1 Pictures→CloverWorks"] = 1;
            tagDic["Geno Studio"] = 1;
            tagDic["Millepensee"] = 1;
            tagDic["WAO WORLD"] = 1;
            tagDic["Studio 3Hz、Actas"] = 1;
            tagDic["diomedéa"] = 1;
            tagDic["SATELIGHT×C2C"] = 1;
            tagDic["Bridge"] = 1;
            tagDic["ISSEN×XEBEC"] = 1;
            tagDic["yaoyorozu"] = 1;
            tagDic["MAPPA、VOLN"] = 1;
            tagDic["NUT"] = 1;
            tagDic["Lay-duce"] = 1;
            tagDic["WAO! World"] = 1;
            tagDic["EMT²"] = 1;
            tagDic["AXsiZ"] = 1;
            tagDic["WIT STUDIO"] = 1;
            tagDic["Seven Arcs Pictures"] = 1;
            tagDic["龙之子Production"] = 1;
            tagDic["Animation Studio Artland"] = 1;
            tagDic["手冢Productions"] = 1;
            tagDic["Nexus"] = 1;
            tagDic["A-Real"] = 1;
            tagDic["Studio Pierrot+"] = 1;
            tagDic["AIC PLUS+"] = 1;
            tagDic["Actas Inc."] = 1;
            tagDic["Aslead"] = 1;
            tagDic["project No.9×Studio Blanc"] = 1;
            tagDic["DLE"] = 1;
            tagDic["GAINAX"] = 1;
            tagDic["Brain's･Base"] = 1;
            tagDic["asread"] = 1;
            tagDic["童梦"] = 1;

            DateTime dt = DateTime.Now;
            TagCloudOption tagCloudOption = new TagCloudOption();
            tagCloudOption.IsRandomInitAngle = true;
            tagCloudOption.RotateList = new List<int> { 0, 90 };
            tagCloudOption.FontColorList = new List<Color> { Color.FromArgb(66, 141, 194), Color.FromArgb(42, 131, 194), Color.FromArgb(10, 100, 164) };
            PrivateFontCollection collection = new PrivateFontCollection();
            //collection.AddFontFile(@"H:\AnimeReport\Fonts\Lolita.ttf");
            //FontFamily fontFamily = new FontFamily("Lolita", collection);
            //tagCloudOption.FontFamily = fontFamily;
            tagCloudOption.TagSpacing = 3;
            tagCloudOption.FontSizeRange = (16, 180);
            tagCloudOption.BackgroundColor = new TagCloudOption.ColorOption(Color.AliceBlue);
            tagCloudOption.HorizontalCanvasGrowthStep = 5;
            tagCloudOption.VerticalCanvasGrowthStep = 5;
            //tagCloudOption.OutputSize = new ImgSize(1920, 1080);
            tagCloudOption.HorizontalOuterMargin = 0;
            tagCloudOption.VerticalOuterMargin = 0;
            //tagCloudOption.MaskPath = @"D:\cloud.png";
            tagCloudOption.ShowMask = true;
            tagCloudOption.StretchMask = true;

            Dictionary<string, TagOption> tagOptionDic = new Dictionary<string, TagOption>();
            tagOptionDic["J.C.STAFF"] = new TagOption() { Rotate = new Rotate(0) };

            Bitmap bmp = new TagCloud(tagDic, tagCloudOption, tagOptionDic).Get();
            
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
