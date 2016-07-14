// -----------------------------------------------------------------------
// <copyright file="WeeklyLiveMatchDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core;
    using Core.Exception;
    using Data;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Logging;
    using Model.Core.Helper;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class WeeklyLiveMatchDataAccess : BaseDataAccess, IWeeklyLiveMatchDataAccess
    {
        private const int StartWeekColumnIndex = 2;

        private const string CustomSortingWeekColumn = "SortingWeekValue";

        public IEnumerable<WeeklyLiveMatch> GetDataList(WeeklyLiveMatchMetaModel metaModel, out int[] totalData)
        {
            string uspName = "Lrf_Rpt_LiveMatchWeekly";

            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    db.AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
                    db.AddInParameter("UserId", DbType.Int32, metaModel.UserId);
                    db.AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
                    db.AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);

                    using (var reader = db.ExecuteReader())
                    {
                        return ReadData(metaModel.NumberOfWeeks, reader, metaModel.SortColumn, metaModel.SortDirection, out totalData);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName);
            }
        }

        private static IEnumerable<WeeklyLiveMatch> ReadData(
            int numberOfWeeks,
            IDataReader reader,
            string sortColumn,
            string sortDirection,
            out int[] totalData)
        {
            var data = new List<WeeklyLiveMatch>();
            totalData = new int[numberOfWeeks];
            var sortingWeekIndex = -1;
            if (!string.IsNullOrEmpty(sortColumn) && sortColumn.StartsWith("Week", System.StringComparison.OrdinalIgnoreCase))
            {
                sortingWeekIndex = int.Parse(sortColumn.Replace("Week", string.Empty));
                sortColumn = CustomSortingWeekColumn;
            }

            while (reader.Read())
            {
                var item = new WeeklyLiveMatch()
                {
                    SportName = CacheRef.GetSportName(reader["sporttype"]),
                    NumbersOfMatches = new List<int>()
                };
                var numbersOfMatches = new int[numberOfWeeks];
                for (int i = StartWeekColumnIndex; i < reader.FieldCount; i++)
                {
                    var value = reader.IsDBNull(i) ? 0 : reader.GetInt32(i);
                    numbersOfMatches[i - StartWeekColumnIndex] = value;
                    totalData[i - StartWeekColumnIndex] += value;
                }

                item.NumbersOfMatches = numbersOfMatches;
                if (sortingWeekIndex >= 0)
                {
                    item.SortingWeekValue = numbersOfMatches[sortingWeekIndex];
                }

                data.Add(item);
            }

            if (string.IsNullOrEmpty(sortColumn) == false && string.IsNullOrEmpty(sortDirection) == false)
            {
                data = SortData(data, sortColumn, sortDirection).ToList();
            }

            AssignPropertyIndex(data);
            return data;
        }

        private static IEnumerable<WeeklyLiveMatch> SortData(IEnumerable<WeeklyLiveMatch> data, string sortColumn, string sortDirection)
        {
            var isAscending = sortDirection.Equals("asc", System.StringComparison.OrdinalIgnoreCase);
            if (sortColumn.Equals(CustomSortingWeekColumn))
            {
                return Sort(data, x => x.SortingWeekValue, isAscending).ThenBy(x => x.SportName);
            }
            else
            {
                return Sort(data, x => x.SportName, isAscending);
            }
        }

        private static IOrderedEnumerable<T> Sort<T, TKey>(IEnumerable<T> data, Func<T, TKey> keySelector, bool isAscending = true)
        {
            return isAscending ? data.OrderBy(keySelector) : data.OrderByDescending(keySelector);
        }

        private static void AssignPropertyIndex(IEnumerable<WeeklyLiveMatch> data)
        {
            int index = 1;
            foreach (var item in data)
            {
                item.Index = index++;
            }
        }
    }
}