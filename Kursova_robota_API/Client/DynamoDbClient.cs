using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Kursova_robota_API.Const;
using Kursova_robota_API.Extensions;
using Kursova_robota_API.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kursova_robota_API.Client
{
    public class DynamoDbClient : IDynamoDbClient, IDisposable
    {
        public string _tableName;
        private readonly IAmazonDynamoDB _dynamoDb;
        public DynamoDbClient(IAmazonDynamoDB dynamoDB)
        {
            _dynamoDb = dynamoDB;
            _tableName = Constants.tableName;
        }

        public async Task<bool> DeleteFromFavorites(RecipeDbRepository data)
        {
            var request = new DeleteItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>()
                {
                    { "MessageChatId", new AttributeValue { S = data.MessageChatId } },
                    { "Recipe", new AttributeValue { S = data.Recipe } }
                }
            };
            try
            {
                var response = await _dynamoDb.DeleteItemAsync(request);
                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                Console.WriteLine("Here  is your error \n" + e);
                return false;
            }
        }

        public async Task<List<RecipeDbRepository>> GetFavoritesByMessageChatId(string messageChatId)
        {
            var request = new QueryRequest
            {
                TableName = _tableName,
                KeyConditionExpression = "MessageChatId = :v_Id",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                {":v_Id", new AttributeValue { S =  messageChatId  }} }
            };
            var response = await _dynamoDb.QueryAsync(request);
            if (response.Items == null)
                return null;
            return response.Items.ConvertAll(item => item.ToClass<RecipeDbRepository>());
        }

        public async Task<RecipeDbRepository> GetDietByMessageChatId(string messageChatId)
        {
            var item = new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"MessageChatId", new AttributeValue{S = messageChatId } }
                }
            };
            var response = await _dynamoDb.GetItemAsync(item);
            if (response.Item == null || !response.IsItemSet)
                return null;
            var result = response.Item.ToClass<RecipeDbRepository>();
            return result;
        }
        public async Task<bool> PostDataToDb(RecipeDbRepository data)
        {
            var request = new PutItemRequest
            {
                TableName = _tableName,
                Item = new Dictionary<string, AttributeValue>
                {
                    {"MessageChatId", new AttributeValue{S= data.MessageChatId} },
                    {"Recipe", new AttributeValue{S= data.Recipe} }
                }
            };
            try
            {
                var response = await _dynamoDb.PutItemAsync(request);
                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                Console.WriteLine("Here  is your error \n" + e);
                return false;
            }
        }

        public Task UpdateDataIntoDb()
        {
            throw new NotImplementedException();
        }
        public async Task<bool> ClearFavoritesByMessageChatId(string MessageChatId)
        {
            DynamoDBContext context = new DynamoDBContext(_dynamoDb);
            var config = new DynamoDBOperationConfig();
            config.OverrideTableName = _tableName;
            var bookBatch = context.CreateBatchWrite<RecipeDbRepository>(config);
            var items = await GetFavoritesByMessageChatId(MessageChatId);
            bookBatch.AddDeleteItems(items);
            await bookBatch.ExecuteAsync();
            return true;
        }

        /*public async Task<List<RecipeDbRepository>> GetAllFavorite()
        {
            var result = new List<RecipeDbRepository>();
            var request = new ScanRequest
            {
                TableName =_tableName,
            };
            var response = await _dynamoDb.ScanAsync(request);
            if (response.Items == null || response.Items.Count == 0)
                return null;
            foreach (Dictionary<string, AttributeValue> item in response.Items)
            {
                result.Add(item.ToClass<RecipeDbRepository>());
            }
            return result;
        }*/

        public void Dispose()
        {
            _dynamoDb.Dispose();
        }
    }
}