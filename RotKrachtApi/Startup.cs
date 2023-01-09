using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RotKrachtApi;
using System;

[assembly: FunctionsStartup(typeof(Startup))]

namespace RotKrachtApi;

public class Startup : FunctionsStartup
{
    private static readonly IConfigurationRoot Configuration = new ConfigurationBuilder()
        .SetBasePath(Environment.CurrentDirectory)
        .AddJsonFile("appsettings.json", true)
        .AddEnvironmentVariables()
        .Build();

    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddSingleton(s =>
        {
            var connectionString = Configuration["CosmosDBConnection"];
            if(string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Missing CosmosDBConnection");
            }
            return new CosmosClientBuilder(connectionString).Build();
        });
    }
}