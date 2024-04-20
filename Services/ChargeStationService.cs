using MongoDB.Driver;
using SmartCharging.Configuration;
using SmartCharging.Models;
using System.Threading.Tasks;

namespace SmartCharging.Services
{
    public class ChargeStationService
    {
        private readonly IMongoCollection<ChargeStation> _chargeStation;
        private readonly IMongoCollection<Connector> _connectors;

        public ChargeStationService(MongoDbContext context)
        {
            _chargeStation = context.ChargeStations;
            _connectors = context.Connectors;
        }

        public async Task<ChargeStation> GetStationById(string id)
        {
            return await _chargeStation.Find(station => station.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ChargeStation> CreateStation(ChargeStation station)
        {
            await _chargeStation.InsertOneAsync(station);
            return station;
        }

        public async Task<ChargeStation> UpdateStation(ChargeStation station, string id)
        {
            await _chargeStation.ReplaceOneAsync(c => c.Id == id, station);
            return station;
        }

        public async Task<string> DeleteStation(string id)
        {
            var filter = Builders<ChargeStation>.Filter.Eq("Id", id);

            await _chargeStation.DeleteOneAsync(filter);
            return id;
        }

        public async Task<string> DeleteConnectorsByGroupId(string id)
        {
            var filter = Builders<Connector>.Filter.Eq("ConnectedStationId", id);

            await _connectors.DeleteManyAsync(filter);
            return id;
        }
    }
}
