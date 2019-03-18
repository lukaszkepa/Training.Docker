using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;

namespace Training.Docker.CommonLibs.MongoDbDAL
{
    public class Repository
    {
        private string _db = String.Empty;
        private string _collection = String.Empty;
        private MongoClient _mongoClient = null;
        public Repository(string host, string port, string db, string collection)
        {
            this._db = db;
            this._collection = collection;
            string url = String.Format("mongodb://{0}:{1}", host, port);
            this._mongoClient = new MongoClient(url);

        }

        public async Task<JObject> GetJsonObjectById(string jsonObjectId)
        {
            JObject result = null;
            if (this._mongoClient != null)
            {
                var db = this._mongoClient.GetDatabase(this._db);
                if (db != null)
                {
                    var collection = db.GetCollection<BsonDocument>(this._collection);
                    if (collection != null)
                    {
                        var builder = Builders<BsonDocument>.Filter;
                        var filter = builder.Eq("_id", ObjectId.Parse(jsonObjectId));
                        IAsyncCursor<BsonDocument> cursor = await collection.FindAsync<BsonDocument>(filter);
                        if (await cursor.MoveNextAsync())
                        {
                            BsonDocument bsonDocument = cursor.Current.ElementAt(0);
                            if (bsonDocument != null)
                            {
                                string json = bsonDocument.ToJson(new JsonWriterSettings() { OutputMode = JsonOutputMode.Strict });
                                result = JObject.Parse(json);
                            }
                        }
                    }
                }
            }

            return result;
        }

        public async Task SaveJsonObject(JObject jObject)
        {
            if (this._mongoClient != null)
            {
                var db = this._mongoClient.GetDatabase(this._db);
                if (db != null)
                {
                    var collection = db.GetCollection<BsonDocument>(this._collection);
                    if (collection != null)
                    {
                        string json = jObject.ToString();
                        BsonDocument bsonDocument = BsonDocument.Parse(json);
                        await collection.InsertOneAsync(bsonDocument);
                    }
                }
            }
        }        
    }
}