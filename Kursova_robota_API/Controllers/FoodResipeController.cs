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
    public class RecipeController : ControllerBase
    {
        public IDynamoDbClient _dynamoDbClient;
        private readonly ILogger<RecipeController> _logger;
        public RecipeController(IDynamoDbClient dynamoDbClient, ILogger<RecipeController> logger)
        {
            _dynamoDbClient = dynamoDbClient;
            _logger = logger;
        }
        [HttpGet("GetAll")]
        public List<ResultItem> Recipe(string Recipe, string MessageChatId)
        {
            FoodClient client = new FoodClient(_dynamoDbClient);
            return client.GetFoodRecipeAsync(Recipe, MessageChatId).Result;
        }
        [HttpPost("AddtoFavorites")]
        public async Task<ActionResult> AddtoFavorites([FromBody] RecipeDbRepository recipe)
        {
            var data = new RecipeDbRepository
            {
                MessageChatId = recipe.MessageChatId,
                Recipe = recipe.Recipe
            };
            var result = await _dynamoDbClient.PostDataToDb(data);
            if (result == false)
            {
                return BadRequest("Cannot insert value to database. Please see console log");
            }
            return Ok("Value has been successfully added to DB");
        }
        [HttpGet("GetFavorites")]
        public async Task<IActionResult> GetAllFavorites([FromQuery] string MessageChatId)
        {
            var response = await _dynamoDbClient.GetFavoritesByMessageChatId(MessageChatId);
            if (response == null)
                return NotFound("There are no records in db");
            var result = response
                .Select(x => new RecipeDbRepository()
                {
                    Recipe = x.Recipe,
                    MessageChatId = x.MessageChatId
                })
                .ToList();
            return Ok(result);
        }
        [HttpDelete("DeleteFromFavorites")]
        public async Task<ActionResult> DeleteFromFavoritesAsync([FromBody] RecipeDbRepository recipe)
        {
            var result = await _dynamoDbClient.DeleteFromFavorites(recipe);
            if (result == false)
            {
                return BadRequest("Cannot delete value from database");
            }
            return Ok("Value has been successfully deleted from DB");
        }
        [HttpDelete("ClearAllFavorites")]
        public async Task<ActionResult> ClearFavoritesAsync([FromBody] string MessageChatId)
        {
            var result = await _dynamoDbClient.ClearFavoritesByMessageChatId(MessageChatId);
            if (result == false)
            {
                return BadRequest("Cannot delete values from database");
            }
            return Ok("Values has been successfully deleted from DB");
        }
            [HttpGet("GetDiet")]
            public async Task<string> GetDietAsync([FromQuery] string MessageChatId)
            {
                var result = await _dynamoDbClient.GetDietByMessageChatId(MessageChatId);
                if (result == null)
                {
                    return null;
                }
                return result;
            }
        [HttpPost("AddDiet")]
        public async Task<ActionResult> AddtoFavorites([FromBody] DietDbRepository diet)
        {
            var data = new DietDbRepository
            {
                MessageChatId = diet.MessageChatId,
                Diet = diet.Diet
            };
            var result = await _dynamoDbClient.PostDietToDb(data);
            if (result == false)
            {
                return BadRequest("Cannot insert diet to database. Please see console log");
            }
            return Ok("Diet has been successfully added to DB");
        }
    }
}
