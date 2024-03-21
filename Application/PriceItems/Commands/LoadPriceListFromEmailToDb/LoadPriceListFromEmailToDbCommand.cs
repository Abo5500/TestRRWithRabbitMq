using Application.Interfaces.Email;
using Application.Interfaces.PriceItems;
using Application.Options;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.PriceItems.Commands.LoadPriceListFromEmailToDb
{
    public class LoadPriceListFromEmailToDbCommand : IRequest<bool>
    {
        public string SupplierName { get; set; }
    }
    public class LoadPriceListFromEmailToDbCommandHandler : IRequestHandler<LoadPriceListFromEmailToDbCommand, bool>
    {
        private readonly List<SupplierOptions> _supplierOptions;
        private readonly IEmailContainsFileChecker _emailContainsFileChecker;

        public LoadPriceListFromEmailToDbCommandHandler(IOptions<List<SupplierOptions>> supplierOptions, IEmailContainsFileChecker emailContainsFileChecker)
        {
            _supplierOptions = supplierOptions.Value;
            _emailContainsFileChecker = emailContainsFileChecker;
        }

        public async Task<bool> Handle(LoadPriceListFromEmailToDbCommand request, CancellationToken cancellationToken)
        {
            SupplierOptions supplierOptions = _supplierOptions.First(o => o.Name == request.SupplierName);
            var fileInfo = await _emailContainsFileChecker.GetFileInfoAsync(supplierOptions.Email, ".csv");
            if (fileInfo is null)
            {
                return false;
            }
            fileInfo.SupplierOptions = supplierOptions;
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            string jsonFileInfo = JsonSerializer.Serialize(fileInfo);
            var body = Encoding.UTF8.GetBytes(jsonFileInfo);

            channel.BasicPublish(exchange: "PriceItems",
                     routingKey: "PriceListToLoad",
                     basicProperties: null,
                     body: body);

            return true;
        }
    }
}
