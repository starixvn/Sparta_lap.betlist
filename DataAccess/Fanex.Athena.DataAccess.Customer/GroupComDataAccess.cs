// -----------------------------------------------------------------------
// <copyright file="GroupComDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Customer
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Customer.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.MetaModel;
    using Fanex.Common;
    using Session.Interfaces;

    public class GroupComDataAccess : BaseDataAccess, IGroupComDataAccess
    {
        public GroupComDataAccess(IAppSettings appSetting, ISessionStorage sessionStorage)
            : base(appSetting, sessionStorage)
        {
        }

        public IList<GroupCommMetaModel> GetAll(int licenseeId)
        {
            const string UspName = "Lrf_LicenseeGroupCommSel";

            using (var db = GetObjectDb(UspName))
            {
                db.AddInParameter("LicenseeID", DbType.Int32, licenseeId);

                return db.Query<GroupCommMetaModel>().ToList();
            }
        }

        public DataTable GetDataForExcel(int licenseeId)
        {
            const string UspName = "Lrf_LicenseeGroupCommSel";

            using (var db = GetObjectDb(UspName))
            {
                db.AddInParameter("LicenseeID", DbType.Int32, licenseeId);

                var dataset = db.ExecuteDataSet();
                return dataset.Tables.Count > 0 ? dataset.Tables[0] : new DataTable();
            }
        }

        public GroupCommMetaModel GetById(int groupCommId)
        {
            const string UspName = "Lrf_GroupCommGetById";

            using (var db = GetObjectDb(UspName))
            {
                db.AddInParameter("GroupCommID", DbType.Int32, groupCommId);

                return db.QueryEntity<GroupCommMetaModel>();
            }
        }

        public IResultInfo InsertGroup(GroupCommMetaModel item, IRequest request, int userId)
        {
            const string UspName = "Lrf_LicenseeGroupCommIns";

            using (var db = GetObjectDb(UspName))
            {
                db.AddInParameter("LicenseeID", DbType.Int32, item.LicenseeId);
                db.AddInParameter("GroupName", DbType.String, item.GroupName);
                db.AddInParameter("SBComm", DbType.Decimal, item.SBComm);
                db.AddInParameter("ParlayComm", DbType.Decimal, item.ParlayComm);
                db.AddInParameter("RacingComm", DbType.Decimal, item.RCComm);
                db.AddInParameter("NGComm", DbType.Decimal, item.NGComm);
                db.AddInParameter("RNGComm", DbType.Decimal, item.CSComm);
                db.AddInParameter("AllbetComm", DbType.Decimal, item.ABComm);
                db.AddInParameter("GroupColor", DbType.String, item.GroupColor);
                db.AddInParameter("UserID", DbType.Int32, userId);
                db.AddInParameter("IP", DbType.String, request.Ip);
                db.AddInParameter("langkey", DbType.String, request.Language);
                db.AddInParameter("IsDefault", DbType.Boolean, item.IsDefaultGroup);
                db.AddInParameter("IsResetDefault", DbType.Boolean, item.IsResetDefault);

                db.AddOutParameter("GroupCommID", DbType.Int32, 4);
                AddResultParam(db);
                db.ExecuteNonQuery();

                item.GroupCommId = Converter.ToInt(db.GetParameterValue("GroupCommID"));

                return GetResult(db);
            }
        }

        public IResultInfo UpdateGroup(GroupCommMetaModel newItem, IRequest request, int userId)
        {
            const string UspName = "Lrf_LicenseeGroupCommUpd";

            using (var db = GetObjectDb(UspName))
            {
                db.AddInParameter("LicenseeID", DbType.Int32, newItem.LicenseeId);
                db.AddInParameter("GroupCommId", DbType.Int32, newItem.GroupCommId);
                db.AddInParameter("GroupName", DbType.String, newItem.GroupName);
                db.AddInParameter("SbComm", DbType.Decimal, newItem.SBComm);
                db.AddInParameter("ParlayComm", DbType.Decimal, newItem.ParlayComm);
                db.AddInParameter("NGComm", DbType.Decimal, newItem.NGComm);
                db.AddInParameter("RacingComm", DbType.Decimal, newItem.RCComm);
                db.AddInParameter("RNGComm", DbType.Decimal, newItem.CSComm);
                db.AddInParameter("AllbetComm", DbType.Decimal, newItem.ABComm);
                db.AddInParameter("IsUpdateNG", DbType.Boolean, newItem.IsUpdateNG);
                db.AddInParameter("IsUpdateRacing", DbType.Boolean, newItem.IsUpdateRacing);
                db.AddInParameter("IsUpdateRNG", DbType.Boolean, newItem.IsUpdateCS);
                db.AddInParameter("IsUpdateAllbet", DbType.Boolean, newItem.IsUpdateAB);
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
                var resultInfo = GetResult(db);
                resultInfo.ExtendedData = Converter.ToString2(db.GetParameterValue("StrCustID"));
                return resultInfo;
            }
        }

        public IResultInfo DeleteGroup(
            int groupId,
            int licenseeId,
            int userId,
            string ip,
            string language,
            bool isRemoveCommSetting,
            bool isResetDefault)
        {
            const string UspName = "Lrf_LicenseeGroupCommDel";

            using (var db = GetObjectDb(UspName))
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
                var resultInfo = GetResult(db);
                resultInfo.ExtendedData = Converter.ToString2(db.GetParameterValue("StrCustID"));
                return resultInfo;
            }
        }

        public bool CheckByName(int licenseeId, string groupName)
        {
            const string UspName = "Lrf_GroupCommCheckByName";

            using (var db = GetObjectDb(UspName))
            {
                db.AddInParameter("LicenseeId", DbType.Int32, licenseeId);
                db.AddInParameter("GroupName", DbType.String, groupName);

                return Converter.ToInt(db.ExecuteScalar()) != 1;
            }
        }
    }
}