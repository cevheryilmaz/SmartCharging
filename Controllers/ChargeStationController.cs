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
