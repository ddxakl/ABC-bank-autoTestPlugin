using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ABCAgreeAutoTestPluging4.CommonProperty;
using CefSharp.WinForms;

namespace ABCAgreeAutoTestPluging4.Untils
{
    class ScreenShot
    {
        public static void Shot(ChromiumWebBrowser chromiumWebBrowser) {

            Control control = chromiumWebBrowser as Control;

            Bitmap bit = new Bitmap(control.Width, control.Height);//截图本窗体大小针对具体情况进行调整

            Graphics g = Graphics.FromImage(bit);

            g.CopyFromScreen(control.PointToScreen(Point.Empty), Point.Empty, bit.Size);

            string currentTime = DateTime.Now.ToString("yyyyMMddhhmmss");

            string imageFileInfo = Property.LocalPath + @"\" + Property.currentTimePath + @"\image\" + currentTime + ".png";

            string imagePath = Property.LocalPath + @"\" + Property.currentTimePath + @"\image\";

            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);  //创建目录
            }
            
            bit.Save(imageFileInfo, ImageFormat.Png);//保存图片
        }
    }
}
