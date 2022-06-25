using Amazon.DynamoDBv2.Model;
using Kursova_robota_API.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kursova_robota_API.Client
{
    public interface IDynamoDbClient
    {
        public Task<List<RecipeDbRepository>> GetFavoritesByMessageChatId(string messageChatId);
        public Task<bool> PostDataToDb(RecipeDbRepository data);
        public Task<bool> DeleteFromFavorites(RecipeDbRepository data);
        public Task<bool> ClearFavoritesByMessageChatId(string MessageChatId);
        public Task<string> GetDietByMessageChatId(string messageChatId);
        public Task<bool> PostDietToDb(DietDbRepository data);
    }
}