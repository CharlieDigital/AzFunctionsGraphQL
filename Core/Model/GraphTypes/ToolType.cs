using AzFunctionsGraphQL.Core.Model.GraphViewModels;
using GraphQL.Types;

namespace AzFunctionsGraphQL.Core.Model.GraphTypes
{
    /// <summary>
    ///     This is a mapping class for the tool.  If you are familiar with FluentNHibernate, this is
    ///     pretty much the same thing as a ClassMap.
    /// </summary>
    public class ToolType : ObjectGraphType<ToolV>
    {
        /// <summary>
        ///     Initializes the mappings
        /// </summary>
        public ToolType()
        {
            Field(t => t.Id);
            Field(t => t.StoreId);
            Field(t => t.Name);
            Field(t => t.HourlyPrice);
        }
    }
}
