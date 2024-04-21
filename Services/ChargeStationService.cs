using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Driver;
using SmartCharging.Configuration;
using SmartCharging.Models;
using System;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            try
            {
                return await _chargeStation.Find(station => station.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<(ChargeStation, string)> CreateStation(ChargeStation station)
        {
            try
            {
                await _chargeStation.InsertOneAsync(station);
                return (station, "Charging Station was created.");
                }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> UpdateStation(ChargeStation station, string id)
        {
            try
            {
                await _chargeStation.ReplaceOneAsync(c => c.Id == id, station);
                return "Charge Station Updated";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> DeleteStation(string id)
        {
            try
            {
                var filter = Builders<ChargeStation>.Filter.Eq("Id", id);
                await _chargeStation.DeleteOneAsync(filter);
                return "Charge Station Deleted";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> DeleteConnectorsByStationId(string id)
        {
            try
            {
                var filter = Builders<Connector>.Filter.Eq("ConnectedStationId", id);
                await _connectors.DeleteManyAsync(filter);
                return "Connector Deleted By Station Id";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
