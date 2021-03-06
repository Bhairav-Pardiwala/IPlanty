﻿using BackgroundApplicationRelay.PlantyIOT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Foundation;

namespace BackgroundApplicationRelay
{
    public sealed class PumpIO : IPlanty
    {


        int gpioPin = 26; //4;
        GpioController controll;
        GpioPin pin;
        private bool isOn;
        FileIO ioF = new FileIO();
        bool modulePwr = true;
        public PumpIO()
        {
            try
            {
                controll = GpioController.GetDefault();
                //Initialize();
            }
            catch (Exception ex)
            {
                ioF.writeTOFileAs(ex.StackTrace);
            }
        }
        private void Initialize()
        {
            try
            {

                pin = controll.OpenPin(gpioPin);
                pin.SetDriveMode(GpioPinDriveMode.Output);
                pin.Write(GpioPinValue.Low);
                
            }
            catch (Exception ex)
            {
                //ioF.writeTOFileAs(ex.StackTrace);
            }
        }
        private void Close()
        {
            try
            {
               
                pin.Dispose();

            }
            catch (Exception ex)
            {
                //ioF.writeTOFileAs(ex.StackTrace);
            }
        }

        public bool IsOn { get => isOn; }

        public IAsyncAction EndTask()
        {
            return Task.Run(StopPump).AsAsyncAction();
        }


        private async Task StartPump()
        {
            if (modulePwr)
            {
                try
                {
                    ///Dont wanna create a landslide! hence awaitied some time for water to settle 
                    await InitializeBoard.Initialize();
                    await StartPumpFor(TimeSpan.FromSeconds(6));
                   // InitializeBoard.Close();
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    //await InitializeBoard.Initialize();
                    await StartPumpFor(TimeSpan.FromSeconds(20));
                    InitializeBoard.Close();


                }
                catch (Exception ex)
                {

                    ioF.writeTOFileAs(ex.StackTrace);
                }
            }

        }

        private async Task StartPumpFor(TimeSpan t)
        {
            Initialize();
            await ioF.writeTOFileAs("started pump");
            pin.Write(GpioPinValue.High);
            isOn = true;
            await Task.Delay(t);
            Close();
        }

        public IAsyncAction StartTask()
        {

            return Task.Run(StartPump).AsAsyncAction();
        }

        public IAsyncAction  ManualStartPump()
        {
           
            return Task.Run(ManualStart).AsAsyncAction();
        }
        public void ManulStopPump()
        {
            InitializeBoard.Close();
            Close();
        }
        private async Task ManualStart()
        {
            await InitializeBoard.Initialize();
            Initialize();
            await ioF.writeTOFileAs("started pump");
            pin.Write(GpioPinValue.High);
            isOn = true;
        }
        

        private async Task StopPump()
        {
            if (modulePwr)
            {
                //try
                //{
                //    await ioF.writeTOFileAs("stopped pump");
                //    pin.Write(GpioPinValue.Low);
                //    isOn = false;
                    
                //}
                //catch (Exception ex)
                //{
                //    ioF.writeTOFileAs(ex.StackTrace);

                //}
            }
        }
        private async Task PumpTest()
        {
            if (modulePwr)
            {
                try
                {
                    await InitializeBoard.Initialize();
                    await StartPumpFor(TimeSpan.FromSeconds(2));


                    InitializeBoard.Close();




                }
                catch (Exception ex)
                {

                    ioF.writeTOFileAs(ex.StackTrace);
                }
            }
        }

        public  IAsyncAction TestModule()
        {
            return Task.Run(PumpTest).AsAsyncAction();
            
        }
    }
}
