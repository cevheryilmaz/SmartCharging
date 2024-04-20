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
            //var connectionString = configuration.GetConnectionString(_settings.ConnectionString);
            //var databaseName = configuration.GetConnectionString(_settings.MONGO_DB_NAME);

            var client = new MongoClient(_settings.ConnectionString);
            _database = client.GetDatabase(_settings.MONGO_DB_NAME);
        }

        public IMongoCollection<Group> Groups => _database.GetCollection<Group>("Group");
        public IMongoCollection<ChargeStation> ChargeStations => _database.GetCollection<ChargeStation>("ChargeStations");
        public IMongoCollection<Connector> Connectors => _database.GetCollection<Connector>("Connectors");
    }
}
