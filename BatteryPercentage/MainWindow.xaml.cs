using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;
using Forms = System.Windows.Forms;

namespace BatteryPercentage
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            new BatteryIcon(this);

            InitializeBatteryHistory();


            var timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += delegate (object sender, EventArgs args)
            {
                UpdateBatteryPercent();

                UpdateBatteryHistory();
            };
            UpdateBatteryPercent();
            timer.Start();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
            }
            base.OnStateChanged(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        public void SwitchTab(string tab)
        {
            switch (tab)
            {
                case "Main":
                    Tab_Main.IsSelected = true;
                    break;
                case "History":
                    Tab_History.IsSelected = true;
                    break;
                case "Info":
                    Tab_Info.IsSelected = true;
                    break;
                default:
                    throw new ArgumentException("Tab " + tab + " is not exist");
            }
        }

        public void UpdateBatteryPercent()
        {
            int percent = Battery.GetPercent();

            Text_BatteryPercent.Text = percent + "%";
            Rec_BatteryUse.Height = (100 - percent) * 1.2;

            if(percent <= 15)
            {
                Ellipse_BatteryColor.Fill = Brushes.Red;
            }
            else if(percent <= 20)
            {
                Ellipse_BatteryColor.Fill = Brushes.Orange;
            }
            else
            {
                Ellipse_BatteryColor.Fill = Brushes.LightGreen;
            }
        }

        public void InitializeBatteryHistory()
        {
            const double rows = 4;
            const double columns = 8;

            for(int i = 0; i <= columns; i++)
            {
                double x = i * (400 / columns);
                Canvas_History.Children.Add(new Line() {
                    X1 = x, Y1 = 0, X2 = x, Y2 = 180,
                    Stroke = Brushes.Black
                });
            }
            
            for(int i = 0; i <= rows; i++)
            {
                double y = i * (180 / rows);
                Canvas_History.Children.Add(new Line() {
                    X1 = 0, Y1 = y, X2 = 400, Y2 = y,
                    Stroke = Brushes.Black
                });
            }
        }

        private Point latestPoint;
        private int histPercent = 98;
        public void UpdateBatteryHistory()
        {
            int percent = histPercent--;
            const double columns = 8;

            PointCollection points = new PointCollection();
            if(latestPoint != null)
            {
                points.Add(latestPoint);
                points.Add(latestPoint = new Point(latestPoint.X + (400 / columns) / 60, (180 / 100) * percent));
            }
            else
            {
                latestPoint = new Point(0, (180 / 100) * percent);
                return;
            }

            Polyline polyline = new Polyline();
            polyline.StrokeThickness = 2;
            polyline.Stroke = Brushes.Green;
            polyline.Points = points;

            Canvas_History.Children.Add(polyline);
        }
    }
}
