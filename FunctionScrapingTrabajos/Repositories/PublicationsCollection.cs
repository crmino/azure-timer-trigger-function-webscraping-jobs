using FunctionScrapingTrabajos.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FunctionScrapingTrabajos.Repositories
{
    public class PublicationsCollection : IPublicationsCollection
    {
        internal MongoDbRespository _repository = new MongoDbRespository();
        private readonly IMongoCollection<Publication> collection;

        public PublicationsCollection()
        {
            collection = _repository.db.GetCollection<Publication>("Publications");
        }
        public async Task InsertPublication(Publication publication)
        {
            try
            {
                await collection.InsertOneAsync(publication);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task InsertPublications(List<Publication> publicationList)
        {
            try
            {
                await collection.InsertManyAsync(publicationList);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<int> GetLastPublication()
        {
            int lasId = 0;
            try
            {
                var lastPublication = await collection.Find(new BsonDocument()).SortByDescending(p => p.Id).Limit(1).FirstAsync();
                lasId = lastPublication.Id;
                return lasId;
            }
            catch (Exception)
            {
                return lasId;
                throw;
            }           
        }
    }

}
