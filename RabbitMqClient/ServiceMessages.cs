using Microsoft.Extensions.Logging;
using RabbitMqClient;

namespace RabbitMqClient
{
    public class ServiceMessages : IServiceMesssages
    {
        private readonly IRabbitMqClient _rabbitMqClient;
        private readonly ILogger<ServiceMessages> _logger;
        public ServiceMessages(IRabbitMqClient rabbitMqClient, ILogger<ServiceMessages> logger)
        {
            _rabbitMqClient = rabbitMqClient;
            _logger = logger;

        }
        public void PublishMessages(string queueName)
        {        
            _rabbitMqClient.PublishMessageToQueue(queueName, "hello this is message" );       
        }

        public async Task ConsumeMessaeges(string queueName)
        {
            var message = await _rabbitMqClient.ConsumeMessageFromQueue(queueName);
            if (message != string.Empty)
            {
                _logger.LogInformation("recieved message: " + message);
            }

        }

        public void PublishStopMessages(string queueName)
        {
            _rabbitMqClient.PublishMessageToQueue(queueName, "stop connection");
        }
    }
}
