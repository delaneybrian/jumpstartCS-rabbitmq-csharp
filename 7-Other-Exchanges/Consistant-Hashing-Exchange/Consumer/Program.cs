using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory() { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "samplehashing", "x-consistent-hash");

var queue1 = channel.QueueDeclare("letterbox1");
var queue2 = channel.QueueDeclare("letterbox2");

channel.QueueBind("letterbox1", "samplehashing", "1");
channel.QueueBind("letterbox2", "samplehashing", "10");

var consumer1 = new EventingBasicConsumer(channel);
consumer1.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Queue1 - Recieved new message: {message}");
};

var consumer2 = new EventingBasicConsumer(channel);
consumer2.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Queue2 - Recieved new message: {message}");
};

channel.BasicConsume(queue: "letterbox1", autoAck: true, consumer: consumer1);
channel.BasicConsume(queue: "letterbox2", autoAck: true, consumer: consumer2);

Console.WriteLine("Consuming");

Console.ReadKey();