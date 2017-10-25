// -----------------------------------------------------------------------
// <copyright file="GroupingMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Warehouse.MetaModel
{
    using System.Collections.Generic;
    using System.Linq;

    public static class GroupingMetaModel
    {
        public static IDictionary<string, string> GroupingList
        {
            get
            {
                return new Dictionary<string, string>
                {
                    { "1", "date" }, { "2", "month" }, { "3", "currency" }, { "4", "betType" }, { "5", "league" }
                };
            }
        }

        public static int DefaultValue
        {
            get
            {
                return int.Parse(GroupingList.Keys.First());
            }
        }
    }
}