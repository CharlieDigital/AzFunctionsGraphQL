using System;
using AzFunctionsGraphQL.Core.DataAccess;

namespace AzFunctionsGraphQL.Core.Domain
{
    /// <summary>
    ///     Domain model for a tool.  
    /// </summary>
    [Container("Core")]
    public class Tool : DomainEntityBase
    {
        /// <summary>
        ///     The ID of the store where this tool is at.
        /// </summary>
        public Guid StoreId;

        /// <summary>
        ///     The hourly rental price of the tool.
        /// </summary>
        public decimal HourlyPrice;

        /// <summary>
        ///     The partition key for this type.
        /// </summary>
        public override string PartitionKey => StoreId.ToString();
    }
}
