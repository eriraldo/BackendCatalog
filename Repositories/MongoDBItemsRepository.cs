using System;
using System.Collections.Generic;
using System.Linq;
using Catalog.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Repositories
{

    public class MongoDbItemsRepository : IItemsRepository
    {
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;
        private readonly IMongoCollection<Item> itemsCollection;
        private const string dataBaseName = "catalog";
        private const string collectionName = "items";

        public MongoDbItemsRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(dataBaseName);
            itemsCollection = database.GetCollection<Item>(collectionName);

        }
        public void CreateItem(Item item)
        {
            itemsCollection.InsertOne(item);
        }

        public void DeleteItem(Guid id)
        {
            var filter = filterBuilder.Eq(item=>item.Id, id);
            itemsCollection.DeleteOne(filter);
        }

        public Item GetItem(Guid id)
        {
            var filter = filterBuilder.Eq(item=>item.Id, id);
            return itemsCollection.Find(filter).SingleOrDefault();
        }

        public IEnumerable<Item> GetItems()
        {
            return itemsCollection.Find(new BsonDocument()).ToList();
        }

        public void UpdateItem(Item item)
        {
            var filter = filterBuilder.Eq(existingItem=>existingItem.Id, item.Id);
            itemsCollection.ReplaceOne(filter,item);
        }
    }
}