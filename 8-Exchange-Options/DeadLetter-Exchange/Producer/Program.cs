using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory() { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(
    exchange: "mainexchange", 
    type: ExchangeType.Direct);

var message = "This message might expire";

var body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish("mainexchange", "test", null, body);

Console.WriteLine($"Send message: {message}");