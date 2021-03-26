using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionScrapingTrabajos.Models
{
    public class Publication
    {
        //[BsonId]

        [BsonElement("url")]
        public string Url { get; set; }
        [BsonElement("_id")]
        public int Id { get; set; }
        [BsonElement("title")]
        public string Title { get; set; }
        [BsonElement("date")]
        public DateTime Date { get; set; }
        [BsonElement("description")]
        public string Description { get; set; }
        [BsonElement("address")]
        public string Address { get; set; }
        [BsonElement("phoneNumber")]
        public string PhoneNumber { get; set; }
        [BsonElement("company")]
        public string Company { get; set; }
        [BsonElement("dateScraper")]
        public DateTime DateScraper { get; set; }

        public Publication() { }
    }
}
