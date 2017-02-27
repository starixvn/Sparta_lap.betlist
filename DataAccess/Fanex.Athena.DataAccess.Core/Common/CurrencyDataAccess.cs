// -----------------------------------------------------------------------
// <copyright file="CurrencyDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.DTO.Users;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;
    using Model.Core.MetaModel.Account;

    public class CurrencyDataAccess : BaseDataAccess, ICurrencyDataAccess
    {
        public IList<CurrencyUser> GetCurrencies(int licenseeId)
        {
            const string UspName = "Lrf_SelCurrencyByLicensee";
            var uspParams = new { LicenseeId = licenseeId };
            try
            {
                var entityList = new List<CurrencyUser>();
                using (var db = this.GetObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        while (reader.Read())
                        {
                            var currency = new CurrencyUser
                            {
                                CurrencyId = reader.GetInt32(reader.GetOrdinal("Currency")),
                                CurrencyName = reader.GetString(reader.GetOrdinal("CurrencyName")),
                                IsActive = true
                            };

                            entityList.Add(currency);
                        }
                    }
                }

                return entityList;
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        /// <summary>
        /// Assign currencies by user.
        /// </summary>
        /// <param name="request">Request value.</param>
        /// <returns>The result value.</returns>
        public IResultInfo AssignCurrenciesByUser(AssignCurrencyMetaModel request)
        {
            const string UspName = "Lrf_CurrencyCustIns";

            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("UserID", DbType.Int32, request.UserId);
                    db.AddInParameter("LicenseeID", DbType.Int32, request.LicenseeId);
                    db.AddInParameter("CurrencyXML", DbType.String, request.CurrencyList);
                    db.AddInParameter("ModifyBy", DbType.Int32, request.ModifyBy);
                    db.AddInParameter("LangKey", DbType.String, request.LangKey);
                    db.AddInParameter("IP", DbType.String, request.IP);

                    AddResultParam(db);
                    db.ExecuteNonQuery();

                    return GetResult(db);
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        /// <summary>
        /// Get currencies by user.
        /// </summary>
        /// <param name="licenseeId">Licensee ID value.</param>
        /// <param name="userId">User ID value.</param>
        /// <returns>The result value.</returns>
        public IList<CurrencyUser> GetCurrenciesByUser(int licenseeId, int userId)
        {
            const string UspName = "Lrf_SelCurrencyByLicenseeUser";
            IList<CurrencyUser> currencyList = new List<CurrencyUser>();

            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("UserID", DbType.Int32, userId);
                    db.AddInParameter("LicenseeID", DbType.Int32, licenseeId);

                    using (var reader = db.ExecuteReader())
                    {
                        var previousCurrencyId = 0;
                        var agentIdList = new List<int>();
                        while (reader.Read())
                        {
                            var currencyId = reader.ToInt("Currency");
                            if (previousCurrencyId != currencyId)
                            {
                                agentIdList = new List<int>();
                                var currencyUser = new CurrencyUser
                                {
                                    CurrencyId = reader.ToInt("Currency"),
                                    CurrencyName = reader.ToString("CurrencyName"),
                                    IsActive = reader.ToBool("IsActive"),
                                    AgentIdList = agentIdList
                                };

                                currencyList.Add(currencyUser);
                            }

                            previousCurrencyId = currencyId;
                            agentIdList.Add(reader.ToInt("AgentId"));
                        }
                    }
                }

                return currencyList;
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }
    }
}