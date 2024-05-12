using Cdr.Inserter.Monitor.ApplicationServices.WorkerServices.Abstraction;
using Cdr.Inserter.Monitor.Kafka;
using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdr.Inserter.Monitor.ApplicationServices.WorkerServices
{
    public class CdrRecordMonitor : WorkerProcess
    {
        private readonly KafkaClientHandle _kafkaClientHandle;
        public CdrRecordMonitor(KafkaClientHandle kafkaClientHandle)
        {
            _kafkaClientHandle = kafkaClientHandle;
        }

        public override void StartThreadProc(object obj)
        {
            try
            {
                Global.ThreadCompleteEvents.Add(this.completeEvent);
                using (var consumer = _kafkaClientHandle.ConsumerBuilder.Build())
                {
                    consumer.Subscribe(Global.TopicCdrReport);

                    while (true)
                    {
                        if (IsThreadStopped())
                        {
                            completeEvent.Set();
                            return;
                        }

                        try
                        {
                            var consumeResult = consumer.Consume();

                            // print the message
                            Global.Logger.Information($"Thread Id: {Thread.CurrentThread.ManagedThreadId}; Received message: {consumeResult.Message.Value}");


                            // acks
                            consumer.Commit(consumeResult);
                        }
                        catch (ConsumeException e)
                        {
                            Global.Logger.Error($"Error consuming message: {e.Error.Reason}");
                        }

                    }
                }
            }catch(Exception ex)
            {
                Global.Logger.Error(ex.ToString());
                completeEvent.Set();

            }

        }
    }
}
