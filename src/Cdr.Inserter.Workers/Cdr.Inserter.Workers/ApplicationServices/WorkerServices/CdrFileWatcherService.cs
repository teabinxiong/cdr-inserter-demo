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
        public override void StartThreadProc(object obj)
        {
            var path = (string)obj;

            Global.ThreadCompleteEvents.Add(this.completeEvent);

            //Create a new FileSystemWatcher.
            FileSystemWatcher watcher = new FileSystemWatcher();

            //Set the filter to all files.
            watcher.Filter = "*.txt";

            //Subscribe to the Created event.
            watcher.Created += new FileSystemEventHandler(watcher_FileCreated);

            //Set the path 
            watcher.Path = path;

            //Enable the FileSystemWatcher events.
            watcher.EnableRaisingEvents = true;
        }

        private void watcher_FileCreated(object sender, FileSystemEventArgs e)
        {
            //Parse text file.
            FileInfo objFileInfo = new FileInfo(e.FullPath);
            if (!objFileInfo.Exists) return;
            ParseMessage(e.FullPath);

            // TODO: Run the task in thread pools to preocess the file
        }

        private void ParseMessage(string fullPath)
        {
            Global.Logger.Information(fullPath);
            // read file here
        }
    }
}
