namespace Contact.Repositories
{
    using Contact.Domain;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public class Repository<T> : IRepository<T> where T : IViewModel
    {
        private readonly IMongoDatabase mongoDatabase;

        public Repository(IMongoDatabase mongoDatabase)
        {
            this.mongoDatabase = mongoDatabase;
        }

        public Task<T> Single(FilterDefinition<T> expression)
        {
            return Collection.Find(expression).FirstOrDefaultAsync();
        }


        public IEnumerable<Model> GetByRegulerExpression<Model>(string fieldName, string value)
        {
            var collection = GetCollectionOfBsonDocument();

            var expression = $"\\b{value}";

            var filter = Builders<BsonDocument>.Filter.Regex(fieldName, new BsonRegularExpression(expression, "i"));

            var modelProperties = typeof(Model).GetProperties().Select(p => new KeyValuePair<string, object>(p.Name, p.Name));

            var projectDefination = new BsonDocument().AddRange(modelProperties);

            var cursor = collection.Find(filter).Project(projectDefination).ToCursor();

            while (cursor.MoveNext())
            {
                foreach (var item in cursor.Current)
                {
                    yield return BsonSerializer.Deserialize<Model>(item); ;
                }
            }

        }

        public IMongoCollection<T> Collection
        {
            get
            {
                var collectionNamespace = string.Format("{0}s", typeof(T).Name);
                return mongoDatabase.GetCollection<T>(collectionNamespace);
            }
        }

        public Task AddAsync(T item)
        {
            return GetCollection().InsertOneAsync(item);
        }

        public Task AddRangeAsync(IEnumerable<T> items)
        {
            if (items.Any() == false)
            {
                throw new Exception("Items cannot be empty");
            }

            return GetCollection().InsertManyAsync(items);
        }





        public Task ReplaceOneAsync(string itemId, T document)
        {
            var filter = Builders<T>.Filter.Eq("_id", itemId);



            return GetCollection().ReplaceOneAsync(filter, document); ;
        }


        public Task UpdateOneByIdAsync(string itemId, IEnumerable<Update> updates)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", itemId);

            var updateOperations = updates.Select(u => new BsonElement(u.UpdateOperation, new BsonDocument { { u.FieldName, BsonValue.Create(u.Value) } }));

            var update = new BsonDocument(true).AddRange(updateOperations);

            return GetBsonDocumentCollection().UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = false, BypassDocumentValidation = true });
        }

        public Task UpdateOneAsync(IEnumerable<Filter> filters, IEnumerable<Update> updates)
        {
            var filter = CreateFilter(filters);

            var updateOperations = updates.Select(u => new BsonElement(u.UpdateOperation, new BsonDocument { { u.FieldName, BsonValue.Create(u.Value) } }));

            var update = new BsonDocument(true).AddRange(updateOperations);

            return GetBsonDocumentCollection().UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = false, BypassDocumentValidation = true });
        }

        public Task UpdateManyByIdAsync(string itemId, IEnumerable<Update> updates)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", itemId);

            var updateOperations = updates.Select(u => new BsonElement(u.UpdateOperation, new BsonDocument { { u.FieldName, BsonValue.Create(u.Value) } }));

            var update = new BsonDocument(true).AddRange(updateOperations);

            return GetBsonDocumentCollection().UpdateManyAsync(filter, update, new UpdateOptions { IsUpsert = false, BypassDocumentValidation = true });
        }

        public Task UpdateManyAsync(IEnumerable<Filter> filters, IEnumerable<Update> updates)
        {
            var filter = CreateFilter(filters);

            var updateOperations = updates.Select(u => new BsonElement(u.UpdateOperation, new BsonDocument { { u.FieldName, BsonValue.Create(u.Value) } }));

            var update = new BsonDocument(true).AddRange(updateOperations);

            return GetBsonDocumentCollection().UpdateManyAsync(filter, update, new UpdateOptions { IsUpsert = false, BypassDocumentValidation = true });
        }

        private static BsonDocument CreateFilter(IEnumerable<Filter> filters)
        {
            var filterElements = new List<BsonElement>(filters.Count());

            foreach (var filter in filters)
            {
                if (filter.FilterOperator == FilterOperators.Equal)
                {
                    var filterElement = new BsonElement(filter.FieldName, BsonValue.Create(filter.Value));
                    filterElements.Add(filterElement);
                }
            }
            var filterDocument = new BsonDocument(true).AddRange(filterElements);

            return filterDocument;
        }

        private IMongoCollection<BsonDocument> GetBsonDocumentCollection()
        {
            var collectionNamespace = string.Format("{0}s", typeof(T).Name);
            var collection = mongoDatabase.GetCollection<BsonDocument>(collectionNamespace);
            return collection;
        }

        private IMongoCollection<T> GetCollection()
        {
            var CollectionNamespace = string.Format("{0}s", typeof(T).Name);
            return mongoDatabase.GetCollection<T>(CollectionNamespace);
        }

        private IMongoCollection<BsonDocument> GetCollectionOfBsonDocument()
        {
            var CollectionNamespace = string.Format("{0}s", typeof(T).Name);
            return mongoDatabase.GetCollection<BsonDocument>(CollectionNamespace);
        }

        public Task DeleteAsync(T entity, string itemId)
        {
            FieldDefinition<T, string> field = "_id";
            var filterDefination = Builders<T>.Filter.Eq(field, itemId);
            return GetCollection().DeleteOneAsync(filterDefination);
        }

        public bool Any(Expression<Func<T, bool>> expression)
        {
            return Collection.AsQueryable().Any(expression);
        }

        public int Count(Expression<Func<T, bool>> expression)
        {
            return Collection.AsQueryable().Count(expression);
        }

        public IEnumerable<T> TextSearch(string text, int skipped, int pageSize)
        {
            var collection = GetCollection();

            var textFilter = Builders<T>.Filter.Text(text);

            var findFluent = collection.Find(textFilter);

            var cursor = findFluent.Skip(skipped).Limit(pageSize).ToCursor();

            while (cursor.MoveNext())
            {
                foreach (var item in cursor.Current)
                {
                    yield return item;
                }
            }
        }

        public long TextSearchCount(string text)
        {
            var collection = GetCollection();

            var textFilter = Builders<T>.Filter.Text(text);

            return collection.Count(textFilter);
        }

        public IEnumerable<T> GetPaged(Expression<Func<T, bool>> filter, int skip, int take)
        {
            var collection = GetCollection();

            var findFluent = collection.Find(filter).Skip(skip).Limit(take);

            var cursor = findFluent.ToCursor();

            while (cursor.MoveNext())
            {
                foreach (var item in cursor.Current)
                {
                    yield return item;
                }
            }
        }

        public IEnumerable<T> GetPagedDescending(Expression<Func<T, bool>> filter, int skip, int take, Expression<Func<T, object>> orderBy)
        {
            var collection = GetCollection();

            var findFluent = collection.Find(filter).SortByDescending(orderBy).Skip(skip).Limit(take);

            var cursor = findFluent.ToCursor();

            while (cursor.MoveNext())
            {
                foreach (var item in cursor.Current)
                {
                    yield return item;
                }
            }
        }

        public IEnumerable<TField> GetDistinct<TField>(Expression<Func<T, TField>> field, Expression<Func<T, bool>> filter)
        {
            var collection = GetCollection();

            return collection.Distinct(field, filter).ToEnumerable();
        }

        public IEnumerable<M3> GetRatingAverage<M1, M2, M3>(string reviewrId, DateTime fromDate, DateTime toDate) where M1 : M3 where M2 : M3
        {


            var myFilter = Builders<BsonDocument>.Filter.Eq("ReviwerId", reviewrId) & Builders<BsonDocument>.Filter.Gte("CreatedOn", fromDate) & Builders<BsonDocument>.Filter.Lte("CreatedOn", toDate);

            var aggregate = GetBsonDocumentCollection().Aggregate()
                                       .Match(myFilter)
                                       .Group(new BsonDocument
                                       {
                                           { "_id", "$DateCreated" },
                                           { "a" ,    new BsonDocument("$avg", "$Rating")},
                                           { "c", new BsonDocument("$sum", 1) }
                                       });

            var cursor = aggregate.ToCursor();

            while (cursor.MoveNext())
            {
                foreach (var item in cursor.Current)
                {
                    yield return BsonSerializer.Deserialize<M1>(item); ;
                }
            }

            var othersFilter = Builders<BsonDocument>.Filter.Ne("ReviwerId", reviewrId) & Builders<BsonDocument>.Filter.Gte("CreatedOn", fromDate) & Builders<BsonDocument>.Filter.Lte("CreatedOn", toDate);


            aggregate = GetBsonDocumentCollection().Aggregate()
                                      .Match(othersFilter)
                                      .Group(new BsonDocument
                                      {
                                           { "_id", "$DateCreated" },
                                           { "a" ,    new BsonDocument("$avg", "$Rating")},
                                           { "c", new BsonDocument("$sum", 1) }
                                      });

            cursor = aggregate.ToCursor();

            while (cursor.MoveNext())
            {
                foreach (var item in cursor.Current)
                {
                    yield return BsonSerializer.Deserialize<M2>(item); ;
                }
            }
        }

        public IEnumerable<M3> GetRatingAverageMonthly<M1, M2, M3>(string reviewrId, DateTime fromDate, DateTime toDate) where M1 : M3 where M2 : M3
        {
            var myFilter = Builders<BsonDocument>.Filter.Eq("ReviwerId", reviewrId) & Builders<BsonDocument>.Filter.Gte("CreatedOn", fromDate) & Builders<BsonDocument>.Filter.Lte("CreatedOn", toDate);

            var aggregate = GetBsonDocumentCollection().Aggregate()
                                       .Match(myFilter)
                                       .Group(new BsonDocument
                                       {
                                           { "_id", "$DateCreated" },
                                           { "a" ,  new BsonDocument("$avg", "$Rating")},
                                           { "c",   new BsonDocument("$sum", 1) }
                                       });

            var cursor = aggregate.ToCursor();

            while (cursor.MoveNext())
            {
                foreach (var item in cursor.Current)
                {
                    yield return BsonSerializer.Deserialize<M1>(item); ;
                }
            }

            var othersFilter = Builders<BsonDocument>.Filter.Ne("ReviwerId", reviewrId) & Builders<BsonDocument>.Filter.Gte("CreatedOn", fromDate) & Builders<BsonDocument>.Filter.Lte("CreatedOn", toDate);


            aggregate = GetBsonDocumentCollection().Aggregate()
                                      .Match(othersFilter)
                                      .Group(new BsonDocument
                                      {
                                           { "_id", "$DateCreated" },
                                           { "a" ,    new BsonDocument("$avg", "$Rating")},
                                           { "c", new BsonDocument("$sum", 1) }
                                      });

            cursor = aggregate.ToCursor();

            while (cursor.MoveNext())
            {
                foreach (var item in cursor.Current)
                {
                    yield return BsonSerializer.Deserialize<M2>(item); ;
                }
            }
        }
    }
}
