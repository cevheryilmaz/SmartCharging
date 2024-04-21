using Microsoft.AspNetCore.Mvc;
using SmartCharging.Models;
using SmartCharging.Services;
using System;
using System.Threading.Tasks;

namespace SmartCharging.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : Controller
    {

        private GroupService _groupService;
        private ChargeStationService _chargeStationService;
        public GroupController(GroupService groupService, ChargeStationService chargeStationService)
        {
            _groupService = groupService;
            _chargeStationService = chargeStationService;
        }

        /// <summary>
        /// Get group by ID
        /// </summary>
        /// <param name="id">Group ID</param>
        [HttpGet("get-group/{id:length(24)}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                return Ok(await _groupService.GetGroupById(id));
            }
            catch (Exception ex)
            {

                return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
            }

        }

        /// <summary>
        /// Create a new group in to the Group table.
        /// </summary>
        /// <param name="group">Group object (Id, Name, CapacityInAmps, IdsofChargeStations)</param>
        [HttpPost("create-group")]
        public async Task<IActionResult> Create(Group group)
        {
            try
            {
                if (group.CapacityInAmps >= 0) 
                {
                    (Group newGroup, string serviceMessage) = await _groupService.CreateGroup(group);
                    return Ok(new { data = newGroup, success = true, message = serviceMessage });
                }
                else
                {
                    return Ok(new { success = false, message = "Request Rejected! The capacity in amperes cannot be less than 0." });
                }
                
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
            }
        }

        /// <summary>
        /// Update group by ID
        /// </summary>
        /// <param name="group">Group object</param>
        /// <param name="id">Group ID</param>
        [HttpPut("update-group/{id:length(24)}")]
        public async Task<IActionResult> Update(Group group, string id)
        {
            try
            {
                string serviceMessage = await _groupService.UpdateGroup(group,id);
                return Ok(new { success = true, message = serviceMessage });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
            }
        }


        /// <summary>
        /// Delete group by ID
        /// </summary>
        /// <param name="id">Group ID</param>
        [HttpDelete("delete-group/{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                //delete all included charge station and group according to id. 
                //If a group is deleted, the charging stations and connectors connected to it will be deleted.
                await _groupService.DeleteStationsByGroupId(id);
                string serviceMessage =  await _groupService.DeleteGroup(id);
               
                return Ok(new { success = true, message = serviceMessage });

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
            }

        }


    }
}
