// -----------------------------------------------------------------------
// <copyright file="ResultDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using Fanex.Athena.DataAccess.Exception;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Data;
    using Fanex.Logging;

    /// <summary>
    /// Result data access.
    /// </summary>
    public class ResultDataAccess : IResultDataAccess
    {
        /// <summary>
        /// Get out right.
        /// </summary>
        /// <param name="metal">Meta model value.</param>
        /// <returns>The result value.</returns>
        public DataTable GetOutRight(ResultMetalModel metal)
        {
            const string UspName = "Lrf_Result_Outright";
            var uspParams = new
            {
                fromdate = metal.FromDate,
                todate = metal.ToDate,
                isshowall = true
            };

            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    var dataset = db.ExecuteDataSet(uspParams);
                    return dataset.Tables[0];
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        /// <summary>
        /// Get sports.
        /// </summary>
        /// <param name="date">Date value.</param>
        /// <param name="licenseeId">The licensee Id.</param>
        /// <returns>The result value.</returns>
        public IList<Sport> GetSports(DateTime date, int licenseeId)
        {
            const string UspName = "Lrf_Result_SportSelByDate";

            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    db.AddInParameter("eventdate", DbType.DateTime, date);
                    db.AddInParameter("licenseeid", DbType.Int32, licenseeId);
                    using (var reader = db.ExecuteReader())
                    {
                        var data = new List<Sport>();
                        while (reader.Read())
                        {
                            data.Add(new Sport(reader["sporttype"].ToString()));
                        }

                        return data;
                    }
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        /// <summary>
        /// Get leagues.
        /// </summary>
        /// <param name="date">Date value.</param>
        /// <param name="sportId">Sport ID value.</param>
        /// <returns>The result value.</returns>
        public IList<League> GetLeagues(DateTime date, int sportId)
        {
            const string UspName = "Lrf_Result_LeagueSelByDate";
            var uspParams = new
            {
                eventdate = date,
                sporttype = sportId,
                isshowall = true
            };

            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        var data = new List<League>();
                        while (reader.Read())
                        {
                            data.Add(new League { LeagueId = Convert.ToInt32(reader["leagueid"]) });
                        }

                        return data;
                    }
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        /// <summary>
        /// Get match.
        /// </summary>
        /// <param name="metalModel">Meta model value.</param>
        /// <returns>The result value.</returns>
        public DataTable GetMatchs(ResultMetalModel metalModel)
        {
            const string UspName = "Lrf_Result_MatchSelByLeague";
            var uspParams = new
            {
                fromdate = metalModel.FromDate,
                sporttype = metalModel.SportId,
                leagueid = metalModel.LeagueId,

                isshowall = true
            };

            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    var dataset = db.ExecuteDataSet(uspParams);
                    return dataset.Tables[0];
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        /// <summary>
        /// Get Number Game match.
        /// </summary>
        /// <param name="metalModel">Meta model value.</param>
        /// <returns>The result value.</returns>
        public DataTable GetNumberGameMatchs(ResultMetalModel metalModel)
        {
            const string UspName = "Lrf_Result_MatchSelNumberGameByLeague";
            var uspParams = new
            {
                fromdate = metalModel.FromDate,
                leagueid = metalModel.LeagueId,

                isshowall = true
            };

            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    var dataset = db.ExecuteDataSet(uspParams);
                    return dataset.Tables[0];
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        /// <summary>
        /// Get racing match.
        /// </summary>
        /// <param name="metalModel">Meta model value.</param>
        /// <returns>The result value.</returns>
        public DataTable GetRacingMatch(ResultMetalModel metalModel)
        {
            const string UspName = "Lrf_Result_RaceSelByMeeting";
            var uspParams = new
            {
                fromdate = metalModel.FromDate,
                leagueid = metalModel.LeagueId,
                sporttype = metalModel.SportId,
                isshowall = true
            };

            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    var dataset = db.ExecuteDataSet(uspParams);
                    return dataset.Tables[0];
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        /// <summary>
        /// Get Popup Match.
        /// </summary>
        /// <param name="matchId">Match ID value.</param>
        /// <param name="raceNo">Race Number value.</param>
        /// <param name="licenseeId">Licensee ID value.</param>
        /// <param name="userId">User ID value.</param>
        /// <returns>The result value.</returns>
        public DataTable GetPopUpMatch(int matchId, int raceNo, int licenseeId, int userId)
        {
            const string UspName = "Lrf_BetList_SelMatchResult";
            var uspParams = new
            {
                matchid = matchId,
                raceno = raceNo > -1 ? raceNo.ToString(CultureInfo.InvariantCulture) : null,
                licenseeid = licenseeId,
                userid = userId
            };

            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    var dataset = db.ExecuteDataSet(uspParams);
                    return dataset.Tables[0];
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        /// <summary>
        /// Get Popup Parlay Match.
        /// </summary>
        /// <param name="refNo">Reference Number value.</param>
        /// <returns>The result value.</returns>
        public DataTable GetPopUpParalayMatch(long refNo)
        {
            const string UspName = "Lrf_BetList_SelMatchResultParlay";
            var uspParams = new
            {
                refno = refNo
            };
            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    var dataset = db.ExecuteDataSet(uspParams);
                    return dataset.Tables[0];
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        /// <summary>
        /// Get Popup Number game match.
        /// </summary>
        /// <param name="matchId">Match ID value.</param>
        /// <returns>The result value.</returns>
        public DataTable GetPopUpNumberGameMatch(int matchId)
        {
            const string UspName = "Lrf_BetList_SelMatchResultNumberGame";
            var uspParams = new
            {
                matchid = matchId
            };
            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    var dataset = db.ExecuteDataSet(uspParams);
                    return dataset.Tables[0];
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }
    }
}