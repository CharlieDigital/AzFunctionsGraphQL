﻿using System.Collections.Generic;
using AzFunctionsGraphQL.Core.Domain;

namespace AzFunctionsGraphQL.Core.Model.GraphViewModels
{
    /// <summary>
    ///     Viewmodel (or DTO if you like) for Store which is what is GraphQL will be working with.
    /// </summary>
    /// <remarks>
    ///     In this case, because we are using a very simplified use case where the backend representation
    ///     is exactly the same as the front-end representation, we don't need to get fancy and just inherit.
    ///     However, if your back-end representation of the entity has fields that you don't want to expose,
    ///     consider creating the class as a standalone.
    /// </remarks>
    public class StoreV : Store
    {
        /// <summary>
        ///     This property differs from the base type as in the UI, we want to have the tools at the store.
        /// </summary>
        public IEnumerable<Tool> Tools;
    }
}
