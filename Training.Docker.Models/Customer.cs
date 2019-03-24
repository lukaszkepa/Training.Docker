using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Training.Docker.Models
{
    public class Customer : IIdentified
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("City")]
        public string City { get; set; }

        [BsonElement("PostalCode")]
        public string PostalCode { get; set; }

        [BsonElement("StreetAndNumber")]
        public string StreetAndNumber { get; set; }

        [BsonElement("Order")]
        public Order Order { get; set; }
    }
}
