using BackgroundApplicationRelay.PlantyIOT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.System;

namespace BackgroundApplicationRelay
{
    class SleepModule : IPlanty
    {

        TimeSpan sleepUntil;
       
        bool modulePwr = true;
        public SleepModule()
        {
            
           
            
        }

        public IAsyncAction EndTask()
        {
            return Task.Run(dummy).AsAsyncAction();
        }
        public async Task dummy ()
        {
            ///this is a dummy task
        }
      

        public IAsyncAction StartTask()
        {
            return Task.Run(Sleep).AsAsyncAction();
        }

        private async Task Sleep()
        {
            if (modulePwr)
            {
                try
                {
                    var x = CheckDateIsToday();
                    if(!x)
                    {
                        Windows.System.ShutdownManager.BeginShutdown(Windows.System.ShutdownKind.Restart, TimeSpan.FromSeconds(1));
                    }
                    //Windows.System.ShutdownManager.BeginShutdown(Windows.System.ShutdownKind.Restart, TimeSpan.FromSeconds(1));
                }
                catch (Exception e)
                {

                  
                }
            }

        }

        private bool CheckDateIsToday()
        {
           if(DateTime.Today==retrieveSetting())
            {
                return true;

            }
           else
            {
                return false;
            }
        }
        private void saveSetting()
        {
            Windows.Storage.ApplicationDataContainer localSettings =
    Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["restartdate"] = DateTime.Today.ToString();
            

        }
        private DateTime retrieveSetting()
        {
            Windows.Storage.ApplicationDataContainer localSettings =
   Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values["restartdate"]==null)
            {
                return DateTime.Today.Subtract(TimeSpan.FromDays(1));
            }
            else
            {
                DateTime dt = (DateTime)localSettings.Values["restartdate"] ;
                return dt;
            }

        }
    }
    }
