using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using Windows.System.Threading;
using Windows.Devices.Gpio;
using System.Threading.Tasks;
using BackgroundApplicationRelay.PlantyIOT;
using Windows.Foundation;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace BackgroundApplicationRelay
{
    public sealed class StartupTask : IBackgroundTask
    {
        BackgroundTaskDeferral _deferral;
        private ThreadPoolTimer timer,snapshottimer;
        bool isOn = false;
       // int gpioPin = 4;
       // GpioController controll;
       // GpioPin pin;

        DateTime? startime;
        DateTime? endtime;
        PlantySlot watch,restart;
        PlantyEngine eng = new PlantyEngine();

        TimeSpan duration = TimeSpan.FromSeconds(5);
        PumpIO io = new PumpIO();
        LedBlink ledblb = new LedBlink();
        SnapshotMod snapmod;
        InitializeBoard boardInitializer = new InitializeBoard();

        /*
        *  public IAsyncOperation<string> GetUriContentAsync(string uri)
   {
       return this.GetUriContentAsynHelper(uri).AsAsyncOperation();
   }

        */

        public void Run(IBackgroundTaskInstance taskInstance)
        {
           // controll = GpioController.GetDefault();
           // pin = controll.OpenPin(gpioPin);
           // pin.SetDriveMode(GpioPinDriveMode.Output);
            // 
            // TODO: Insert code to perform background work
            //
            // If you start any asynchronous methods here, prevent the task
            // from closing prematurely by using BackgroundTaskDeferral as
            // described in http://aka.ms/backgroundtaskdeferral
            //
            _deferral = taskInstance.GetDeferral();
            //snapmod = new SnapshotMod();
           // snapmod.TakePic();
            startime = DateTime.Today.AddMinutes(50);

            //=28800 +1800
          // watch = new PlantySlot(TimeSpan.FromSeconds(15), DateTime.Now.Add(TimeSpan.FromSeconds(2)).Subtract(DateTime.Now.Date), io);

           watch = new PlantySlot(TimeSpan.FromMinutes(15),TimeSpan.FromHours(8).Add(TimeSpan.FromMinutes(30)), io);
            (eng.WatcherList as List<PlantySlot>).Add(watch);

            // restart = new PlantySlot(TimeSpan.FromMinutes(5), TimeSpan.FromHours(23).Add(TimeSpan.FromMinutes(30)), new SleepModule());
           // restart = new PlantySlot(TimeSpan.FromMinutes(4), DateTime.Now.Add(TimeSpan.FromSeconds(50)).Subtract(DateTime.Now.Date), new SleepModule());
           // (eng.WatcherList as List<PlantySlot>).Add(restart);
            this.timer = ThreadPoolTimer.CreatePeriodicTimer(Timer_Tick, TimeSpan.FromMinutes(2));

            //Snapshot timer uncomment to enable and flip the bool variable
            //snapshottimer= ThreadPoolTimer.CreatePeriodicTimer(TakeSnapshot, TimeSpan.FromMinutes(5));

            // this.timer = ThreadPoolTimer.CreatePeriodicTimer(Timer_Tick, TimeSpan.FromSeconds(10));
            ledblb.TestModule();
            eng.Initialize();
        }

        private void TakeSnapshot(ThreadPoolTimer timer)
        {
            snapmod.TakePic();
        }
        

        private async  void Timer_Tick(ThreadPoolTimer timer)
        {
          // await  io.StartTask();
          
           await ledblb.Blink();
            eng.tick();
            //if(startime.Value<startime.Value)
            //{
            //    //check if end time has elpsed 
            //    StartTask();
            //}
            //else
            //{
            //    //end task 
            //    if (endtime.Value < DateTime.Now)
            //    {
            //        EndTask();
            //    }

            //}

        }
        //private async Task EndTask()
        //{
        //    await Task.Delay(TimeSpan.FromSeconds(1));
        //}

        //private async Task StartTask()
        //{
        //    await Task.Delay(TimeSpan.FromSeconds(30));
        //}

        //private void EndTask()
        //{
        //    DoTask();
        //    startime = null;
        //    endtime = null;
        //}

        //private void StartTask()
        //{
        //    startime = DateTime.Now;
        //    DoTask();
        //    endtime = startime.Value.Add(duration);
        //}
        //private void DoTask()
        //{
        //    if (isOn)
        //    {

        //        pin.Write(GpioPinValue.Low);
        //        isOn = false;
        //    }
        //    else
        //    {
        //        pin.Write(GpioPinValue.High);
        //        isOn = true;
        //    }
        //}
    }
}
