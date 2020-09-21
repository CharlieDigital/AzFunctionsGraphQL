using AzFunctionsGraphQL.Core.Domain;

namespace AzFunctionsGraphQL.Core.DataAccess
{
    /// <summary>
    ///     Repository for accessing the tools.
    /// </summary>
    public class ToolRepository : CosmosRepositoryBase<Tool>
    {
        /// <summary>
        ///     Injection constructor.
        /// </summary>
        /// <param name="cosmos">The injected Cosmos gateway.</param>
        public ToolRepository(CosmosGateway cosmos) : base(cosmos)
        {
        }
    }
}
