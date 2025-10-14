using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain
{
    public class RefreshToken
    {
        [BsonElement("token")]
        public string Token { get; set; }

        [BsonElement("expires")]
        public DateTime Expires { get; set; } = DateTime.UtcNow.AddDays(7);

        [BsonElement("revoked")]
        public DateTime? Revoked { get; set; }

        [BsonIgnore]
        public bool IsExpired => DateTime.UtcNow >= Expires;

        [BsonIgnore]
        public bool IsActive => Revoked == null && !IsExpired;
    }
}
