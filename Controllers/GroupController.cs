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

        [HttpPost("create-group")]
        public async Task<IActionResult> Create(Group group)
        {
            try
            {
                await _groupService.CreateGroup(group);
                return Ok(new { success = true, message = "Group created" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
            }
        }

        [HttpPut("update-group/{id:length(24)}")]
        public async Task<IActionResult> Update(Group group, string id)
        {
            try
            {
                await _groupService.UpdateGroup(group,id);
                return Ok(new { success = true, message = "Group Updated" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
            }
        }


        [HttpDelete("delete-group/{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                //delete all included charge station and group according to id.
                await _groupService.DeleteGroup(id);
                await _groupService.DeleteStationsByGroupId(id);
                return Ok(new { success = true, message = "Group and associated charging stations have been deleted." });

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
            }

        }


    }
}
