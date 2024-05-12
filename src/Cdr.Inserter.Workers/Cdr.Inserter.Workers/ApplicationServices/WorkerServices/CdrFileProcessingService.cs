using Cdr.Inserter.Workers.ApplicationServices.WorkerServices.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdr.Inserter.Workers.ApplicationServices.WorkerServices
{
    public sealed class CdrFileProcessingService : WorkerProcess
    {
        public override void StartThreadProc(object obj)
        {
            var filePath = (string)obj;

            Global.ThreadCompleteEvents.Add(this.completeEvent);

            Global.Logger.Information($"Start Processing {filePath}");



        }
    }
}
