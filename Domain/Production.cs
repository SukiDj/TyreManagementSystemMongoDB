using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain
{
    public class Production
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("shift")]
        public int Shift { get; set; }

        [BsonElement("quantityProduced")]
        public int QuantityProduced { get; set; }

        [BsonElement("productionDate")]
        public DateTime ProductionDate { get; set; }

        [BsonElement("tyre")]
        public TyreInfo Tyre { get; set; }

        [BsonElement("operator")]
        public UserInfo Operator { get; set; }

        [BsonElement("machine")]
        public MachineInfo Machine { get; set; }

        [BsonElement("sales")]
        public List<SaleInfo> Sales { get; set; } = new();
    }

    public class TyreInfo
    {
        [BsonElement("code")]
        public string Code { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }
    }

    public class UserInfo
    {
        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }
    }

    public class MachineInfo
    {
        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }
    }

    public class SaleInfo
    {
        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("quantity")]
        public int Quantity { get; set; }

        [BsonElement("price")]
        public double Price { get; set; }
    }
}
