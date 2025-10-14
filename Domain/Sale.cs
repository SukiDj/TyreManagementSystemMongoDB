using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain
{
    public class Sale
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("saleDate")]
        public DateTime SaleDate { get; set; }

        [BsonElement("quantitySold")]
        public int QuantitySold { get; set; }

        [BsonElement("pricePerUnit")]
        public double PricePerUnit { get; set; }

        [BsonElement("unitOfMeasure")]
        public string UnitOfMeasure { get; set; }

        [BsonElement("targetMarket")]
        public string TargetMarket { get; set; }

        [BsonElement("client")]
        public ClientInfo Client { get; set; }

        [BsonElement("tyre")]
        public TyreInfo Tyre { get; set; }

        [BsonElement("productionId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductionId { get; set; }
    }

    public class ClientInfo
    {
        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }
    }
}
