using MongoDB.Bson.Serialization.Attributes;

namespace Domain
{
    public class GeoPoint
    {
        [BsonElement("type")]
        public string Type { get; set; } = "Point";

        [BsonElement("coordinates")]
        public double[] Coordinates { get; set; } = default!;
    }
}
