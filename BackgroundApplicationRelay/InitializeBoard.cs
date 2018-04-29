using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Foundation;

namespace BackgroundApplicationRelay
{
    class InitializeBoard
    {
       static int gpioPin = 25; //4;
       static GpioController controll;
       static GpioPin pin;
      static  private bool isOn;
    
        static bool modulePwr = true;
        
        public static IAsyncAction Initialize()
        {
            if(modulePwr && !isOn)
            {
                try
                {

                    controll = GpioController.GetDefault();
                    pin = controll.OpenPin(gpioPin);
                    pin.SetDriveMode(GpioPinDriveMode.Output);
                    pin.Write(GpioPinValue.High);
                    isOn = true;
                    //Give Lm7805 1 second to stabilize 
                    return Task.Delay(TimeSpan.FromSeconds(1.5)).AsAsyncAction();
                }
                catch (Exception ex)
                {
                    //ioF.writeTOFileAs(ex.StackTrace);
                }
            }
            return Task.CompletedTask.AsAsyncAction();
          
        }
        public static void Close()
        {
            try
            {
                pin.Write(GpioPinValue.Low);
                pin.Dispose();
                isOn = false;

            }
            catch (Exception ex)
            {
                //ioF.writeTOFileAs(ex.StackTrace);
            }
        }

        public bool IsOn { get => isOn; }
    }
}
