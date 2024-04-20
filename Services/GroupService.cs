using MongoDB.Driver;
using MongoDB.Driver.Core.Connections;
using SmartCharging.Configuration;
using SmartCharging.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartCharging.Services
{

    public class GroupService
    {
        private readonly IMongoCollection<Group> _groups;
        private readonly IMongoCollection<ChargeStation> _chargeStations;

        public GroupService(MongoDbContext context)
        {
            _groups = context.Groups;
            _chargeStations = context.ChargeStations;
        }

        public async Task<Group> GetGroupById(string id)
        {
            return await _groups.Find(group => group.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Group> CreateGroup(Group group)
        {
            await _groups.InsertOneAsync(group);
            return group;
        }

        public async Task<Group> UpdateGroup(Group group, string id)
        {
            await _groups.ReplaceOneAsync(c => c.Id == id, group);
            return group;
        }

        public async Task<string> DeleteGroup(string id)
        {
            var filter = Builders<Group>.Filter.Eq("Id", id);

            await _groups.DeleteOneAsync(filter);
            return id;
        }

        public async Task<string> DeleteStationsByGroupId(string id)
        {
            var filter = Builders<ChargeStation>.Filter.Eq("GroupId", id);

            await _chargeStations.DeleteManyAsync(filter);
            return id;
        }

        public async Task<string> DeleteStationsByStationId(string id)
        {
            var filter = Builders<ChargeStation>.Filter.Eq("GroupId", id);

            await _chargeStations.DeleteManyAsync(filter);
            return id;
        }

        public async Task<bool> CheckChargeStation(string id)
        {
            // Construct the filter to check if any group contains the specified charge station ID
            var filter = Builders<Group>.Filter.AnyIn(g => g.IdsofChargeStations, new List<string> { id });

            // Execute the filter against the MongoDB collection
            var groupWithChargeStation = await _groups.Find(filter).ToListAsync();

            // If any group contains the charge station ID, return true; otherwise, return false
            return groupWithChargeStation.Count > 0;
        }

    }
}
