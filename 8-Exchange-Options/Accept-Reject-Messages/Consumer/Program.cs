using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory() { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(
    exchange: "acceptrejectexchange", 
    type: ExchangeType.Fanout);

channel.QueueDeclare(queue: "letterbox");
channel.QueueBind("letterbox", "acceptrejectexchange", "");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    if (ea.DeliveryTag % 5 is 0){
        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: true);
        //channel.BasicNack(deliveryTag: ea.DeliveryTag, requeue: false, multiple: true);
    }

    //channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: false);

    Console.WriteLine($"Main - Recieved new message: {message}");
};
channel.BasicConsume(queue: "letterbox", consumer: consumer);

Console.WriteLine("Consuming");

Console.ReadKey();