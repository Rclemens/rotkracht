using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;
using BlazorApp.Shared;
using System.ComponentModel;
using System.Linq;
using User = BlazorApp.Shared.User;
using Container = Microsoft.Azure.Cosmos.Container;

namespace RotKrachtApi;

public class UserFunction
{
    private readonly CosmosClient _cosmosClient;
    private Container _documentContainer;

    public UserFunction(CosmosClient cosmosClient)
    {
        _cosmosClient = cosmosClient;
        _documentContainer = _cosmosClient.GetContainer("Rotkracht", "Users");
    }

    [FunctionName("GetUser")]
    public async Task<IActionResult> GetUser(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{email}")] HttpRequest req,
    ILogger log, string email)
    {
        try
        {
            var result = _documentContainer.GetItemLinqQueryable<User>(true)
                .Where<User>(item => item.Email == email)
                .AsEnumerable()
                .FirstOrDefault();

            if(result == null)
                return new NotFoundResult();

            return new OkObjectResult(result);
        }
        catch (CosmosException e) when (e.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return new NotFoundResult();
        }
    }

    [FunctionName("DeleteUser")]
    public async Task<IActionResult> DeleteUser(
    [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "user/{id}/{email}")] HttpRequest req,
    ILogger log, string id, string email)
    {
        try
        {
            await _documentContainer.DeleteItemAsync<User>(id, new PartitionKey(email));

            return new OkResult();
        }
        catch (CosmosException e) when (e.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return new NotFoundResult();
        }
    }

    [FunctionName("AddUser")]
    public async Task<IActionResult> AddUser(
    [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "user")] HttpRequest req,
    ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        string requestData = await new StreamReader(req.Body).ReadToEndAsync().ConfigureAwait(false);
        var item = JsonConvert.DeserializeObject<BlazorApp.Shared.User>(requestData);

        item.Created = DateTime.Now;

        await _documentContainer.CreateItemAsync(item, new PartitionKey(item.Email));

        return new OkObjectResult(item);
    }

    [FunctionName("UpdateUser")]
    public async Task<IActionResult> UpdateUser(
    [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user")] HttpRequest req,
    ILogger log)
    {
        string requestData = await new StreamReader(req.Body).ReadToEndAsync().ConfigureAwait(false);
        var requestItem = JsonConvert.DeserializeObject<BlazorApp.Shared.User>(requestData);

        var result = _documentContainer.GetItemLinqQueryable<User>(true)
                .Where<User>(item => item.Id == requestItem.Id)
                .AsEnumerable()
                .FirstOrDefault();

        if (result == null)
            return new NotFoundResult();

        requestItem.Updated = DateTime.Now;
        await _documentContainer.UpsertItemAsync(requestItem, new PartitionKey(requestItem.Email));

        return new OkResult();
    }
}
