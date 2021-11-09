using System;
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory() { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "pubsub", type: ExchangeType.Fanout);

var message = "Hello I want to broadcast this message";

var body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish(exchange: "pubsub", "", null, body);

Console.WriteLine($"Send message: {message}");