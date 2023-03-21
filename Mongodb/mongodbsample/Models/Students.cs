using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace mongodbsample.Models
{
    [BsonIgnoreExtraElements]
    public class Students
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
        [BsonElement("graduated")]
        public bool IsGraduated { get; set; }
        [BsonElement("cources")]
        public string[]  Cources { get; set; }
        [BsonElement("gender")] 
        public string Gender { get; set; } = string.Empty;
        [BsonElement("age")] 
        public int Age { get; set; }
        public Students() { }
            
    }
}
