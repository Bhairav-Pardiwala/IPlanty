using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace BackgroundApplicationRelay.PlantyIOT
{
    public sealed class PlantyEngine
    {
        private System.Collections.Generic.IEnumerable<PlantySlot> watcherList;


        private int cnt = 0;

        private IAsyncAction waitTask;

        public int Cnt { get => cnt; }
        public System.Collections.Generic.IEnumerable<PlantySlot> WatcherList { get => watcherList; set => watcherList = value; }
        public IAsyncAction WaitTask { get => waitTask; set => waitTask = value; }
        public bool IsComplete { get => isComplete; set => isComplete = value; }

        private bool isComplete = true;
        private bool allModulesTested = false;
        private bool testinProgress = false;

        public PlantyEngine()
        {
            WatcherList = new List<PlantySlot>();
        }
        public async void Initialize()
        {
            if(!testinProgress)
            {
                testinProgress = true;
                foreach (var item in watcherList)
                {
                    await item.TestModule();
                }
                allModulesTested = true;
            }
           
            testinProgress = false;
        }

        public void tick()
        {
            if(!allModulesTested)
            {
                Initialize();
                return;
            }
            ////start first item and quit if first item task has completed start second item 
            //if last item then go back to first item 
            if (WatcherList.Count() > 0)
            {
                if (cnt > WatcherList.Count() - 1 && IsComplete)
                {
                    cnt = 0;
                }
                if (IsComplete)
                {
                    IsComplete = false;
                    WaitTask = WatcherList.ElementAt(cnt).runAsync();
                    WaitTask.Completed = test;
                    

                  
                    cnt += 1;
                }
                //if (waitTask.IsCompleted)
                //{
                //    isComplete = true;
                //}
            }


        }

        private void test(IAsyncAction asyncInfo, AsyncStatus asyncStatus)
        {
            if (asyncStatus == AsyncStatus.Completed)
            {
                IsComplete = true;
            }
        }
    }
}
