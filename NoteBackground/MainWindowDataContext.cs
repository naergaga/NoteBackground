using Microsoft.Win32;
using NoteBackground.Models.Enum;
using NoteBackground.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace NoteBackground
{
    public class MainWindowDataContext : ObservableObject
    {
        #region DllImport
        private const int SPI_SETDESKWALLPAPER = 20;
        private const int SPIF_UPDATEINIFILE = 1;
        private const int SPIF_SENDCHANGE = 2;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int SystemParametersInfo(
          int uAction, int uParam, string lpvParam, int fuWinIni);
        #endregion

        public string ImagePath { get; set; }
        public string NoteText { get; set; }
        public PointF Point { get; set; } = new PointF(0, 0);
        public SizeF Size { get; set; } = new SizeF(400, 200);
        public Bitmap Bitmap { get; set; }
        public BackStyle Style { get; set; } = BackStyle.Stretched;
        public BackStyle[] Styles { get; set; } = new BackStyle[] { BackStyle.Stretched, BackStyle.Centered, BackStyle.Tiled };

        public MainWindow Main => (MainWindow)App.Current.MainWindow;

        public void SetImage(Bitmap bitmap)
        {
            if (Bitmap != null)
            {
                Bitmap.Dispose();
            }
            Bitmap = bitmap;
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = memory;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                Main.ImageView.Source = image;
            }
        }

        public void SetImage(string imagePath)
        {
            ImagePath = imagePath;
            var bitmap = new Bitmap(imagePath);
            SetImage(bitmap);
        }

        public void UpdateNote(string text)
        {
            NoteText = text;
            ReDraw();
        }

        /// <summary>
        /// 更新文本时
        /// 点击预览时
        /// </summary>
        public void ReDraw()
        {
            var d = new ImageDraw();
            var bitmap1 = new Bitmap(ImagePath);
            d.Draw(bitmap1, new RectangleF(Point, Size), NoteText);
            SetImage(bitmap1);
        }

        /// <summary>
        /// Sets the chose wallpaper
        /// </summary>
        /// <param name="Bitmap">the path to the image used as wallpaper</param>
        /// <param name="style">the wallpaper style (centered, stretched or tiled)</param>
        public void SetWallpaper()
        {
            try
            {
                string destDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "SetWallpaper");

                if (!Directory.Exists(destDir))
                {
                    Directory.CreateDirectory(destDir);
                }

                string destImg = System.IO.Path.Combine(destDir, "wallpaper.bmp");

                System.Drawing.Image src = Bitmap;
                src.Save(destImg, ImageFormat.Bmp);

                RegistryKey key = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", true);

                switch (Style)
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
    }
}
