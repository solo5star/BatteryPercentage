using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.Remoting;
using System.Threading;

namespace BatteryPercentage
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            new BatteryIcon();
        }
    }
}
