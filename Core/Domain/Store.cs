using System;
using AzFunctionsGraphQL.Core.DataAccess;

namespace AzFunctionsGraphQL.Core.Domain
{
    /// <summary>
    ///     This is the domain model which is distinct from the view model.  The domain model,
    ///     for example, may more closely match the underlying data storage model.  In this
    ///     case, because the underlying storage is Cosmos, we don't keep the tools on the store
    ///     because the tools can be unbounded.  In Cosmos, this would be problematic.  But from
    ///     the UI, we may want to query for a list of tools for rent at this store.
    /// </summary>
    [Container("Core")]
    public class Store : DomainEntityBase
    {
        /// <summary>
        ///     The phone number of the store.
        /// </summary>
        public string PhoneNumber;

        /// <summary>
        ///     The partition key for this type.
        /// </summary>
        public override string PartitionKey => PhoneNumber;
    }
}
