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

        /// <summary>
        /// Get a charging station by its ID.
        /// </summary>
        /// <param name="id">The ID of the charging station to retrieve.</param>
        /// <returns>The charging station with the specified ID.</returns>
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

        /// <summary>
        /// Create a new charging station.
        /// </summary>
        /// <param name="station">The charging station object to create.</param>
        /// <returns>A tuple containing the newly created charging station and a success message.</returns>
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

        /// <summary>
        /// Update an existing charging station.
        /// </summary>
        /// <param name="station">The updated charging station object.</param>
        /// <param name="id">The ID of the charging station to update.</param>
        /// <returns>A success message indicating the update operation was successful.</returns>
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

        /// <summary>
        /// Delete a charging station by its ID.
        /// </summary>
        /// <param name="id">The ID of the charging station to delete.</param>
        /// <returns>A success message indicating the deletion operation was successful.</returns>
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

        /// <summary>
        /// Delete all connectors associated with a charging station by station ID.
        /// </summary>
        /// <param name="id">The ID of the charging station whose connectors should be deleted.</param>
        /// <returns>A success message indicating the deletion operation was successful.</returns>
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
