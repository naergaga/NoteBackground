using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace NoteBackground.Services
{
    public class ImageDraw
    {
        //private const double WIDTH_CHAR = 37.64705882352941;
        //private const double HEIGHT_CHAR = 80;
        //private const int SIZE_CHAR = 24;

        public static Font font = new Font(FontFamily.GenericSansSerif, 24);

        public void Draw(Bitmap bitmap,RectangleF rectangle,string text)
        {
            var w = bitmap.Width;
            var h = bitmap.Height;

            var g = Graphics.FromImage(bitmap);
            g.DrawString(text, font, Brushes.Red, rectangle);
           
            g.Flush();
        }
    }
}
