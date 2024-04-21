using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.Connections;
using SmartCharging.Models;

namespace SmartCharging.Configuration
{
  
    /// <summary>
    /// Represents a MongoDB database context.
    /// </summary>
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        private readonly DatabaseConfiguration _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDbContext"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="settings">The database configuration settings.</param>
        public MongoDbContext(IConfiguration configuration, IOptions<DatabaseConfiguration> settings)
        {
            _settings = settings.Value;

            // For demo purposes, connecting to localhost. You may consider reading connection string from appSettings.json.
            var client = new MongoClient("mongodb://localhost:27017");
            _database = client.GetDatabase("SmartCharging");
        }

        public IMongoCollection<Group> Groups => _database.GetCollection<Group>("Group");
        public IMongoCollection<ChargeStation> ChargeStations => _database.GetCollection<ChargeStation>("ChargeStations");
        public IMongoCollection<Connector> Connectors => _database.GetCollection<Connector>("Connectors");
    }

}
