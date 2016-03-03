// -----------------------------------------------------------------------
// <copyright file="WeeklyLiveMatchDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.Report
{
    using System;
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
        public IEnumerable<WeeklyLiveMatch> GetDataList(WeeklyLiveMatchMetaModel metaModel, out int[] total)
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

            total = new int[] { 14545, 2565, 553, 5654, 654, 5, 6, 6, 6, 7 };

            return data;
        }
    }
}