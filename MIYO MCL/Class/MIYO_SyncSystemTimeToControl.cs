using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MIYO_MCL.Class
{
    internal class MIYO_SyncSystemTimeToControl<T>
    {

        T Target;

        public MIYO_SyncSystemTimeToControl(T TargetControl) 
        {
            Target = TargetControl;

        }

        public DispatcherTimer Start()
        {
            DispatcherTimer SyncSystemTimeTimer = new DispatcherTimer() 
            {
                Interval = TimeSpan.FromSeconds(1),
            };
            SyncSystemTimeTimer.Tick += (s, e) => 
            {
                if(Target != null) 
                {
                    if(Target is TextBlock objTb)
                    {
                        objTb.Text = DateTime.Now.ToString("g");
                    }
                    else if(Target is Label objlb)
                    {
                       objlb.Content = DateTime.Now.ToString("g");
                    }
                    else 
                    {
                    
                    }
                
                }
            };
            SyncSystemTimeTimer.Start();

            return SyncSystemTimeTimer;
        }

    }
}
