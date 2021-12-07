namespace RabbitMqClient
{
    public interface IServiceMesssages
    {
        void PublishMessages(string queuName);
        void PublishStopMessages(string queuName);
        Task ConsumeMessaeges(string queuName);

    }
}
