using MongoDB.Driver;
using MongoDB.Driver.Core.Connections;
using SmartCharging.Configuration;
using SmartCharging.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartCharging.Services
{

    public class GroupService
    {
        /// <summary>
        /// The collections used in the service from Mongodb are read here.
        /// </summary>
        private readonly IMongoCollection<Group> _groups;
        private readonly IMongoCollection<ChargeStation> _chargeStations;
        private readonly IMongoCollection<Connector> _connectors;

        public GroupService(MongoDbContext context)
        {
            _groups = context.Groups;
            _chargeStations = context.ChargeStations;
            _connectors = context.Connectors;
        }

        /// <summary>
        /// Get a group by its ID.
        /// </summary>
        /// <param name="id">The ID of the group to retrieve.</param>
        /// <returns>The group with the specified ID.</returns>
        public async Task<Group> GetGroupById(string id)
        {
            try
            {
                return await _groups.Find(group => group.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Create a new group.
        /// </summary>
        /// <param name="group">The group object to create.</param>
        /// <returns>A tuple containing the newly created group and a success message.</returns>
        public async Task<(Group,string)> CreateGroup(Group group)
        {
            try
            {
                await _groups.InsertOneAsync(group);
                return (group, "Group created");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Update an existing group.
        /// </summary>
        /// <param name="group">The updated group object.</param>
        /// <param name="id">The ID of the group to update.</param>
        /// <returns>A success message indicating the update operation was successful.</returns>
        public async Task<string> UpdateGroup(Group group, string id)
        {
            try
            {
                await _groups.ReplaceOneAsync(c => c.Id == id, group);
                return "Group Updated";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete a group by its ID.
        /// </summary>
        /// <param name="id">The ID of the group to delete.</param>
        /// <returns>A success message indicating the deletion operation was successful.</returns>
        public async Task<string> DeleteGroup(string id)
        {
            try
            {
                var filter = Builders<Group>.Filter.Eq("Id", id);
                await _groups.DeleteOneAsync(filter);
                return "Group has been deleted.";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete all charge stations associated with a group by group ID.
        /// When the charging station is to be deleted, the control for deleting the connectors connected to it is done here. 
        /// </summary>
        /// <param name="id">The ID of the group whose charge stations should be deleted.</param>
        /// <returns>A success message indicating the deletion operation was successful.</returns>
        public async Task<string> DeleteStationsByGroupId(string id)
        {
            try
            {
                var filter = Builders<ChargeStation>.Filter.Eq("GroupId", id);
                var stationList = await _chargeStations.Find(data => data.GroupId == id).ToListAsync();
                foreach (var station in stationList)
                {
                    foreach (var connector in station.Connectors)
                    {
                        var filterConn = Builders<Connector>.Filter.Eq("Id", connector.Id);
                        await _connectors.DeleteOneAsync(filterConn);
                    }
                }
                await _chargeStations.DeleteManyAsync(filter);
                return "Relevant charge stations were deleted according to group id.";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Check if a charge station exists within any group.
        /// </summary>
        /// <param name="id">The ID of the charge station to check.</param>
        /// <returns>True if the charge station exists within any group; otherwise, false.</returns>
        public async Task<bool> CheckChargeStation(string id)
        {
            try
            {
                var filter = Builders<Group>.Filter.AnyIn(g => g.IdsofChargeStations, new List<string> { id });
                var groupWithChargeStation = await _groups.Find(filter).ToListAsync();
                return groupWithChargeStation.Count > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
