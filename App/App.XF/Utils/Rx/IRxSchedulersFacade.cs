using System.Reactive.Concurrency;

namespace App.XF.Utils.Rx
{
    public interface IRxSchedulersFacade
    {
        IScheduler IO();
        IScheduler Computation();
        IScheduler NewThread();
        IScheduler Default();
        IScheduler Trampoline();
        IScheduler UI();
    }
}
