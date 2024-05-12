using Cdr.Inserter.Monitor.ApplicationServices.WorkerServices.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdr.Inserter.Monitor.ApplicationServices.WorkerServices
{
    public class CdrRecordMonitor : WorkerProcess
    {
        public CdrRecordMonitor()
        {
            
        }

        public override void StartThreadProc(object obj)
        {
            Global.ThreadCompleteEvents.Add(this.completeEvent);


        }
    }
}
