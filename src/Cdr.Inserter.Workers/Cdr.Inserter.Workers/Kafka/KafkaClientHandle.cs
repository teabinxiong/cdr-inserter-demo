using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdr.Inserter.Workers.Kafka
{
    public class KafkaClientHandle : IDisposable
    {
        IProducer<byte[], byte[]> kafkaProducer;
        public KafkaClientHandle(IConfiguration config)
        {
            var conf = new ProducerConfig();

            config.GetSection("Kafka:ProducerSettings").Bind(conf);
            this.kafkaProducer = new ProducerBuilder<byte[], byte[]>(conf).Build();
        }
        public Handle Handle
        {
            get => this.kafkaProducer.Handle;
        }

        public void Dispose()
        {
            kafkaProducer.Flush();
            kafkaProducer.Dispose();
        }
    }
}
