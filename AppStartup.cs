using System;
using System.Text.Json;
using AzFunctionsGraphQL.Core.DataAccess;
using AzFunctionsGraphQL.Core.Model.GraphTypes;
using AzFunctionsGraphQL.Core.Model.Queries;
using AzFunctionsGraphQL.Core.Model.Schemas;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Execution;
using GraphQL.SystemTextJson;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(AzFunctionsGraphQL.AppStartup))]

namespace AzFunctionsGraphQL
{
    /// <summary>
    ///     Startup class used to initialize the dependency injection.
    /// </summary>
    /// <remarks>
    ///     See: https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection
    /// </remarks>
    public class AppStartup : FunctionsStartup
    {
        /// <summary>
        ///     Configure the DI container.
        /// </summary>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Registrations for domain data access.
            builder.Services.AddSingleton(new CosmosClient(
                    Environment.GetEnvironmentVariable("CosmosEndpoint"),
                    Environment.GetEnvironmentVariable("CosmosAuthKey")));

            builder.Services.AddSingleton<CosmosGateway>();
            builder.Services.AddSingleton<StoreRepository>();
            builder.Services.AddSingleton<ToolRepository>();

            // Core services for GraphQL
            builder.Services.AddSingleton<JsonSerializerOptions>();
            builder.Services.AddSingleton(new ErrorInfoProviderOptions());
            builder.Services.AddSingleton<IErrorInfoProvider, ErrorInfoProvider>();
            builder.Services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            builder.Services.AddSingleton<IDocumentWriter, DocumentWriter>();

            // Needed for DataLoader
            builder.Services.AddSingleton<IServiceProvider>(s => new FuncServiceProvider(s.GetRequiredService));
            builder.Services.AddSingleton<IDataLoaderContextAccessor, DataLoaderContextAccessor>(); 
            builder.Services.AddSingleton<DataLoaderDocumentListener>();

            // App specific registrations for GraphQL
            builder.Services.AddSingleton<StoreType>();
            builder.Services.AddSingleton<ToolType>();

            builder.Services.AddSingleton<StoreQuery>();
            builder.Services.AddSingleton<StoreSchema>();
        }
    }
}
