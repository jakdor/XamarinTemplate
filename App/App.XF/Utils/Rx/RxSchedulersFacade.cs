using System.Reactive.Concurrency;
using System.Threading;

namespace App.XF.Utils.Rx
{
    /// <summary>
    /// Facade for changing Schedulers during testing
    /// Naming scheme consistent with global ReactiveX conventions
    /// </summary>
    public class RxSchedulersFacade : IRxSchedulersFacade
    {
        /// <summary>
        /// Schedulers pool with smart creation and re-use
        /// </summary>
        public IScheduler IO()
        {
            return TaskPoolScheduler.Default; 
        }

        /// <summary>
        /// Schedulers pool limited by device CPU threads number 
        /// </summary>
        public IScheduler Computation()
        {
            return ThreadPoolScheduler.Instance;
        }

        /// <summary>
        /// Spawns new thread for each observable
        /// </summary>
        public IScheduler NewThread()
        {
            return new NewThreadScheduler();
        }

        /// <summary>
        /// Platform default scheduler
        /// </summary>
        public IScheduler Default()
        {
            return DefaultScheduler.Instance;
        }

        /// <summary>
        /// Runs task on current thread
        /// </summary>
        public IScheduler Trampoline()
        {
            return CurrentThreadScheduler.Instance;
        }

        /// <summary>
        /// Used for synchronizing with UI thread
        /// </summary>
        public IScheduler UI()
        {
            return new SynchronizationContextScheduler(SynchronizationContext.Current);
        }
    }
}
