using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Training.Docker.Models;
using Training.Docker.API.Services;

namespace Training.Docker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerOrdersController : ControllerBase
    {
        private readonly ICustomerOrdersService _orderService;

        public CustomerOrdersController(ICustomerOrdersService orderService) => _orderService = orderService;

        [HttpGet]
        public async Task<IEnumerable<Customer>> Get()
        {
            var result = await _orderService.GetAsync();
            return result;
        }

        [HttpGet("{id}")]
        public Task<Customer> Get(string id)
        {
            return _orderService.GetAsync(id);
        }

        [HttpPost]
        public Task<Customer> Post([FromBody] Customer customerOrder)
        {
            return _orderService.CreateAsync(customerOrder);
        }

        [HttpPut("{id}")]
        public Task Put(string id, [FromBody] Customer customerOrder)
        {
            return _orderService.UpdateAsync(id, customerOrder);
        }

        [HttpDelete("{id}")]
        public Task Delete(string id)
        {
            return _orderService.RemoveAsync(id);
        }
    }
}
