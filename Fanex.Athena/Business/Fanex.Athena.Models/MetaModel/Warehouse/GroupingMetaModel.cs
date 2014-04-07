// -----------------------------------------------------------------------
// <copyright file="GroupingMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Warehouse
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.ViewModel;

    /// <summary>
    /// GroupingMetaModel class.
    /// </summary>
    public class GroupingMetaModel
    {
        private static readonly string[,] GroupingMap = new[,]
        {
            { "1", ViewResources.Day }, { "2", ViewResources.Month },
            { "3", ViewResources.Currency }, { "4", ViewResources.BetType },
            { "5", ViewResources.League }
        };

        public static int DefaultValue
        {
            get
            {
                return int.Parse(GroupingMap[0, 0]);
            }
        }

        public static Dictionary<string, string> GetCollection()
        {
            var list = new Dictionary<string, string>();
            for (var i = 0; i <= GroupingMap.GetUpperBound(0); i++)
            {
                list.Add(GroupingMap[i, 0], GroupingMap[i, 1]);
            }

            return list;
        }

        public static List<int> GetKeys()
        {
            var list = new List<int>();
            for (var i = 0; i <= GroupingMap.GetUpperBound(0); i++)
            {
                list.Add(int.Parse(GroupingMap[i, 0]));
            }

            return list;
        }
    }
}