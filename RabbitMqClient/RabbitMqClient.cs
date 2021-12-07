
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMqClient
{
    public class RabbitMqClient : IRabbitMqClient
    {
        private const string CONFIG_QUEUE = "serverMessages";
        private readonly IConnection rabbitMqConnection;
        private readonly IModel rabbitMqChannel;

        public RabbitMqClient()
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };
            this.rabbitMqConnection = factory.CreateConnection();
            this.rabbitMqChannel = rabbitMqConnection.CreateModel();
        }

        public void PublishMessageToQueue(string queueName,string message)
        {
            //var factory = new ConnectionFactory()
            //{
            //    HostName = "localhost",
            //    UserName = ConnectionFactory.DefaultUser,
            //    Password = ConnectionFactory.DefaultPass,
            //    Port = AmqpTcpEndpoint.UseDefaultPort
            //};

            //using (var connection = factory.CreateConnection())
            //using (var channel = connection.CreateModel())
            //{
            rabbitMqChannel.QueueDeclare(queue: queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                //string message = "hello world";
                var body = Encoding.UTF8.GetBytes(message);

            rabbitMqChannel.BasicPublish(exchange: "",
                                     routingKey: queueName,
                                     basicProperties: null,
                                     body: body);
            //}
        }

        public async Task<string> ConsumeMessageFromQueue(string queueName)
        {

            var tcs = new TaskCompletionSource<string>();


            //declare the queue  
            //declare client messages queue
            rabbitMqChannel.QueueDeclare(queue: queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
            rabbitMqChannel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            //consume the message received  
            var consumer = new EventingBasicConsumer(rabbitMqChannel);
            var message = String.Empty;
            consumer.Received += (model, args) =>
            {
                var body = args.Body;
                message = Encoding.UTF8.GetString(body.ToArray());
                tcs.TrySetResult(message);
                rabbitMqChannel.BasicAck(deliveryTag: args.DeliveryTag, multiple: false);
                Thread.Sleep(1000);

            };
             rabbitMqChannel.BasicConsume(queue: queueName,
                                         autoAck: false,
                                         consumer: consumer);
            return await tcs.Task;
        }
      
    }
}
