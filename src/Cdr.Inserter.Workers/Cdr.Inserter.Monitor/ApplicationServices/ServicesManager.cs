using Cdr.Inserter.Monitor.ApplicationServices.WorkerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdr.Inserter.Monitor.ApplicationServices
{
    public sealed class ServicesManager
    {

        CancellationTokenSource cts = new CancellationTokenSource();

        private readonly CdrRecordMonitor _cdrRecordMonitor;

        public ServicesManager(CdrRecordMonitor cdrRecordMonitor)
        {
            _cdrRecordMonitor = cdrRecordMonitor;
        }

        public void StartAllThread()
        {
            ThreadPool.QueueUserWorkItem(_cdrRecordMonitor.StartThreadProc, cts.Token);
        }

        public void StopAllThread()
        {
            Global.Logger.Information("StopAllThread");
            _cdrRecordMonitor.StopThread();

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
