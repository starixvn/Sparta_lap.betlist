// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WeeklyLiveMatchService.cs" company="">
// The licensee win loss service.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Report
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Extension.Helper;
    using Fanex.Athena.DataAccess.Report;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Report.Interfaces;

    /// <summary>
    ///     The active customer service.
    /// </summary>
    public class WeeklyLiveMatchService : IWeeklyLiveMatchService
    {
        private const int DaysInWeek = 7;

        private readonly IWeeklyLiveMatchDataAccess _weeklyLiveMatchDataAccess;

        public WeeklyLiveMatchService()
        {
            this._weeklyLiveMatchDataAccess = new WeeklyLiveMatchDataAccess();
        }

        public IEnumerable<WeeklyLiveMatch> GetData(WeeklyLiveMatchMetaModel metaModel, out int[] total)
        {
            if (metaModel.IsValid)
            {
                return this._weeklyLiveMatchDataAccess.GetDataList(metaModel, out total);
            }
            else
            {
                total = new int[0];
                return new List<WeeklyLiveMatch>();
            }
        }

        public IEnumerable<string> GetWeekHeaders(DateTime fromDate, DateTime toDate, int numberOfWeeks)
        {
            var culture = CultureInfo.GetCultureInfo(CultureInfo.CurrentUICulture.IetfLanguageTag);

            var firstDate = DateTimeHelper.GetFirstDateOfWeek(fromDate);
            var nextDate = firstDate.AddDays(DaysInWeek - 1);

            var headers = new List<string>();
            headers.Add(GetMaxDate(firstDate, fromDate).ToString("dd MMM", culture) + " - " + GetMinDate(nextDate, toDate).ToString("dd MMM yyyy", culture));
            for (int i = 1; i < numberOfWeeks; i++)
            {
                firstDate = firstDate.AddDays(DaysInWeek);
                nextDate = firstDate.AddDays(DaysInWeek - 1);
                headers.Add(firstDate.ToString("dd MMM", culture) + " - " + GetMinDate(nextDate, toDate).ToString("dd MMM yyyy", culture));
            }

            return headers;
        }

        private static DateTime GetMaxDate(DateTime dateOne, DateTime dateTwo)
        {
            return dateOne > dateTwo ? dateOne : dateTwo;
        }

        private static DateTime GetMinDate(DateTime dateOne, DateTime dateTwo)
        {
            return dateOne < dateTwo ? dateOne : dateTwo;
        }
    }
}