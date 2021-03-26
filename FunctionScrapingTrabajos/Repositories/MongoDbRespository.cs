using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionScrapingTrabajos.Repositories
{
    public class MongoDbRespository
    {
        public MongoClient client;
        public IMongoDatabase db;
        public MongoDbRespository()
        {
            //client = new MongoClient("mongodb://127.0.0.1:27017");            
            client = new MongoClient(SecretKey.ConnectionString);
            db = client.GetDatabase("JobsScraper");
        }
    }
}
