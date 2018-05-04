using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace NoteBackground.UIs
{
    /// <summary>
    /// SetTextDialog.xaml 的交互逻辑
    /// </summary>
    public partial class SetTextDialog : Window
    {
        public string NoteText { get; set; }

        public SetTextDialog()
        {
            InitializeComponent();
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            NoteText = this.TbText.Text;
            this.DialogResult = true;
        }
    }
}
