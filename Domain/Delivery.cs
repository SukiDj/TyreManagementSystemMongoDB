using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain
{
    public class Delivery
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("client")]
        public string Client { get; set; } = default!;

        [BsonElement("origin")]
        public GeoPoint Origin { get; set; } = default!;

        [BsonElement("destination")]
        public GeoPoint Destination { get; set; } = default!;

        [BsonElement("distanceMeters")]
        public double DistanceMeters { get; set; }

        [BsonElement("status")]
        public string Status { get; set; } = "In Progress"; 
    }

    
}
