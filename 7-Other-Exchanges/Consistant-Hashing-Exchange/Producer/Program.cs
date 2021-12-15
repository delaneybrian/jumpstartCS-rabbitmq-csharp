using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory() { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "samplehashing", "x-consistent-hash");

var message = "Hello hash the routing key and pass me on please!";

var routingKeyToHash = "hash me!";

var body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish("samplehashing", routingKeyToHash, null, body);

Console.WriteLine($"Send message: {message}");