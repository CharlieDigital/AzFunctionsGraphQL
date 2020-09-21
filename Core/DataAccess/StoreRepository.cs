using AzFunctionsGraphQL.Core.Domain;

namespace AzFunctionsGraphQL.Core.DataAccess
{
    /// <summary>
    ///     Repository for accessing the stores.
    /// </summary>
    public class StoreRepository : CosmosRepositoryBase<Store>
    {
        /// <summary>
        ///     Injection constructor.
        /// </summary>
        /// <param name="cosmos">The injected Cosmos gateway.</param>
        public StoreRepository(CosmosGateway cosmos) : base(cosmos)
        {
        }
    }
}
