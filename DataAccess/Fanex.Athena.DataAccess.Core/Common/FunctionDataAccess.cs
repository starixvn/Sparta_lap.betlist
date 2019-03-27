// -----------------------------------------------------------------------
// <copyright file="FunctionDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Common;
    using Fanex.Common.Extensions;

    public class FunctionDataAccess : BaseDataAccess, IFunctionDataAccess
    {
        private readonly IAppSettings _appSettings;

        public FunctionDataAccess(IAppSettings appSetting)
        {
            _appSettings = appSetting;
        }

        public async Task<IList<Function>> GetFunctionList()
        {
            var functionList = new List<Function>();
            using (var db = this.GetObjectDb("Lrf_FunctionsSel"))
            {
                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    var isProduction = _appSettings.Environment.Equals("PRO", System.StringComparison.OrdinalIgnoreCase);
                    while (reader.Read())
                    {
                        var environment = reader.ToString("Environment");
                        if (isProduction && !environment.Equals("LIVE", System.StringComparison.OrdinalIgnoreCase))
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
                            FunctionKey = reader.ToString("FunctionKey"),
                            IsFunction = reader.ToBool("IsFunction")
                        };

                        functionList.Add(function);
                    }
                }
            }

            return functionList;
        }

        public async Task<IList<Function>> GetLicenseeFunctionList(int licenseeId)
        {
            var functionList = new List<Function>();
            using (var db = this.GetObjectDb("Lrf_LicenseeFunctionsSel"))
            {
                db.SetParameterValue("licenseeid", licenseeId);
                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
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

        public async Task<IList<Function>> GetUserFunctionList(int licenseeId, int userId)
        {
            var functionList = new List<Function>();

            using (var db = this.GetObjectDb("Lrf_LicenseeUserFunctionsSel"))
            {
                db.SetParameterValue("LicenseeId", licenseeId);
                db.SetParameterValue("UserId", userId);

                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
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

        public IResultInfo AssignUserFunction(int licenseeId, int custId, int functionId, int userId, string ip, string langKey)
        {
            return ExecuteNonQueryUserFunction("Lrf_LicenseeUserFunctionsIns", licenseeId, userId, custId, ip, functionId, langKey);
        }

        public IResultInfo RemoveUserFunction(int licenseeId, int userId, int custId, string ip, int functionId, string langKey)
        {
            return ExecuteNonQueryUserFunction("Lrf_LicenseeUserFunctionsDel", licenseeId, userId, custId, ip, functionId, langKey);
        }

        private IResultInfo ExecuteNonQueryUserFunction(
            string uspName,
            int licenseeId,
            int userId,
            int custId,
            string ip,
            int functionId,
            string langKey)
        {
            using (var db = GetObjectDb(uspName))
            {
                db.SetParameterValue("UserID", userId);
                db.SetParameterValue("LicenseeID", licenseeId);
                db.SetParameterValue("FunctionID", functionId);
                db.SetParameterValue("CustID", custId);
                db.SetParameterValue("LangKey", langKey);
                db.SetParameterValue("IP", ip);

                AddResultParam(db);
                db.ExecuteNonQueryAsync();

                return GetResult(db);
            }
        }
    }
}