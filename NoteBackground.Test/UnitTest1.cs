using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoteBackground.Services;

namespace NoteBackground.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var d1 = new ImageDraw();
            using (var image1 = new Bitmap("./test.jpg"))
            {
                //d1.Draw(image1);
                //image1.Save("test1.jpg");
            }
        }
    }
}
