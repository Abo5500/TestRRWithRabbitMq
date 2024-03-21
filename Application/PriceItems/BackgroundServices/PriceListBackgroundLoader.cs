using Application.Common.Models;
using Application.Interfaces.Email;
using Application.Interfaces.PriceItems;
using Application.Options;
using Application.PriceItems.PriceItemsTaker;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.PriceItems.BackgroundServices
{
    public class PriceListBackgroundLoader : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _serviceProvider;

        public PriceListBackgroundLoader(IServiceProvider serviceProvider)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += Consume;

            _channel.BasicConsume("PriceListToLoad", autoAck: false, consumer);

            return Task.CompletedTask;
        }

        private async void Consume(object? ch, BasicDeliverEventArgs args)
        {
            using var scope = _serviceProvider.CreateAsyncScope();
            var fileLoader = scope.ServiceProvider.GetRequiredService<IEmailFileLoader>();
            var fileInfo = JsonSerializer.Deserialize<EmailFileInfo>(Encoding.UTF8.GetString(args.Body.ToArray()));
            string csvFilePath = $"C:\\Users\\admin\\source\\repos\\TestRRWithRabbitMq\\Application\\CsvFiles\\{fileInfo.SupplierOptions.Name}_{DateTime.Now:MM-dd-yy-HH-mm}.csv";
            await fileLoader.LoadFileAsync(fileInfo, csvFilePath);
            var priceItemTaker = scope.ServiceProvider.GetRequiredService<IPriceItemTaker>();
            int index = 0;
            int chunkSize = 500;
            List<PriceItem> priceItems = await priceItemTaker.GetPriceItemsPartFromScvAsync(csvFilePath, index, chunkSize, fileInfo.SupplierOptions);
            var priceItemRepository = scope.ServiceProvider.GetRequiredService<IPriceItemRepository>();
            while (priceItems.Count != 0)
            {
                await priceItemRepository.AddPriceItemsAsync(priceItems);
                index += chunkSize;

                priceItems = await priceItemTaker.GetPriceItemsPartFromScvAsync(csvFilePath, index, chunkSize, fileInfo.SupplierOptions);
            }

            _channel.BasicAck(args.DeliveryTag, false);
        }
    }
}
