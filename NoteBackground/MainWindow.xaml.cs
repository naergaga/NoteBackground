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
        public MainWindowDataContext DC => (MainWindowDataContext)DataContext;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnOpenPic_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                DC.SetImage(dialog.FileName);
            }
        }

        private void BtnSetText_Click(object sender, RoutedEventArgs e)
        {
            var d1 = new SetTextDialog();
            d1.Owner = this;
            if (d1.ShowDialog() == true)
            {
                DC.UpdateNote(d1.NoteText);
            }
        }

        private void BtnSetBack_Click(object sender, RoutedEventArgs e)
        {
            if (DC.Bitmap != null)
                DC.SetWallpaper();
        }

        private void BtnViewBack_Click(object sender, RoutedEventArgs e)
        {
            DC.ReDraw();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
