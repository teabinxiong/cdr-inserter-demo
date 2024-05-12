using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdr.Inserter.Monitor.Kafka
{
    public class KafkaClientHandle 
    {
        ConsumerBuilder<Ignore, string> kafkaConsumerBuilder;
        public KafkaClientHandle(IConfiguration config)
        {
            var conf = new ConsumerConfig();

            config.GetSection("Kafka:ConsumerSettings").Bind(conf);
       
                 kafkaConsumerBuilder = new ConsumerBuilder<Ignore, string>(conf)
                   .SetErrorHandler((_, e) => Global.Logger.Error($"Error: {e.Reason}"))
                   .SetPartitionsAssignedHandler((c, partitions) =>
                   {
                       Global.Logger.Information($"Assigned partitions: {string.Join(", ", partitions)}");
                   })
                   .SetPartitionsRevokedHandler((c, partitions) =>
                   {
                       Global.Logger.Information($"Revoked partitions: {string.Join(", ", partitions)}");
                   });

        }

        public ConsumerBuilder<Ignore, string> ConsumerBuilder
        {
            get => this.kafkaConsumerBuilder;
        }


    }
}
