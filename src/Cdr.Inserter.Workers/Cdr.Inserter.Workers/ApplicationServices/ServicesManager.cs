using Cdr.Inserter.Workers.ApplicationServices.WorkerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdr.Inserter.Workers.ApplicationServices
{
    public sealed class ServicesManager
    {
        
        CancellationTokenSource cts = new CancellationTokenSource();

        private readonly CdrFileWatcherService _cdrFileWatcherService;

        public ServicesManager(CdrFileWatcherService cdrFileWatcherService)
        {
            _cdrFileWatcherService = cdrFileWatcherService;
        }

        public void StartAllThread()
        {
            ThreadPool.QueueUserWorkItem(_cdrFileWatcherService.StartThreadProc, cts.Token);
        }

        public void StopAllThread()
        {
            Global.Logger.Information("StopAllThread");
            _cdrFileWatcherService.StopThread();

            cts.Cancel();

            Global.Logger.Information("Wait for All theread to exit....");

            foreach (ManualResetEvent stopEvent in Global.ThreadCompleteEvents)
            {
                stopEvent.WaitOne();
            }

            Global.Logger.Information("Gracfully shutdown");
        }
    }
}
