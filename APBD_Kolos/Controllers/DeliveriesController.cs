using APBD_Kolos.Model;
using Microsoft.AspNetCore.Mvc;
using APBD_Kolos.Services;

namespace APBD_Kolos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveriesController : ControllerBase
    {
        private readonly IDbService _dbService;

        public DeliveriesController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDelivery(int id)
        {
            if (!await _dbService.DoesDeliveryExist(id))
            {
                return NotFound($"Delivery of {id} could not be found.");
            }

            var delivery = await _dbService.GetDelivery(id);
            return Ok(delivery);
        }
    }
    
    
}