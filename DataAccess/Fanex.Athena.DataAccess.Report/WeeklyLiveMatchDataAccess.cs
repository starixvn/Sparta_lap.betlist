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
    using System.Threading.Tasks;
    using Core;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Report.ResultModel;
    using VendorAdapter.Interfaces;

    public class WeeklyLiveMatchDataAccess : BaseDataAccess, IWeeklyLiveMatchDataAccess
    {
        private const int StartWeekColumnIndex = 2;

        private const string CustomSortingWeekColumn = "SortingWeekValue";

        private readonly ICacheRefAdapter _cacheRefAdapter;

        public WeeklyLiveMatchDataAccess(
            ICacheRefAdapter cacheRefAdapter)
        {
            _cacheRefAdapter = cacheRefAdapter;
        }

        public async Task<WeeklyLiveMatchResultModel> GetDataList(WeeklyLiveMatchMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_LiveMatchWeekly";

            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("FromDate", metaModel.FromDate);
                db.SetParameterValue("ToDate", metaModel.ToDate);

                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    return ReadData(reader, metaModel.SortColumn, metaModel.SortDirection);
                }
            }
        }

        private static List<WeeklyLiveMatch> SortData(List<WeeklyLiveMatch> data, string sortColumn, string sortDirection)
        {
            if (string.IsNullOrEmpty(sortColumn) || string.IsNullOrEmpty(sortDirection))
            {
                return data;
            }

            var isAscending = sortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase);
            var result = sortColumn.Equals(CustomSortingWeekColumn) ?
                Sort(data, x => x.SortingWeekValue, isAscending).ThenBy(x => x.SportName) :
                Sort(data, x => x.SportName, isAscending);

            AssignPropertyIndex(result);

            return result.ToList();
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

        private WeeklyLiveMatchResultModel ReadData(
            IDataReader reader,
            string sortColumn,
            string sortDirection)
        {
            var data = new List<WeeklyLiveMatch>();
            var sortingWeekIndex = -1;
            var sortColumnName = sortColumn;
            if (!string.IsNullOrEmpty(sortColumn) && sortColumn.StartsWith("Week", StringComparison.OrdinalIgnoreCase))
            {
                sortingWeekIndex = int.Parse(sortColumn.Replace("Week", string.Empty));
                sortColumnName = CustomSortingWeekColumn;
            }

            var numberOfWeeks = reader.FieldCount <= StartWeekColumnIndex ? 0 : reader.FieldCount - StartWeekColumnIndex;
            var totalData = new int[numberOfWeeks];

            var index = 1;
            while (reader.Read())
            {
                var item = new WeeklyLiveMatch
                {
                    SportName = _cacheRefAdapter.GetSportName(reader["sporttype"]),
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
                if (sortingWeekIndex >= 0 && sortingWeekIndex < numberOfWeeks)
                {
                    item.SortingWeekValue = numbersOfMatches[sortingWeekIndex];
                }

                item.Index = index++;
                data.Add(item);
            }

            data = SortData(data, sortColumnName, sortDirection);

            return new WeeklyLiveMatchResultModel
            {
                DataList = data,
                TotalData = totalData,
                NumberOfWeeks = numberOfWeeks
            };
        }
    }
}