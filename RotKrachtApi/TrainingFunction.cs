using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using BlazorApp.Shared;
using Microsoft.Azure.Cosmos;

namespace RotKrachtApi
{
    public class TrainingFunction
    {
        private readonly CosmosClient _cosmosClient;
        private Container _documentContainer;

        public TrainingFunction(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
            _documentContainer = _cosmosClient.GetContainer("Rotkracht","Trainings");
        }


        [FunctionName("GetTraining")]
        public static async Task<IActionResult> GetTraining(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("AddTraining")]
        public async Task<IActionResult> AddTraining(
            [HttpTrigger(AuthorizationLevel.Function, "Post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestData = await new StreamReader(req.Body).ReadToEndAsync().ConfigureAwait(false);
            var item  = JsonConvert.DeserializeObject<Training>(requestData);

            item.Created = DateTime.Now;

            await _documentContainer.CreateItemAsync(item, new PartitionKey(item.DateKey));

            return new OkObjectResult(item);
        }
    }
}