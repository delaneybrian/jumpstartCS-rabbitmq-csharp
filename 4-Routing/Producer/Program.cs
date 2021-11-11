using System;
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory() { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "routing", type: ExchangeType.Direct);

var message = "This message needs to be routed";

var body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish(exchange: "routing", routingKey: "analyticsonly", null, body);

Console.WriteLine($"Send message: {message}");