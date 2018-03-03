using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MyLib
{
    public class RabbitMqJobExecuter
    {
        private IModel _channel;
        private IConnection _connection;

        public void SubscribeToQueueUntilCancelled()
        {
            Console.WriteLine(
                $"Working, ThreadPool: {Thread.CurrentThread.IsThreadPoolThread}, Thread ID: {Thread.CurrentThread.ManagedThreadId}");
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "admin",
                Password = "Passw0rd1"
            };

            _connection = factory.CreateConnection("Subscriber");
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(
                queue: Constants.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            // EventingBasicConsumer fires the handler subscribed to consumer.Received event
            // As long as the thread containing this code is running
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [ _/ ] Received {message}, Correlation Id: {ea.BasicProperties.CorrelationId}");
            };
            _channel.BasicConsume(queue: Constants.QueueName, noAck: true, consumer: consumer);
        }

        public void Stop()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}