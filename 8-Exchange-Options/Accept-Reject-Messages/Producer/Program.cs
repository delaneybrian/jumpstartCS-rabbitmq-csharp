using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory() { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(
    exchange: "acceptrejectexchange", 
    type: ExchangeType.Fanout);

var message = "Lets send this";

var body = Encoding.UTF8.GetBytes(message);

while (true){
    channel.BasicPublish("acceptrejectexchange", "test", null, body);
    Console.WriteLine($"Send message: {message}");

    Console.WriteLine("Press any key to continue");
    Console.ReadKey();
}
