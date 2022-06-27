using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Kursova_robota_API.Const;
using Kursova_robota_API.Model;
using Newtonsoft.Json;

namespace Kursova_robota_API.Client
{
    class YtClient
    {
        private HttpClient _client;
        private static string _address;
        private static string _apikey;
        public YtClient()
        {
            _address = Constants.ytaddress;
            _apikey = Constants.apikey;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_address);
            _client.DefaultRequestHeaders.Add("X-RapidAPI-Host", _address.Substring(8));
            _client.DefaultRequestHeaders.Add("X-RapidAPI-Key", _apikey);
        }
        public async Task<Items> GetVideoAsync(string name, string messageChatId)
        {
            var responce = await _client.GetAsync($"/search?q={name}&part=id&maxResults=1");
            var content = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<Items>(content);
            return result;
        }
    }
}
