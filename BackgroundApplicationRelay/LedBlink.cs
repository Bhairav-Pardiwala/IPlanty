using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Foundation;

namespace BackgroundApplicationRelay
{
   public sealed class LedBlink
    {

        int gpioPin = 13; //4;
        int gpioShutodownPin = 12; //4;
        GpioController controll;
        GpioPin pin,shutDownPin;
        private bool isOn;
        bool modulePwr = true;
        // FileIO ioF = new FileIO();
        public LedBlink()
        {
            //  Initialize();
            controll = GpioController.GetDefault();
        }

        private void Initialize()
        {
            try
            {
                
                pin = controll.OpenPin(gpioPin);
                pin.SetDriveMode(GpioPinDriveMode.Output);
                shutDownPin = controll.OpenPin(gpioShutodownPin);
                shutDownPin.SetDriveMode(GpioPinDriveMode.InputPullDown);
                shutDownPin.ValueChanged += ShutDownPin_ValueChanged;

            }
            catch (Exception ex)
            {
                //ioF.writeTOFileAs(ex.StackTrace);
            }
        }

        private void ShutDownPin_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            if(shutDownPin.Read()==GpioPinValue.High)
            {
               // Windows.System.ShutdownManager.BeginShutdown(Windows.System.ShutdownKind.Shutdown, TimeSpan.FromSeconds(1));
            }
        }

        private  void Close()
        {
            try
            {
                pin.Dispose();
                
                shutDownPin.Dispose();
            }
            catch (Exception ex)
            {
                //ioF.writeTOFileAs(ex.StackTrace);
            }
        }

        public bool IsOn { get => isOn; }

        public IAsyncAction Blink()
        {
            return Task.Run(BlinkLed).AsAsyncAction();
        }
        private async Task BlinkLed()
        {
            if (modulePwr)
            {
                try
                {
                    Initialize();
                    pin.Write(GpioPinValue.High);

                    await Task.Delay(TimeSpan.FromSeconds(1));
                    pin.Write(GpioPinValue.Low);
                    if (shutDownPin.Read() == GpioPinValue.High)
                    {
                         Windows.System.ShutdownManager.BeginShutdown(Windows.System.ShutdownKind.Shutdown, TimeSpan.FromSeconds(3));
                    }
                    Close();
                }
                catch (Exception ex)
                {


                }
            }
           
        }
       
    }
}
