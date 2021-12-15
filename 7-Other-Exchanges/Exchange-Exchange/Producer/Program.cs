using System;
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory() { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "firstexchange", type: ExchangeType.Direct);

channel.ExchangeDeclare(exchange: "secondexchange", type: ExchangeType.Fanout);

channel.ExchangeBind("secondexchange", "firstexchange", "");

var message = "This message has gone through multiple exchanges";

var body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish("firstexchange", "", null, body);

Console.WriteLine($"Send message: {message}");