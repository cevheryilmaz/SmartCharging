using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SmartCharging.Models
{
    public class Connector
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } 
        public  int? MaxCurrentInAmps { get; set; }
        public string? ConnectedStationId { get; set; }
        
    }
}
