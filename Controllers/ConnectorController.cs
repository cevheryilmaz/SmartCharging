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
                ChargeStation stationData = await _chargeStationService.GetStationById(connector.ConnectedStationId);
                if (stationData.Connectors.Count < 5)
                {
                    (Connector newConnector, string serviceMessage) = await _connectorService.CreateConnector(connector);
                    stationData.Connectors.Add(newConnector);
                    await _chargeStationService.UpdateStation(stationData, stationData.Id);
                    return Ok(new { data = newConnector, success = true, message = serviceMessage });
                }
                else
                {
                    return Ok(new {  success = false, message = "Request rejected! The charging station has a maximum limit of connectors. That's why adding a connector could not be done." });
                }
               
            
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
                (string serviceMessage, bool serviceStatus) = await _connectorService.UpdateConnector(connector, id);
                return Ok(new {success = serviceStatus, message = serviceMessage });
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
               string serviceMessage = await _connectorService.DeleteConnector(id);
                return Ok(new { success = true, message = serviceMessage });

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
            }


        }

    }
}
