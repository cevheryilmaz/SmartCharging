using Microsoft.AspNetCore.Mvc;
using SmartCharging.Services;
using System.Threading.Tasks;
using System;
using SmartCharging.Models;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;

namespace SmartCharging.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChargeStationController : Controller
    {
        private ChargeStationService _chargeStationService;
        private GroupService _groupService;

        public ChargeStationController(ChargeStationService chargeStationService, GroupService groupService)
        {
            _chargeStationService = chargeStationService;
            _groupService = groupService;
        }

        /// <summary>
        /// Get charge station by ID.
        /// </summary>
        /// <param name="id">Charge station ID.</param>
        [HttpGet("get-station/{id:length(24)}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                return Ok(await _chargeStationService.GetStationById(id));
            }
            catch (Exception ex)
            {

                return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
            }

        }

        /// <summary>
        /// Create a new charge station.
        /// When a charging station is added, the groupId information about it is also saved in the db.
        /// The id information of the created charging station is transferred to the IdsofChargeStations array in the Group table.
        /// </summary>
        /// <param name="chargeStation">Charge station object to create.</param>
        [HttpPost("create-station")]
        public async Task<IActionResult> Create(ChargeStation chargeStation)
        {
            try
            {
                (ChargeStation newStation, string serviceMessage) = await _chargeStationService.CreateStation(chargeStation);
                bool statusOfStation = await _groupService.CheckChargeStation(newStation.Id);
                if (!statusOfStation)
                {
                    Group group = await _groupService.GetGroupById(chargeStation.GroupId);
                    List<string> chargeStationIds = group.IdsofChargeStations != null ? group.IdsofChargeStations.ToList() : new List<string>();
                    chargeStationIds.Add(newStation.Id);
                    group.IdsofChargeStations = chargeStationIds.ToArray();
                    await _groupService.UpdateGroup(group, group.Id);
                    
                }
                return Ok(new { data= newStation, success = true, message = serviceMessage });

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
            }
        }

        /// <summary>
        /// Update a charge station by ID.
        /// </summary>
        /// <param name="chargeStation">Updated charge station object.</param>
        /// <param name="id">Charge station ID.</param>
        [HttpPut("update-station/{id:length(24)}")]
        public async Task<IActionResult> Update(ChargeStation chargeStation, string id)
        {
            try
            {
                string serviceMessage = await _chargeStationService.UpdateStation(chargeStation, id);
                return Ok(new { success = true, message = serviceMessage });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
            }
        }

        /// <summary>
        /// Delete a charge station by ID.
        /// If a charging station is deleted, all connectors connected to it are deleted. 
        /// </summary>
        /// <param name="id">Charge station ID.</param>
        [HttpDelete("delete-station/{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                //delete all included connectors and charge stations according to id.
                string serviceMessage = await _chargeStationService.DeleteStation(id);
                await _chargeStationService.DeleteConnectorsByStationId(id);
                return Ok(new { success = true, message = serviceMessage  });

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
            }


        }

    }
}
