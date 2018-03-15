using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Forms = System.Windows.Forms;

namespace BatteryPercentage
{
    class BatteryIcon
    {
        public Font TrayIconFont { get; } = new Font("Microsoft Sans Serif", 16, System.Drawing.FontStyle.Regular, GraphicsUnit.Pixel); // font name, font size

        public Forms.ContextMenu Menu { get; private set; }
        public Forms.NotifyIcon TrayIcon { get; private set; }

        public BatteryIcon()
        {
            TrayIcon = new Forms.NotifyIcon()
            {
                Text = Battery.GetStatus(),
                Icon = DrawIcon(),
                Visible = true,
                ContextMenu = Menu = new Forms.ContextMenu()
                {
                    MenuItems = {
                        new Forms.MenuItem("Exit", delegate(object click, EventArgs eClick){
                            Application.Current.Shutdown();
                        })
                    }
                }
            };

            var timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += delegate (object sender, EventArgs args)
            {
                var status = Battery.GetStatus();
                if(TrayIcon.Text != status)
                {
                    TrayIcon.Icon = DrawIcon();
                    TrayIcon.Text = status;
                }
            };
            timer.Start();
        }

        public Icon DrawIcon()
        {
            Bitmap bmp = new Bitmap(16, 16);
            Graphics g = Graphics.FromImage(bmp);

            int percent = Battery.Percent;
            Brush brush = Brushes.White;
            int x = -4, y = -2;

            g.Clear(Color.Transparent);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            
            // Select brush
            if (Battery.IsOnline)
            {
                brush = Brushes.LightGreen;
            }
            else if (percent <= 15)
            {
                brush = Brushes.Red;
            }
            else if(percent <= 20)
            {
                brush = Brushes.Orange;
            }

            // Drawing
            if(percent >= 100)
            {
                g.DrawString("1", TrayIconFont, brush, x - 2, y);
                g.DrawString("0", TrayIconFont, brush, x + 3, y);
                g.DrawString("0", TrayIconFont, brush, x + 9, y);
            }
            else if (percent < 10)
            {
                g.DrawString(percent.ToString(), TrayIconFont, brush, x + 4, y);
            }
            else
            {
                g.DrawString(percent.ToString(), TrayIconFont, brush, x, y);
            }

            return Icon.FromHandle(bmp.GetHicon());
        }
    }
}
