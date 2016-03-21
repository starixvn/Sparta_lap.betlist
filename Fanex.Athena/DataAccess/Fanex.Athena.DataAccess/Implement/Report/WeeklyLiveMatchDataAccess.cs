// -----------------------------------------------------------------------
// <copyright file="WeeklyLiveMatchDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.Report
{
    using System.Collections.Generic;
    using System.Data;
    using Data;
    using Exception;
    using Fanex.Athena.DataAccess.Implement.Base;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Logging;
    using Models.Cache;
    using Models.DTO.Configuration;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class WeeklyLiveMatchDataAccess : BaseDataAccess, IWeeklyLiveMatchDataAccess
    {
        private const int StartWeekColumnIndex = 2;

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
                        return ReadData(metaModel.NumberOfWeeks, reader, out totalData);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName);
            }
        }

        public IEnumerable<WeeklyLiveMatch> ReadData(int numberOfWeeks, IDataReader reader, out int[] totalData)
        {
            var data = new List<WeeklyLiveMatch>();
            totalData = new int[numberOfWeeks];
            var index = 1;
            while (reader.Read())
            {
                var item = new WeeklyLiveMatch()
                {
                    Index = index++,
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

                data.Add(item);
            }

            return data;
        }
    }
}