using System;
using System.IO;
using System.Threading.Tasks;
using AzFunctionsGraphQL.Core.DataAccess;
using AzFunctionsGraphQL.Core.Domain;
using AzFunctionsGraphQL.Core.Model.Schemas;
using GraphQL;
using GraphQL.DataLoader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AzFunctionsGraphQL.Core
{
    /// <summary>
    ///     This class is the set of Functions to support our GraphQL query service.
    /// </summary>
    public class QueryService
    {
        private readonly IDocumentWriter _documentWriter;
        private readonly DataLoaderDocumentListener _listener;
        private readonly CosmosGateway _cosmos;
        private readonly StoreRepository _stores;
        private readonly ToolRepository _tools;
        private readonly StoreSchema _schema;

        /// <summary>
        ///     Injection constructor.
        /// </summary>
        /// <param name="documentWriter">Instance injected by DI.</param>
        /// <param name="listener">Instance injected by DI.</param>
        /// <param name="schema">Instance injected by DI.</param>
        /// <param name="cosmos">Injected instance of the gateway for setting up the database</param>
        /// <param name="stores">Injected instance of the repository for setting up the database</param>
        /// <param name="tools">Injected instance of the repository for setting up the database</param>
        public QueryService(IDocumentWriter documentWriter, DataLoaderDocumentListener listener, StoreSchema schema,
            CosmosGateway cosmos, StoreRepository stores, ToolRepository tools)
        {
            _documentWriter = documentWriter;
            _listener = listener;
            _schema = schema;

            _cosmos = cosmos;
            _stores = stores;
            _tools = tools;
        }

        /// <summary>
        ///     Entry point for the GraphQL query.  The query will be in the request body.
        /// </summary>
        [FunctionName("Query")]
        public async Task<IActionResult> ExecuteQuery(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "query")] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            string json = await _schema.ExecuteAsync(_documentWriter, options =>
            {
                options.Query = requestBody;
                options.Listeners.Add(_listener); // Supports DataLoader
                options.UnhandledExceptionDelegate = context =>
                {
                    // Exceptions do not bubble out; must be explicitly handled.
                    log.LogError(context.Exception.ToString());
                };
            });

            return new OkObjectResult(json);
        }
        /// <summary>
        ///     Initializes the DB; THIS IS ONLY FOR TESTING PURPOSES!
        /// </summary>
        [FunctionName("Init")]
        public async Task<IActionResult> InitializeDatabase(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "init")] HttpRequest req,
            ILogger log)
        {
            await _cosmos.ResetDatabase();

            // Stores
            Store toolShack = await _stores.AddOrUpdate(new Store
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000001"),
                Name = "Tool Shack",
                PhoneNumber = "(555) 555-5501"
            });

            Store toolRentalSuperstore = await _stores.AddOrUpdate(new Store
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000002"),
                Name = "Tool Rental Superstore",
                PhoneNumber = "(555) 555-5502"
            });

            // Tools
            await _tools.AddOrUpdate(new Tool
            {
                Id = Guid.Parse("20000000-0000-0000-0000-000000000001"),
                StoreId = toolShack.Id,
                HourlyPrice = 50.0m,
                Name = "Jackhammer"
            });

            await _tools.AddOrUpdate(new Tool
            {
                Id = Guid.Parse("20000000-0000-0000-0000-000000000002"),
                StoreId = toolShack.Id,
                HourlyPrice = 25.0m,
                Name = "Rotary Hammer Drill"
            });

            await _tools.AddOrUpdate(new Tool
            {
                Id = Guid.Parse("20000000-0000-0000-0000-000000000003"),
                StoreId = toolRentalSuperstore.Id,
                HourlyPrice = 250.0m,
                Name = "Skid Steer"
            });

            await _tools.AddOrUpdate(new Tool
            {
                Id = Guid.Parse("20000000-0000-0000-0000-000000000004"),
                StoreId = toolRentalSuperstore.Id,
                HourlyPrice = 39.0m,
                Name = "Core Aerator"
            });

            await _tools.AddOrUpdate(new Tool
            {
                Id = Guid.Parse("20000000-0000-0000-0000-000000000004"),
                StoreId = toolRentalSuperstore.Id,
                HourlyPrice = 25.0m,
                Name = "Extension Ladder"
            });

            return new OkObjectResult("Database initialized.");
        }
    }
}
