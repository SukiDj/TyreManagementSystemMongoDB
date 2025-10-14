using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("username")]
        public string Username { get; set; }

        [BsonElement("passwordHash")]
        public string PasswordHash { get; set; }

        [BsonElement("ime")]
        public string Ime { get; set; }

        [BsonElement("prezime")]
        public string Prezime { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("telefon")]
        public string Telefon { get; set; }

        [BsonElement("datumRodjenja")]
        public DateTime DatumRodjenja { get; set; }

        [BsonElement("role")]
        public string Role { get; set; } // "Operator", "Supervisor", "Leader"

        [BsonElement("refreshTokens")]
        public List<RefreshToken> RefreshTokens { get; set; } = new();
    }
}
