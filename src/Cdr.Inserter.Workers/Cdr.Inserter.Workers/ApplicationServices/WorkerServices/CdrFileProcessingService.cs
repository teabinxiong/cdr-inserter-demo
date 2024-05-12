using Cdr.Inserter.Workers.ApplicationServices.WorkerServices.Abstraction;
using Cdr.Inserter.Workers.Kafka;
using Cdr.Inserter.Workers.Models;
using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Confluent.Kafka.ConfigPropertyNames;

namespace Cdr.Inserter.Workers.ApplicationServices.WorkerServices
{
    public sealed class CdrFileProcessingService : WorkerProcess
    {
        private readonly KafkaDependentProducer<string, string> _producer;
        public CdrFileProcessingService(KafkaDependentProducer<string, string> producer)
        {
            this._producer = producer;
        }
        public override void StartThreadProc(object obj)
        {
            var filePath = (string)obj;

            Global.ThreadCompleteEvents.Add(this.completeEvent);

            Global.Logger.Information($"Start Processing {filePath}");

            var csvRows = System.IO.File.ReadAllLines(filePath, Encoding.Default).ToList();

            var cdrRecords = new List<CdrRecord>();
            foreach (var row in csvRows.Skip(1))
            {
                if (IsThreadStopped())
                {
                    completeEvent.Set();
                    return;
                }

                var columns = row.Split(';');

                var cdrRecord = new CdrRecord();
                cdrRecord.Msidn = columns[0];
                cdrRecord.Imsi = columns[1];
                cdrRecord.Imei = columns[2];
                cdrRecord.Plan = columns[3];
                cdrRecord.CallType = columns[4];
                cdrRecord.CorrespType = columns[5];
                cdrRecord.CorrespIsdn = columns[6];
                cdrRecord.Duration = int.Parse(columns[7]);
                cdrRecord.Time = columns[8];
                cdrRecord.Date = columns[9];
                cdrRecords.Add(cdrRecord);
                Global.Logger.Information(cdrRecord.ToString());

                // publish to kafka
                PublishMessage(cdrRecord);

            }

        }
        private void PublishMessage(CdrRecord cdrRecords)
        {
 
            if(IsThreadStopped())
            {
                completeEvent.Set();
                return;
            }
            
            _producer.Produce(
                Global.TopicCdrReport,
                new Message<string, string> { Key = cdrRecords.Msidn, Value = JsonConvert.SerializeObject(cdrRecords) },
                deliveryReportHandler
            );
            
        }

        private void deliveryReportHandler(DeliveryReport<string, string> deliveryReport)
        {
            if (deliveryReport.Status == PersistenceStatus.Persisted)
            {
                // success logic
                Global.Logger.Information(JsonConvert.SerializeObject(deliveryReport));
            }
            if (deliveryReport.Status == PersistenceStatus.NotPersisted)
            {
                // add error handling
                Global.Logger.Error(JsonConvert.SerializeObject(deliveryReport));
            }
        }
    }
}
