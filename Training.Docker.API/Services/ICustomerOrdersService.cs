using System.Collections.Generic;
using System.Threading.Tasks;
using Training.Docker.Models;

namespace Training.Docker.API.Services
{
    public interface ICustomerOrdersService
    {
        Task<IEnumerable<Customer>> GetAsync();
        Task<Customer> GetAsync(string id);
        Task<Customer> CreateAsync(Customer customerOrder);
        Task UpdateAsync(string id, Customer customerOrder);
        Task RemoveAsync(string id);
    }
}
