using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MyLib;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;
using Constants = MyLib.Constants;

namespace Startup
{
    internal class Program
    {
        private static CancellationTokenSource _cancellationTokenSource;

        public static void Main(string[] args)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _cancellationTokenSource.Token;
            var executor = new RabbitMqJobExecuter();
            
            Task.Run(() =>
            {
                Send();
            }, cancellationToken);

            /* RUN AS A DEDICATED THREAD */
//            Task.Factory.StartNew(() =>
//            {
//                executor.SubscribeToQueueUntilCancelled();
//            }, TaskCreationOptions.LongRunning);

            /* RUN ON A THREAD POOL THREAD */
            Task.Run(() =>
            {
                executor.SubscribeToQueueUntilCancelled();
            }, cancellationToken);


            Console.WriteLine("In Main, Done, Press enter to stop sending messages");
            Console.ReadLine();
            _cancellationTokenSource.Cancel();
            executor.Stop();
            Console.WriteLine("After cancellation, press any key to terminate");

            Console.ReadLine();
        }

        private static void Send()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "admin",
                Password = "Passw0rd1"
            };
            using (var connection = factory.CreateConnection("Publisher"))
            using (var channel = connection.CreateModel())
            {
                // QueueDeclare ensures the Queue is created if does not exist on the
                // Specified vshost
                channel.QueueDeclare(
                    queue: Constants.QueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    string message = $"{DateTime.UtcNow} | Hello RabbitMQ! ";
                    var body = Encoding.UTF8.GetBytes(message);
                    var correlationId = Guid.NewGuid().ToString();
                    channel.BasicPublish(
                        exchange: "",
                        routingKey: Constants.QueueName,
                        basicProperties: new BasicProperties
                        {
                            CorrelationId = correlationId
                        }, 
                        body: body);
                    Console.WriteLine($" [ _/ ] Message sent, CorrelationId: {correlationId} ");
                    Console.WriteLine("--------------");
                    Task.Delay(TimeSpan.FromSeconds(2)).Wait();
                }
            }
        }
    }
}
