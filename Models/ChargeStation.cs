using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace SmartCharging.Models
{
    public class ChargeStation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public  string Id { get; set; }
        public  string Name { get; set; }
        public  string GroupId { get; set; }
        public  List<Connector> Connectors { get; set; }

    }
}
