using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzFunctionsGraphQL.Core.DataAccess;
using AzFunctionsGraphQL.Core.DataAccess.Support;
using AzFunctionsGraphQL.Core.Model.GraphViewModels;
using GraphQL.DataLoader;
using GraphQL.Types;

namespace AzFunctionsGraphQL.Core.Model.GraphTypes
{
    /// <summary>
    ///     This is a mapping class for the store.  If you are familiar with FluentNHibernate, this is
    ///     pretty much the same thing as a ClassMap.
    /// </summary>
    public class StoreType : ObjectGraphType<StoreV>
    {
        private ToolRepository _tools;

        /// <summary>
        ///     Injection constructor.
        /// </summary>
        /// <param name="loaderContext">The injected loader context for DataLoader.</param>
        /// <param name="tools">The injected repository for tools for lookup of store tools.</param>
        public StoreType(IDataLoaderContextAccessor loaderContext, ToolRepository tools)
        {
            _tools = tools;

            Field(s => s.Id);
            Field(s => s.Name);
            Field(s => s.PhoneNumber);

            // Now we want the tools; this is how we wire up the DataLoader.
            Field<ListGraphType<ToolType>>("tools", resolve: context =>
            {
                // Absolutely no idea what the string name is for.
                IDataLoader<Guid, IEnumerable<ToolV>> loader =
                    loaderContext.Context.GetOrAddCollectionBatchLoader<Guid, ToolV>(
                        "GetToolsByStoreId",
                        GetToolsByStoreIdAsync);

                return loader.LoadAsync(context.Source.Id);
            });
        }

        /// <summary>
        ///     This function is invoked to load the sub-tree of tools by store.
        /// </summary>
        /// <param name="storeIds">The batched store IDs.</param>
        /// <returns>A lookup list of tools by store IDs.</returns>
        public async Task<ILookup<Guid, ToolV>> GetToolsByStoreIdAsync(IEnumerable<Guid> storeIds)
        {
            List<ToolV> tools = (await _tools.GetItemsFiltered(0, 100,
                t => t.Name, SortDirection.Ascending,
                t => storeIds.Contains(t.StoreId)))
                .Select(t => new ToolV
                {
                    // TODO: Use some other mechanism to do this more efficiently!
                    Id = t.Id,
                    Name = t.Name, 
                    HourlyPrice = t.HourlyPrice,
                    StoreId = t.StoreId
                }).ToList();

            return tools.ToLookup(t => t.StoreId);
        }
    }
}
