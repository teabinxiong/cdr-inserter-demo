﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdr.Inserter.Monitor
{
    public class Global
    {
        public static Serilog.ILogger Logger;

        public static List<ManualResetEvent> ThreadCompleteEvents = new List<ManualResetEvent>();

        public const int MaxThreads = 5;

        public const string TopicCdrReport = "cdr-report";
    }
}
