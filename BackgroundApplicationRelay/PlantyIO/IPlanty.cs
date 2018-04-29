using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace BackgroundApplicationRelay.PlantyIOT
{
    public interface IPlanty
    {
        IAsyncAction StartTask();
        IAsyncAction EndTask();
    }
}
