using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace BackgroundApplicationRelay.PlantyIOT
{
    public sealed class PlantySlot
    {
        /// <summary>
        /// Starts from when for current date
        /// </summary>
        private DateTime? starttime;
        /// <summary>
        /// ends from when fro current date
        /// </summary>
        private DateTime endtime;
        /// <summary>
        /// set duration for task to run 
        /// </summary>
        private TimeSpan duration;

        /// <summary>
        /// the time span to start task 
        /// </summary>
        private TimeSpan toStart;

        private DateTime lastrun;

        private bool isRunning, isCompleted, isByPassed;

        private IPlanty plantyIO;

        public IPlanty PlantyIO { get => plantyIO; set => plantyIO = value; }

        public PlantySlot(TimeSpan duration, TimeSpan tostart, IPlanty p)
        {

            this.duration = duration;
            this.toStart = tostart;
            PlantyIO = p;
        }
        public  IAsyncAction TestModule()
        {
            return plantyIO.TestModule();
        }
      

        //public void setStartTime(DateTime dt)
        //{
        //    starttime = dt;
        //}
        //public void setendTime(DateTime dt)
        //{
        //    endtime = dt;
        //}
        public void setduration(TimeSpan ts)
        {
            duration = ts;
        }
        public void setToStart(TimeSpan ts)
        {
            toStart = ts;
        }
        public IAsyncAction runAsync()
        {
            if (!starttime.HasValue)
            {
                return Task.Run(SetParametersAndDecide).AsAsyncAction();
            }
            else if (lastrun.Date != DateTime.Now.Date && isCompleted)
            {
                isCompleted = false;
                return Task.Run(SetParametersAndDecide).AsAsyncAction();
            }
            else if (!isCompleted)
            {
                return Task.Run(DecideOnTask).AsAsyncAction();

            }
            return Task.CompletedTask.AsAsyncAction();
            //return Task.Run(Task.Delay(5)).AsAsyncAction();
            ///if startime not set then 
            ///get the system date and add timespan to it fro start time 
            ///add duration to starttime for end time and check 
            ///else check if date is current date or previous date 
            /// if current date then check if current time within elapsed task 
        }
        private async Task dummyTask()
        {
            //this is just a dummy task
        }

        private async Task SetParametersAndDecide()
        {

            starttime = DateTime.Today.Add(toStart);
            endtime = starttime.Value.Add(duration);
            await DecideOnTask();
        }

        /// <summary>
        /// this method requires all vairables to be declared and set before running 
        /// </summary>
        private async Task DecideOnTask()
        {
            if (DateTime.Now > starttime && DateTime.Now < endtime)
            {
                if (!isRunning && !isCompleted)
                {
                    await StartTask();
                }
                //else he is running in parameters 
            }
            else if (DateTime.Now > endtime && !isCompleted && isRunning)
            {
                await EndTask();
            }
            else if (DateTime.Now > endtime && !isCompleted && !isRunning)
            {
                lastrun = DateTime.Now.Date;
                isRunning = false;
                isCompleted = true;
                isByPassed = true;
            }
        }

        private async Task StartTask()
        {

            isRunning = true;
            await PlantyIO.StartTask();
        }
        private async Task EndTask()
        {
            lastrun = DateTime.Now.Date;
            isRunning = false;
            isCompleted = true;
            isByPassed = false;
            lastrun = endtime;

            //  lastrun = endtime.AddDays(-1);
            await PlantyIO.EndTask();
        }
    }
}
