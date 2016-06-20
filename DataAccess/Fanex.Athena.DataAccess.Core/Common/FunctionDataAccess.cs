// -----------------------------------------------------------------------
// <copyright file="FunctionDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common
{
    using System.Collections.Generic;
    using System.Data;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    /// <summary>
    /// Function data access.
    /// </summary>
    public class FunctionDataAccess : BaseDataAccess, IFunctionDataAccess
    {
        /// <summary>
        /// Get function list.
        /// </summary>
        /// <returns>The result value.</returns>
        public IList<Function> GetFunctionList()
        {
            var functionList = new List<Function>();
            using (var db = new ObjectDb("Lrf_FunctionsSel"))
            {
                using (var reader = db.ExecuteReader())
                {
                    var isProduction = AppSettings.Enviroment.Equals("PRO", System.StringComparison.OrdinalIgnoreCase);
                    while (reader.Read())
                    {
                        var environment = reader.ToString("Environment");
                        if (isProduction && environment.Equals("LIVE", System.StringComparison.OrdinalIgnoreCase) == false)
                        {
                            continue;
                        }

                        var function = new Function
                        {
                            FunctionId = reader.ToInt("functionid"),
                            ParentId = reader.ToInt("ParentId"),
                            Description = reader.ToString("Description"),
                            Url = reader.ToString("Url"),
                            DisplayOrder = reader.ToInt("DisplayOrder"),
                            IsActive = reader.ToBool("IsActive"),
                            IsMenu = reader.ToBool("IsMenu"),
                            IsNew = reader.ToBool("IsNew"),
                            FunctionKey = reader.ToString("FunctionKey")
                        };

                        functionList.Add(function);
                    }
                }
            }

            return functionList;
        }

        /// <summary>
        /// Get licensee function list.
        /// </summary>
        /// <param name="licenseeId">Licensee ID value.</param>
        /// <returns>The result value.</returns>
        public IList<Function> GetLicenseeFunctionList(int licenseeId)
        {
            var functionList = new List<Function>();
            using (var db = new ObjectDb("Lrf_LicenseeFunctionsSel"))
            {
                db.AddInParameter("licenseeid", DbType.Int32, licenseeId);
                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var function = new Function
                        {
                            FunctionId = Converter.ToInt(reader["FunctionId"]),
                        };
                        functionList.Add(function);
                    }
                }
            }

            return functionList;
        }

        /// <summary>
        /// Get User function list.
        /// </summary>
        /// <param name="licenseeId">Licensee ID value.</param>
        /// <param name="userId">User ID value.</param>
        /// <returns>The result value.</returns>
        public IList<Function> GetUserFunctionList(int licenseeId, int userId)
        {
            var functionList = new List<Function>();

            using (var db = new ObjectDb("Lrf_LicenseeUserFunctionsSel"))
            {
                db.AddInParameter("LicenseeId", DbType.Int32, licenseeId);
                db.AddInParameter("UserId", DbType.Int32, userId);

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var function = new Function
                        {
                            FunctionId = Converter.ToInt(reader["FunctionId"]),
                        };
                        functionList.Add(function);
                    }
                }
            }

            return functionList;
        }

        /// <summary>
        /// Assign user function.
        /// </summary>
        /// <param name="licenseeId">Licensee ID value.</param>
        /// <param name="custId">Customer ID value.</param>
        /// <param name="functionId">Function ID value.</param>
        /// <param name="userId">User ID value.</param>
        /// <param name="ip">IP parameter value.</param>
        /// <param name="langKey">Language key value.</param>
        /// <returns>The result value.</returns>
        public IResultInfo AssignUserFunction(int licenseeId, int custId, int functionId, int userId, string ip, string langKey)
        {
            const string UspName = "Lrf_LicenseeUserFunctionsIns";

            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    db.AddInParameter("UserID", DbType.Int32, userId);
                    db.AddInParameter("LicenseeID", DbType.Int32, licenseeId);
                    db.AddInParameter("FunctionID", DbType.Int32, functionId);
                    db.AddInParameter("CustID", DbType.Int32, custId);
                    db.AddInParameter("LangKey", DbType.String, langKey);
                    db.AddInParameter("IP", DbType.String, ip);

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
        /// Remove user function.
        /// </summary>
        /// <param name="licenseeId">Licensee ID value.</param>
        /// <param name="userId">User ID value.</param>
        /// <param name="custId">Customer ID value.</param>
        /// <param name="ip">IP parameter value.</param>
        /// <param name="functionId">Function ID value.</param>
        /// <param name="langKey">Language key value.</param>
        /// <returns>The result value.</returns>
        public IResultInfo RemoveUserFunction(int licenseeId, int userId, int custId, string ip, int functionId, string langKey)
        {
            const string UspName = "Lrf_LicenseeUserFunctionsDel";

            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    db.AddInParameter("UserID", DbType.Int32, userId);
                    db.AddInParameter("LicenseeID", DbType.Int32, licenseeId);
                    db.AddInParameter("FunctionID", DbType.Int32, functionId);
                    db.AddInParameter("CustID", DbType.Int32, custId);
                    db.AddInParameter("LangKey", DbType.String, langKey);
                    db.AddInParameter("IP", DbType.String, ip);

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
    }
}