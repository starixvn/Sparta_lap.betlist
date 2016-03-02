// -----------------------------------------------------------------------
// <copyright file="WeeklyLiveMatchDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.DataAccess.Implement.Base;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Models.Cache;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class WeeklyLiveMatchDataAccess : BaseDataAccess, IWeeklyLiveMatchDataAccess
    {
        /// <summary>
        /// Get data list.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="total">Total value.</param>
        /// <returns>The result value.</returns>
        public IEnumerable<WeeklyLiveMatch> GetDataList(WeeklyLiveMatchMetaModel metaModel, out WeeklyLiveMatch total)
        {
            var data = new List<WeeklyLiveMatch>();
            for (var i = 0; i < 100; i++)
            {
                data.Add(new WeeklyLiveMatch()
                {
                    Index = i,
                    SportName = CacheRef.GetSportName(i),
                    NumOfMatches = new List<int>() { 11111, 2, 3, 4, 5, 61111, 7, 8, 9, 9, 9 }
                });
            }

            total = new WeeklyLiveMatch();

            return data;
        }
    }
}