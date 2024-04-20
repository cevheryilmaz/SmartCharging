using Microsoft.AspNetCore.Mvc;
using SmartCharging.Services;
using System.Threading.Tasks;
using System;
using SmartCharging.Models;

namespace SmartCharging.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectorController : Controller
    {
        private ConnectorService      _connectorService;
        private ChargeStationService _chargeStationService;

        public ConnectorController(ConnectorService connectorService, ChargeStationService chargeStation)
        {
            _connectorService = connectorService;
            _chargeStationService = chargeStation;
        }

        [HttpGet("get-connector/{id:length(24)}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                return Ok(await _connectorService.GetConnectorById(id));
            }
            catch (Exception ex)
            {

                return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
            }

        }

        [HttpPost("create-connector")]
        public async Task<IActionResult> Create(Connector connector)
        {
            try
            {
                Connector newConnector = await _connectorService.CreateConnector(connector);
                ChargeStation stationData = await _chargeStationService.GetStationById(connector.ConnectedStationId);
                stationData.Connectors.Add(newConnector);
                await _chargeStationService.UpdateStation(stationData, stationData.Id);
                return Ok(new { success = true, message = "Connector was added and the necessary update was made to the charging station." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
            }
        }

        [HttpPut("update-connector/{id:length(24)}")]
        public async Task<IActionResult> Update(Connector connector, string id)
        {
            try
            {
                await _connectorService.UpdateConnector(connector, id);
                return Ok(new { success = true, message = "Connector Updated" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
            }
        }


        [HttpDelete("delete-connector/{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _connectorService.DeleteConnector(id);
                return Ok(new { success = true, message = "Connector Deleted" });

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
            }


        }
    }
}
