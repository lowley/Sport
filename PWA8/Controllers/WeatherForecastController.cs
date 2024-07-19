using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PWA6.ApplicationFiles;

namespace PWA8.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IHubContext<MapPinHub> hubContext;

        public WeatherForecastController(IHubContext<MapPinHub> context)
        {
            hubContext = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string message)
        {
            await hubContext.Clients.All.SendAsync("test", message);
            return Ok("Fini!");
        }
    }
}
