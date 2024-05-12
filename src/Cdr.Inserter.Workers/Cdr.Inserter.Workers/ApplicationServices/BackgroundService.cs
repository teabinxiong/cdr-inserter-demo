using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdr.Inserter.Workers.ApplicationServices
{
    public sealed class BackgroundService
    {
        private ServicesManager _servicesManager;
        public BackgroundService()
        {
            _servicesManager = new ServicesManager();
        }

        public void Start()
        {
            Global.Logger.Information("Start Data Processing Service");

            _servicesManager.StartAllThread();

            Global.Logger.Information("All services started");
        }

        public void Stop()
        {
            Console.WriteLine("Quit Data Processing Service");

            Global.Logger.Information("Quit program!");
            _servicesManager.StopAllThread();
        }
    }
}
