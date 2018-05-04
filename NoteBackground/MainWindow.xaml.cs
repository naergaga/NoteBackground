using Microsoft.Win32;
using NoteBackground.Models;
using NoteBackground.Models.Enum;
using NoteBackground.Services;
using NoteBackground.UIs;
using NoteBackground.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NoteBackground
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region DllImport
        private const int SPI_SETDESKWALLPAPER = 20;
        private const int SPIF_UPDATEINIFILE = 1;
        private const int SPIF_SENDCHANGE = 2;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int SystemParametersInfo(
          int uAction, int uParam, string lpvParam, int fuWinIni);
        #endregion

        public MainModel Model { get; set; } = new MainModel();
        private Bitmap _bitmap;
        public string Style1 { get; set; } = "Stretched";
        public string[] StyleArr { get; set; }=new string[]{"Centered", "Stretched", "Tiled" };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnOpenPic_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                Model.ImagePath = dialog.FileName;
                SetImage(Model.ImagePath);
            }
        }

        private void SetImage(Bitmap bitmap)
        {
            if (_bitmap != null)
            {
                _bitmap.Dispose();
            }
            _bitmap = bitmap;
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = memory;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                ImageView.Source = image;
            }
        }

        private void SetImage(string imagePath)
        {
            var bitmap = new Bitmap(imagePath);
            SetImage(bitmap);
        }

        private void BtnSetText_Click(object sender, RoutedEventArgs e)
        {
            var d1 = new SetTextDialog();
            if (d1.ShowDialog() == true)
            {
                UpdateNote(d1.NoteText);
            }
        }

        private void UpdateNote(string text)
        {
            this.Model.NoteText = text;
            ReDraw();
        }

        private PointF GetPoint()
        {
            try
            {
                float x = float.Parse(this.TbPosX.Text);
                float y = float.Parse(this.TbPosY.Text);
                return new PointF(x, y);
            }
            catch
            {
                //MessageBox.Show("x or y 输入错误");
            }
            return new PointF(0, 0);
        }


        /// <summary>
        /// 更新文本时
        /// 点击预览时
        /// </summary>
        private void ReDraw()
        {
            var d = new ImageDraw();
            var bitmap1 = new Bitmap(Model.ImagePath);
            d.Draw(bitmap1, GetPoint(), Model.NoteText);
            SetImage(bitmap1);
        }

        /// <summary>
        /// Sets the chose wallpaper
        /// </summary>
        /// <param name="image">the path to the image used as wallpaper</param>
        /// <param name="style">the wallpaper style (centered, stretched or tiled)</param>
        private void setWallpaper(Bitmap image, BackStyle style)
        {
            try
            {
                // convert the image and save it in <user>\Local Settings\Application Data\SetWallpaper
                string destDir = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "SetWallpaper");

                if (!Directory.Exists(destDir))
                {
                    Directory.CreateDirectory(destDir);
                }

                string destImg = System.IO.Path.Combine(destDir, "wallpaper.bmp");

                System.Drawing.Image src = image;
                src.Save(destImg, ImageFormat.Bmp);

                // save the settings in registry
                //for tiled,    use TileWallpaper=1 WallpaperStyle=0
                //for centered, use TileWallpaper=0 WallpaperStyle=0
                //for strech,   use TileWallpaper=0 WallpaperStyle=2
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", true);

                switch (style)
                {
                    case BackStyle.Centered:
                        key.SetValue("TileWallpaper", "0");
                        key.SetValue("WallpaperStyle", "0");
                        break;

                    case BackStyle.Stretched:
                        key.SetValue("TileWallpaper", "0");
                        key.SetValue("WallpaperStyle", "2");
                        break;

                    case BackStyle.Tiled:
                        key.SetValue("TileWallpaper", "1");
                        key.SetValue("WallpaperStyle", "0");
                        break;
                }

                // finally, set the wallpaper
                SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, destImg, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
            }
            catch (Exception e)
            {
                MessageBox.Show("设置壁纸时，发生错误: " + e.Message);
            }
        }

        private void BtnSetBack_Click(object sender, RoutedEventArgs e)
        {
            if (_bitmap != null)
                setWallpaper(_bitmap, (BackStyle)Enum.Parse(typeof(BackStyle),Style1));
        }

        private void BtnViewBack_Click(object sender, RoutedEventArgs e)
        {
            ReDraw();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine(Style1);
        }
    }
}
