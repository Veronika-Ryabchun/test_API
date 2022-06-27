using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Kursova_robota_API.Model;
using Kursova_robota_API.Client;
using System;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Logging;
using Amazon.DynamoDBv2;
using System.Threading.Tasks;
using System.Linq;

namespace Kursova_robota_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class YtController : ControllerBase
    {
        private readonly ILogger<RecipeController> _logger;
        public YtController(ILogger<RecipeController> logger)
        {
            _logger = logger;
        }
        [HttpGet("GetVideo")]
        public Items YtVideo(string name, string messageChatId)
        {
            YtClient client = new YtClient();
            return client.GetVideoAsync(name, messageChatId).Result;
        }
    }
}
