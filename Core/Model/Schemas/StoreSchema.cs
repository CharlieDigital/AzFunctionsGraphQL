using System;
using AzFunctionsGraphQL.Core.Model.Queries;
using GraphQL.Types;

namespace AzFunctionsGraphQL.Core.Model.Schemas
{
    /// <summary>
    ///     GraphQL schema.
    /// </summary>
    public class StoreSchema : Schema
    {
        /// <summary>
        /// Create an instance of <see cref="T:GraphQL.Types.Schema" /> with a specified <see cref="T:System.IServiceProvider" />, used
        /// to create required objects
        /// </summary>
        public StoreSchema(IServiceProvider services) : base(services)
        {
            Query = services.GetService(typeof(StoreQuery)) as IObjectGraphType;
        }
    }
}
