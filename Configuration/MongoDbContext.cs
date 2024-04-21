using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.Connections;
using SmartCharging.Models;

namespace SmartCharging.Configuration
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        private readonly DatabaseConfiguration _settings;
        public MongoDbContext(IConfiguration configuration, IOptions<DatabaseConfiguration> settings)
        {
            _settings = settings.Value;

            var client = new MongoClient("mongodb://localhost:27017");
            _database = client.GetDatabase("SmartCharging");
            //You can read that from appSettings.json file
        }  //var client = new MongoClient(_settings.ConnectionString);
           //_database = client.GetDatabase(_settings.MONGO_DB_NAME);

        public IMongoCollection<Group> Groups => _database.GetCollection<Group>("Group");
        public IMongoCollection<ChargeStation> ChargeStations => _database.GetCollection<ChargeStation>("ChargeStations");
        public IMongoCollection<Connector> Connectors => _database.GetCollection<Connector>("Connectors");
    }
}
