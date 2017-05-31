// -----------------------------------------------------------------------
// <copyright file="GroupComDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Customer
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Customer.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.MetaModel;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    public class GroupComDataAccess : BaseDataAccess, IGroupComDataAccess
    {
        public IList<GroupCommMetaModel> GetAll(int licenseeId)
        {
            const string UspName = "Lrf_LicenseeGroupCommSel";
            try
            {
                var entityList = new List<GroupCommMetaModel>();
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("LicenseeID", DbType.Int32, licenseeId);
                    using (var reader = db.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var entity = new GroupCommMetaModel
                            {
                                GroupCommId = reader.ToInt("GroupCommId"),
                                GroupName = reader.ToString("GroupName"),
                                SBComm = reader.ToDecimal("SBComm"),
                                ParlayComm = reader.ToDecimal("Parlaycomm"),
                                RCComm = reader.ToDecimal("RacingComm"),
                                NGComm = reader.ToDecimal("NGComm"),
                                LCComm = reader.ToDecimal("LCComm"),
                                CSComm = reader.ToDecimal("RNGComm"),
                                GroupColor = reader.ToString("GroupColor"),
                                NumberOfCustomer = reader.ToInt("NumberOfCustomer"),
                                IsDefaultGroup = reader.ToBool("IsDefault")
                            };

                            entityList.Add(entity);
                        }
                    }
                }

                return entityList;
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        public DataTable GetDataForExcel(int licenseeId)
        {
            const string UspName = "Lrf_LicenseeGroupCommSel";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("LicenseeID", DbType.Int32, licenseeId);

                    var dataset = db.ExecuteDataSet();
                    return dataset.Tables.Count > 0 ? dataset.Tables[0] : new DataTable();
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        public GroupCommMetaModel GetById(int groupCommId)
        {
            const string UspName = "Lrf_GroupCommGetById";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("GroupCommID", DbType.Int32, groupCommId);

                    using (var reader = db.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new GroupCommMetaModel
                            {
                                GroupCommId = groupCommId,
                                GroupName = reader.ToString("GroupName"),
                                SBComm = reader.ToDecimal("SBComm"),
                                ParlayComm = reader.ToDecimal("Parlaycomm"),
                                RCComm = reader.ToDecimal("RacingComm"),
                                NGComm = reader.ToDecimal("NGComm"),
                                LCComm = reader.ToDecimal("LCComm"),
                                CSComm = reader.ToDecimal("RNGComm"),
                                GroupColor = reader.To<string>("GroupColor"),
                                IsDefaultGroup = reader.ToBool("IsDefault")
                            };
                        }
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }

            return null;
        }

        public int InsertGroup(GroupCommMetaModel item, IRequest request, int userId, out IResultInfo resultInfo)
        {
            const string UspName = "Lrf_LicenseeGroupCommIns";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("LicenseeID", DbType.Int32, item.LicenseeId);
                    db.AddInParameter("GroupName", DbType.String, item.GroupName);
                    db.AddInParameter("SBComm", DbType.Decimal, item.SBComm);
                    db.AddInParameter("ParlayComm", DbType.Decimal, item.ParlayComm);
                    db.AddInParameter("RacingComm", DbType.Decimal, item.RCComm);
                    db.AddInParameter("NGComm", DbType.Decimal, item.NGComm);
                    db.AddInParameter("LCComm", DbType.Decimal, item.LCComm);
                    db.AddInParameter("RNGComm", DbType.Decimal, item.CSComm);
                    db.AddInParameter("GroupColor", DbType.String, item.GroupColor);
                    db.AddInParameter("UserID", DbType.Int32, userId);
                    db.AddInParameter("IP", DbType.String, request.Ip);
                    db.AddInParameter("langkey", DbType.String, request.Language);
                    db.AddInParameter("IsDefault", DbType.Boolean, item.IsDefaultGroup);
                    db.AddInParameter("IsResetDefault", DbType.Boolean, item.IsResetDefault);

                    // add output parameter
                    db.AddOutParameter("GroupCommID", DbType.Int32, 4);
                    AddResultParam(db);
                    db.ExecuteNonQuery();

                    item.GroupCommId = Converter.ToInt(db.GetParameterValue("GroupCommID"));

                    resultInfo = GetResult(db);
                }

                return resultInfo.Code;
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        public bool UpdateGroup(GroupCommMetaModel newItem, IRequest request, int userId, out IResultInfo resultInfo)
        {
            const string UspName = "Lrf_LicenseeGroupCommUpd";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("LicenseeID", DbType.Int32, newItem.LicenseeId);
                    db.AddInParameter("GroupCommId", DbType.Int32, newItem.GroupCommId);
                    db.AddInParameter("GroupName", DbType.String, newItem.GroupName);
                    db.AddInParameter("SbComm", DbType.Decimal, newItem.SBComm);
                    db.AddInParameter("ParlayComm", DbType.Decimal, newItem.ParlayComm);
                    db.AddInParameter("NGComm", DbType.Decimal, newItem.NGComm);
                    db.AddInParameter("RacingComm", DbType.Decimal, newItem.RCComm);
                    db.AddInParameter("LCComm", DbType.Decimal, newItem.LCComm);
                    db.AddInParameter("RNGComm", DbType.Decimal, newItem.CSComm);
                    db.AddInParameter("IsUpdateNG", DbType.Boolean, newItem.IsUpdateNG);
                    db.AddInParameter("IsUpdateRacing", DbType.Boolean, newItem.IsUpdateRacing);
                    db.AddInParameter("IsUpdateLC", DbType.Boolean, newItem.IsUpdateLC);
                    db.AddInParameter("IsUpdateRNG", DbType.Boolean, newItem.IsUpdateCS);
                    db.AddInParameter("GroupColor", DbType.String, newItem.GroupColor);
                    db.AddInParameter("UserId", DbType.Int32, userId);
                    db.AddInParameter("Ip", DbType.String, request.Ip);
                    db.AddInParameter("langkey", DbType.String, request.Language);
                    db.AddInParameter("IsDefault", DbType.Boolean, newItem.IsDefaultGroup);
                    db.AddInParameter("IsResetDefault", DbType.Boolean, newItem.IsResetDefault);
                    db.AddOutParameter("StrCustID", DbType.String, int.MaxValue);
                    AddResultParam(db);
                    db.ExecuteNonQuery();
                    newItem.GroupCommId = Converter.ToInt(db.GetParameterValue("GroupCommID"));
                    resultInfo = GetResult(db);
                    resultInfo.ExtendedData = Converter.ToString2(db.GetParameterValue("StrCustID"));
                }

                return resultInfo.Code == 0;
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        public int DeleteGroup(
            int groupId,
            int licenseeId,
            int userId,
            string ip,
            string language,
            bool isRemoveCommSetting,
            bool isResetDefault,
            out IResultInfo resultInfo)
        {
            const string UspName = "Lrf_LicenseeGroupCommDel";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("LicenseeId", DbType.Int32, licenseeId);
                    db.AddInParameter("GroupCommId", DbType.Int32, groupId);
                    db.AddInParameter("UserId", DbType.Int32, userId);
                    db.AddInParameter("Ip", DbType.String, ip);
                    db.AddInParameter("langkey", DbType.String, language);
                    db.AddInParameter("IsRemovePlayercomm", DbType.Boolean, isRemoveCommSetting);
                    db.AddInParameter("IsResetDefault", DbType.Boolean, isResetDefault);
                    AddResultParam(db);
                    db.AddOutParameter("StrCustID", DbType.String, int.MaxValue);
                    db.ExecuteScalar();
                    resultInfo = GetResult(db);
                    resultInfo.ExtendedData = Converter.ToString2(db.GetParameterValue("StrCustID"));
                }

                return resultInfo.Code;
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        public bool CheckByName(int licenseeId, string groupName)
        {
            const string UspName = "Lrf_GroupCommCheckByName";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("LicenseeId", DbType.Int32, licenseeId);
                    db.AddInParameter("GroupName", DbType.String, groupName);

                    return Converter.ToInt(db.ExecuteScalar()) != 1;
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