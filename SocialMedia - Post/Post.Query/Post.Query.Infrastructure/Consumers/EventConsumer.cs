using System.Text.Json;
using Confluent.Kafka;
using CQRS.Core.Consumers;
using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using Post.Query.Infrastructure.Converters;
using Post.Query.Infrastructure.Handlers;

namespace Post.Query.Infrastructure.Consumers
{
    public class EventConsumer : IEventConsumer
    {
        private readonly ConsumerConfig _config;
        private readonly IEventHandler _eventHandler;

        public EventConsumer(IOptions<ConsumerConfig> config, IEventHandler eventHandler)
        {
            _config = config.Value;
            _eventHandler = eventHandler;
        }

        public void Consume(string topic)
        {
            // Implementation for consuming events from Kafka and invoking the appropriate handler methods
            // This is a placeholder and should be implemented based on the Kafka consumer library being used
            using var consumer = new ConsumerBuilder<string, string>(_config)
            .SetKeyDeserializer(Deserializers.Utf8)
            .SetValueDeserializer(Deserializers.Utf8)
            .Build();

            consumer.Subscribe(topic);

            while (true)
            {
                var consumeResult = consumer.Consume();
                if(consumeResult?.Message == null) continue;                
                
                var options = new JsonSerializerOptions
                {
                    Converters = { new EventJsonConverter() }
                };

                var @event = JsonSerializer.Deserialize<BaseEvent>(consumeResult.Message.Value, options);
                
                //Reflection - Runtime type discovery and method invocation
                var handlerMethod = _eventHandler.GetType().GetMethod("On", new Type[] { @event.GetType() });
                if(handlerMethod == null)
                {
                    throw new ArgumentNullException($"No handler found for event type: {nameof(handlerMethod)}");
                }

                handlerMethod.Invoke(_eventHandler, new object[] { @event });
                consumer.Commit(consumeResult);       
            }
        }
    }
}