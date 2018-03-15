using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Forms = System.Windows.Forms;

namespace BatteryPercentage
{
    static class Battery
    {
        public static int Percent => (int)Math.Round(Forms.SystemInformation.PowerStatus.BatteryLifePercent * 100);

        public static bool IsOnline => Forms.SystemInformation.PowerStatus.PowerLineStatus == Forms.PowerLineStatus.Online;

        public static int RemainingTime => IsOnline? 0 : Forms.SystemInformation.PowerStatus.BatteryLifeRemaining;

        public static string GetStatus()
        {
            int percent = Percent;
            if (IsOnline)
            {
                if(percent < 100)
                {
                    return percent + "%, charging";
                }
                else
                {
                    return "Fully charged";
                }
            }
            else
            {
                int mins = RemainingTime / 60;
                int hours = mins / 60;
                mins = mins % 60;
                return percent + "%, " + hours + " hours " + mins + " minutes remaining";
            }
        }
    }
}
