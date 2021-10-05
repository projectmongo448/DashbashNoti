using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Server
{
    public class Mech
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("machineId")]
        public string machineId { get; set; }
        [BsonElement("datetime")]
        public string datetime { get; set; }
        [BsonElement("voltmean")]
        public string voltmean { get; set; }
        [BsonElement("rotatemean")]
        public string rotatemean { get; set; }
        [BsonElement("pressuremean")]
        public string pressuremean { get; set; }
        [BsonElement("vibrationmean")]
        public string vibrationmean { get; set; }
    }
}
