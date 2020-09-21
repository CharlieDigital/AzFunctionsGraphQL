using System;
using System.Collections.Generic;
using AzFunctionsGraphQL.Core.DataAccess;
using AzFunctionsGraphQL.Core.Domain;
using AzFunctionsGraphQL.Core.Model.GraphTypes;
using AzFunctionsGraphQL.Core.Model.GraphViewModels;
using GraphQL;
using GraphQL.Types;

namespace AzFunctionsGraphQL.Core.Model.Queries
{
    /// <summary>
    ///     This is the entry point for the resolution of GraphQL queries.  The fields in the query are the
    ///     top level entry points.
    /// </summary>
    public class StoreQuery : ObjectGraphType
    {
        /// <summary>
        ///     Constructor which creates the field mappings for the top level query.
        /// </summary>
        /// <param name="stores">Injected store repository.</param>
        public StoreQuery(StoreRepository stores)
        {
            Field<StoreType>("stores",
                // Define the list of available arguments; one for each one for this scope
                arguments: new QueryArguments(
                    new List<QueryArgument> {
                        // This argument allows us to filter the store by ID.
                        new QueryArgument<GuidGraphType>
                        {
                            Name = "id"
                        }
                    }),
                // Define the resolver which consumes the arguments
                resolve: context =>
                {
                    Guid storeId = context.GetArgument<Guid>("id");

                    Store store = stores.GetByIdAsync(storeId).Result;

                    // Note that we convert this to the view model.  The tools property is resolved separately by the DataLoader.
                    return new StoreV { Id = store.Id, Name = store.Name, PhoneNumber = store.PhoneNumber };
                });
        }
    }
}
