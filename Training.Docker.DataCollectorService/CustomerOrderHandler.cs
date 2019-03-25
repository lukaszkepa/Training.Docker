using Microsoft.Extensions.Logging;
using Rebus.Handlers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Training.Docker.Models;

namespace Training.Docker.DataCollectorService
{
    public class CustomerOrderHandler : IHandleMessages<CustomerOrderAdded>
    {
        private readonly ILogger<CustomerOrderHandler> _logger;

        public CustomerOrderHandler(ILogger<CustomerOrderHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(CustomerOrderAdded message)
        {
            _logger.LogInformation("Hanlding customer order adder with id '{orderId}'", message.CustomerOrderId);
            return Task.CompletedTask;
        }
    }
}
