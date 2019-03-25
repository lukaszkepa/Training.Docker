using Microsoft.Extensions.Options;
using Rebus.Bus;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Training.Docker.API.Settings;
using Training.Docker.CommonLibs.MongoDbDAL;
using Training.Docker.Models;

namespace Training.Docker.API.Services
{
    public class CustomerOrdersService : ICustomerOrdersService
    {
        private readonly IBus _bus;
        private readonly IRepository<Customer> _repository;

        public CustomerOrdersService(IBus bus, IOptions<MongoDbSettings> optionsAccessor)
        {
            _bus = bus;
            _repository = new Repository<Customer>(optionsAccessor.Value.ConnectionString,
                optionsAccessor.Value.DatabaseName, Collections.Orders);
        }

        public async Task<Customer> CreateAsync(Customer customerOrder)
        {
            var result = await _repository.CreateAsync(customerOrder);

            await _bus.Publish(new CustomerOrderAdded
            {
                RequestId = Guid.NewGuid(),
                CustomerOrderId = result.Id,
            });
            return result;
        }

        public Task<IEnumerable<Customer>> GetAsync()
        {
            return _repository.GetAsync();
        }

        public Task<Customer> GetAsync(string id)
        {
            return _repository.GetAsync(id);
        }

        public Task RemoveAsync(string id)
        {
            return _repository.RemoveAsync(id);
        }

        public Task UpdateAsync(string id, Customer customerOrder)
        {
            return _repository.UpdateAsync(id, customerOrder);
        }
    }
}
