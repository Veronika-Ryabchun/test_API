using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Kursova_robota_API.Const;
using Kursova_robota_API.Model;
using static Kursova_robota_API.Client.DynamoDbClient;
using Newtonsoft.Json;

namespace Kursova_robota_API.Client
{
    class FoodClient
    {
        private HttpClient _client;
        private static string _address;
        private static string _apikey;
        private IDynamoDbClient _dynamoDb;
        public FoodClient(IDynamoDbClient dynamoDbClient)
        {
            _address = Constants.address;
            _apikey = Constants.apikey;
            _client = new HttpClient();
            _dynamoDb = dynamoDbClient;
            _client.BaseAddress = new Uri(_address);
            _client.DefaultRequestHeaders.Add("X-RapidAPI-Host", _address.Substring(8));
            _client.DefaultRequestHeaders.Add("X-RapidAPI-Key", _apikey);
        }
        public async Task<List<ResultItem>> GetFoodRecipeAsync(string recipe, string messageChatId, bool ignoreDiet)
        {
            HttpResponseMessage responce;
            string diet = await _dynamoDb.GetDietByMessageChatId(messageChatId);
            if (!ignoreDiet && diet != null && diet.Length != 0)
            {
                responce = await _client.GetAsync($"/recipes/list?from=0&size=10&tags={diet}&q={recipe}");
            }
            else
            {
                responce = await _client.GetAsync($"/recipes/list?from=0&size=10&q={recipe}");
            }
            var content = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<Results>(content);
            return result.results;
        }
    }
}

