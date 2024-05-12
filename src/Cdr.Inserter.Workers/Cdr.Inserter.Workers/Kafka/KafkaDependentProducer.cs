using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdr.Inserter.Workers.Kafka
{
    public class KafkaDependentProducer<K, V>
    {
        IProducer<K, V> kafkaHandle;
        public KafkaDependentProducer(KafkaClientHandle handle)
        {
            kafkaHandle = new DependentProducerBuilder<K, V>(handle.Handle).Build();
        }

        public Task ProduceAsync(string topic, Message<K, V> message) => this.kafkaHandle.ProduceAsync(topic, message);

        public void Produce(
            string topic,
            Message<K, V> message,
            Action<DeliveryReport<K, V>> deliveryHandler = null) => this.kafkaHandle.Produce(topic, message, deliveryHandler);

        public void Flush(TimeSpan timeout) => this.kafkaHandle.Flush(timeout);
    
    }
}
