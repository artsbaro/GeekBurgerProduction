using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GeekBurger.Production.Contract
{
    public class Production
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int ProductionId { get; set; }
        public string[] Restrictions { get; set; }
        public bool On { get; set; }
    }
}
