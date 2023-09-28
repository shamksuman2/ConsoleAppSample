using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace cam1_receiver.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {
        private readonly ILogger<MainController> _logger;

        public MainController(ILogger<MainController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get([FromBody] dynamic input)
        {
            _logger.LogInformation($"Msg cam1 {input}");
            return Ok(input);
        }


        [HttpPost]
        public IActionResult post([FromBody] dynamic input)
        {
            _logger.LogInformation($"Msg cam1 {input}");
            return Ok(input);
        }

    }
}