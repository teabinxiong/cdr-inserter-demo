using Cdr.Inserter.Workers.ApplicationServices.WorkerServices.Abstraction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdr.Inserter.Workers.ApplicationServices.WorkerServices
{
    public sealed class CdrFileWatcherService : WorkerProcess
    {
        private List<WorkerProcess> workerProcesses = new List<WorkerProcess>();

        public override void StartThreadProc(object obj)
        {
            var cts = (CancellationToken)obj;

            Global.ThreadCompleteEvents.Add(this.completeEvent);

            //Create a new FileSystemWatcher.
            FileSystemWatcher watcher = new FileSystemWatcher();

            //Set the filter to all files.
            watcher.Filter = "*.txt";

            //Subscribe to the Created event.
            watcher.Created += new FileSystemEventHandler(watcher_FileCreated);

            //Set the path 
            watcher.Path = Global.StoragePath;

            //Enable the FileSystemWatcher events.
            watcher.EnableRaisingEvents = true;
        }

        private void watcher_FileCreated(object sender, FileSystemEventArgs e)
        {
            //Parse text file.
            FileInfo objFileInfo = new FileInfo(e.FullPath);
            if (!objFileInfo.Exists) return;
            ParseMessage(e.FullPath);

            // run task in a new thread to handle csv file processing
            var fileProcessingService = new CdrFileProcessingService();
            workerProcesses.Add(fileProcessingService);

            ThreadPool.QueueUserWorkItem(fileProcessingService.StartThreadProc, e.FullPath);
        }

        private void ParseMessage(string fullPath)
        {
            // log file path
            Global.Logger.Information(fullPath);
        }

        public override void StopThread()
        {
            base.StopThread();
            foreach(var process in workerProcesses)
            {
                process.StopThread();
            }
        }
    }
}
