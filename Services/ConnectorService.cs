using MongoDB.Driver;
using SmartCharging.Configuration;
using SmartCharging.Models;
using System;
using System.Threading.Tasks;

namespace SmartCharging.Services
{
    public class ConnectorService
    {
        private readonly IMongoCollection<Group> _groups;
        private readonly IMongoCollection<ChargeStation> _chargeStations;
        private readonly IMongoCollection<Connector> _connectors;
        public ConnectorService(MongoDbContext context)
        {
            _groups = context.Groups;
            _chargeStations = context.ChargeStations;
            _connectors = context.Connectors;
        }

        public async Task<Connector> GetConnectorById(string id)
        {
            try
            {
                return await _connectors.Find(connector => connector.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<(Connector,string)> CreateConnector(Connector connector)
        {
            try
            {
                await _connectors.InsertOneAsync(connector);
                return (connector, "Connector was added and the necessary update was made to the charging station.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<(string, bool)> UpdateConnector(Connector connector, string id)
        {
            try
            {
                ChargeStation station = await _chargeStations.Find(station => station.Id == connector.ConnectedStationId).FirstOrDefaultAsync();
                Group group = await _groups.Find(group => group.Id == station.GroupId).FirstOrDefaultAsync();
                int capacityInAmps = group.CapacityInAmps;
                int usedAmpsInStation = 0;
                foreach (var connItem in station.Connectors)
                {
                    if (connItem.Id != id)
                    {
                        usedAmpsInStation += (int)connItem.MaxCurrentInAmps;
                    }
                }

                string message;
                bool status = true;
                if (capacityInAmps >= (usedAmpsInStation + connector.MaxCurrentInAmps))
                {
                    await _connectors.ReplaceOneAsync(c => c.Id == id, connector);
                    message = "Connector updated successfully.";
                }
                else
                {
                    message = "Request rejected! Updating connector would exceed station capacity.";
                    status = false;
                }

                return (message, status);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> DeleteConnector(string id)
        {
            try
            {
                var filter = Builders<Connector>.Filter.Eq("Id", id);
                await _connectors.DeleteOneAsync(filter);
                return "Connector Deleted";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
