namespace RabbitMqClient
{
    public interface IRabbitMqClient
    {
         Task<string> ConsumeMessageFromQueue(string queueName);
         void PublishMessageToQueue(string queueName, string message);


    }
}