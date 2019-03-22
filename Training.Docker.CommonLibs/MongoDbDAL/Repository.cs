using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using Training.Docker.Models;

namespace Training.Docker.CommonLibs.MongoDbDAL
{
    public class Repository<T> : IRepository<T> where T : IIdentified
    {
        private readonly IMongoDatabase _database = null;
        private IMongoCollection<T> _collection = null;

        public Repository(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
            _collection = _database.GetCollection<T>(collectionName);
        }

        public async Task<IEnumerable<T>> GetAsync()
        {
            var result = await _collection.FindAsync(d => true);
            return await result.ToListAsync();
        }

        public async Task<T> GetAsync(string id)
        {
            var result = await _collection.FindAsync(d => d.Id == id);
            return await result.FirstOrDefaultAsync();
        }

        public async Task<T> CreateAsync(T document)
        {
            await _collection.InsertOneAsync(document);
            return document;
        }

        public async Task UpdateAsync(string id, T document)
        {
            document.Id = id;
            await _collection.ReplaceOneAsync(d => d.Id == id, document);
        }

        public async Task RemoveAsync(string id)
        {
            await _collection.DeleteOneAsync(d => d.Id == id);
        }
    }
}