using GeekBurger.Order.Contracts;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GeekBurger.Production.Model
{
    public class NewOrderModel : NewOrder
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
