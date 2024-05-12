using Cdr.Inserter.Workers.ApplicationServices.WorkerServices.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdr.Inserter.Workers.ApplicationServices.WorkerServices
{
    public sealed class CdrFileWatcherService : WorkerProcess, IDisposable
    {
        private List<WorkerProcess> workerProcesses = new List<WorkerProcess>();
        private readonly IServiceProvider _serviceProvider;
        private readonly FileSystemWatcher _fileWatcher;

        public CdrFileWatcherService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _fileWatcher = new FileSystemWatcher();
        }

        public override void StartThreadProc(object obj)
        {
            var cts = (CancellationToken)obj;

            Global.ThreadCompleteEvents.Add(this.completeEvent);

            //Create a new FileSystemWatcher.


            //Set the filter to all files.
            _fileWatcher.Filter = "*.txt";

            //Subscribe to the Created event.
            _fileWatcher.Created += new FileSystemEventHandler(watcher_FileCreated);

            //Set the path 
            _fileWatcher.Path = Global.StoragePath;

            //Enable the FileSystemWatcher events.
            _fileWatcher.EnableRaisingEvents = true;
        }

        private void watcher_FileCreated(object sender, FileSystemEventArgs e)
        {
            //Parse text file.
            FileInfo objFileInfo = new FileInfo(e.FullPath);
            if (!objFileInfo.Exists) return;
            ParseMessage(e.FullPath);

            // run task in a new thread to handle csv file processing
            using (var scope = _serviceProvider.CreateScope())
            {
                var fileProcessingService = scope.ServiceProvider.GetService<CdrFileProcessingService>();
                
                workerProcesses.Add(fileProcessingService);

                ThreadPool.QueueUserWorkItem(fileProcessingService.StartThreadProc, e.FullPath);
            }
        }

        private void ParseMessage(string fullPath)
        {
            // log file path
            Global.Logger.Information(fullPath);
        }

        public override void StopThread()
        {
            base.StopThread();

            // inform the child workers to stop future processing in preparation for the gracefully shut down
            foreach(var process in workerProcesses)
            {
                process.StopThread();
            }

            this.Dispose();
        }

        public void Dispose()
        {
            _fileWatcher.Dispose();
            completeEvent.Set();
        }
    }
}
