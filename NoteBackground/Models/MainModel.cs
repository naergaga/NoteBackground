using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteBackground.Models
{
    public class MainModel
    {
        public string ImagePath { get; set; }
        public string NoteText { get; set; }
        public PointF Point { get; set; } = new PointF(0, 0);
    }
}
