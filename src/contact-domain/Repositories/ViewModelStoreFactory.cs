using Autofac.Core;
using Contact.Config;
using Contact.Domain;
using MongoDB.Driver;

namespace Contact.Repositories
{
    public static class ViewModelStoreFactory
    {
        //public static IRepository<T> Create<T>(string connectionString, string storeId) where T : IViewModel
        //{
        //    //var eventStoreConfig = AppConfig.AppSettings.ViewModelStoreConfigs.SingleOrDefault(es => es.StoreId.Equals(storeId));

        //    var mongoDatabase = new MongoClient(connectionString).GetDatabase(storeId);

        //    return new Repository<T>(mongoDatabase);
        //}

        public static IMongoDatabase CreateDatabase(string connectionString, string storeIdd)
        {
            //var eventStoreConfig = AppConfig.AppSettings.ViewModelStoreConfigs.SingleOrDefault(es => es.StoreId.Equals(storeId));

            var mongoDatabase = new MongoClient(connectionString).GetDatabase(storeIdd);

            return mongoDatabase;
        }

        public static Repository<T> Create<T>(string connectionString, string storeId) where T : IViewModel
        {
            //var eventStoreConfig = AppConfig.AppSettings.ViewModelStoreConfigs.SingleOrDefault(es => es.StoreId.Equals(storeId));

            var mongoDatabase = new MongoClient(connectionString).GetDatabase(storeId);

            return new Repository<T>(mongoDatabase);
        }
    }
}

