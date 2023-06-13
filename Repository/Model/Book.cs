using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model
{
    [BsonIgnoreExtraElements]
    public class Book
    {
        [BsonId]
        public Guid Book_ID { get; set; }
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonElement("Author")]
        public string Author { get; set; }
        [BsonElement("Price")]
        public double Price { get; set; }
    }
}
