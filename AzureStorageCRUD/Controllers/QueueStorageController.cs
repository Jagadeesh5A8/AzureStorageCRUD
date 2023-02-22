using Azure.Storage.Queues.Models;
using AzureStorageCRUD.Models;
using AzureStorageCRUD.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzureStorageCRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueStorageController : ControllerBase
    {
        private readonly IQueueService _queueService;
        public QueueStorageController(IQueueService queueService)
        {
            _queueService = queueService;
        }

        [HttpGet("ReceiveMessage")]
        public async Task<IActionResult> Receive(string queueName)
        {
            try
            {
                var response = await _queueService.ReceiveMessage(queueName);
                if (response != null)
                {
                    return Ok(response);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> Send(string queueName, string queueMessage)
        {
            try
            {
                var response = await _queueService.SendMessage(queueName, queueMessage);
                if (response != null)
                {
                    return Ok(response);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateMessage")]
        public async Task<IActionResult> Update(string queuename)
        {
            try
            {
                var response = await _queueService.UpdateMessage(queuename);
                if (response != null)
                {
                    return Ok(response);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("ClearMessage")]
        public async Task<IActionResult> Clear(string queueName)
        {
            try
            {
                var response = await _queueService.ClearMessage(queueName);
                if (response != 0)
                {
                    return NoContent();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
