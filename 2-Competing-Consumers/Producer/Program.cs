using System;
using System.Threading.Tasks;
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory() { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.QueueDeclare(
    queue: "letterbox",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);

var messageId = 1;

var random = new Random();

while(true)
{
    var message = $"Sending Message Id: {messageId}";

    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish("", "letterbox", null, body);

    Console.WriteLine($"Send message: {message}");  

    var waitTime = random.Next(1, 4);

    Task.Delay(TimeSpan.FromSeconds(waitTime)).Wait();

    messageId++;
}