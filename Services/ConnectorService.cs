using MongoDB.Driver;
using SmartCharging.Configuration;
using SmartCharging.Models;
using System.Threading.Tasks;

namespace SmartCharging.Services
{
    public class ConnectorService
    {
        private readonly IMongoCollection<Connector> _connectors;

        public ConnectorService(MongoDbContext context)
        {
            _connectors = context.Connectors;
        }

        public async Task<Connector> GetConnectorById(string id)
        {
            return await _connectors.Find(connector => connector.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Connector> CreateConnector(Connector connector)
        {
            await _connectors.InsertOneAsync(connector);
            return connector;
        }

        public async Task<Connector> UpdateConnector(Connector connector, string id)
        {
            await _connectors.ReplaceOneAsync(c => c.Id == id, connector);
            return connector;
        }

        public async Task<string> DeleteConnector(string id)
        {
            var filter = Builders<Connector>.Filter.Eq("Id", id);

            await _connectors.DeleteOneAsync(filter);
            return id;
        }
    }
}
