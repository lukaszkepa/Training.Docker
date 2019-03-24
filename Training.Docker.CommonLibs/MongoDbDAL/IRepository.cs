using System.Collections.Generic;
using System.Threading.Tasks;
using Training.Docker.Models;

namespace Training.Docker.CommonLibs.MongoDbDAL
{
    public interface IRepository<T> where T : IIdentified
    {
        Task<IEnumerable<T>> GetAsync();
        Task<T> GetAsync(string id);
        Task<T> CreateAsync(T document);
        Task UpdateAsync(string id, T document);
        Task RemoveAsync(string id);
    }
}
