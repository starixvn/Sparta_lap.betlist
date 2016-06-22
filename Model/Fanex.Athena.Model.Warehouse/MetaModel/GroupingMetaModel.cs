// -----------------------------------------------------------------------
// <copyright file="GroupingMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Warehouse.MetaModel
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// GroupingMetaModel class.
    /// </summary>
    public sealed class GroupingMetaModel
    {
        [SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional",
            Justification = "Reviewed.")]
        private static readonly string[,] GroupingMap = new[,]
        {
            { "1", "date" }, { "2", "month" },
            { "3", "currency" }, { "4", "betType" },
            { "5", "league" }
        };

        private GroupingMetaModel()
        {
        }

        public static int DefaultValue
        {
            get
            {
                return int.Parse(GroupingMap[0, 0]);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Reviewed. Suppression is OK here.")]
        public static Dictionary<string, string> GetCollection()
        {
            var list = new Dictionary<string, string>();
            for (var i = 0; i <= GroupingMap.GetUpperBound(0); i++)
            {
                list.Add(GroupingMap[i, 0], GroupingMap[i, 1]);
            }

            return list;
        }
    }
}