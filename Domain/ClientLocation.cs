using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain
{
    public class ClientLocation
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("clientId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ClientId { get; set; }

        [BsonElement("location")]
        public GeoPoint Location { get; set; } = default!;
    }
}