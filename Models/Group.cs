using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Collections.Generic;
using System;

namespace SmartCharging.Models
{
    public class Group
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public  string Id { get; set; }
        public  string Name { get; set; }
        public  int CapacityInAmps { get; set; }
        public string[] IdsofChargeStations { get; set; }

    }
}
