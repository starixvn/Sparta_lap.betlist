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
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Customer.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.MetaModel;
    using Fanex.Common;

    public class GroupComDataAccess : BaseDataAccess, IGroupComDataAccess
    {
        public IList<GroupCommMetaModel> GetAll(int licenseeId)
        {
            const string UspName = "Lrf_LicenseeGroupCommSel";

            using (var db = GetObjectDb(UspName))
            {
                db.SetParameterValue("LicenseeID", licenseeId);

                return db.Query<GroupCommMetaModel>().ToList();
            }
        }

        public DataTable GetDataForExcel(int licenseeId)
        {
            const string UspName = "Lrf_LicenseeGroupCommSel";

            using (var db = GetObjectDb(UspName))
            {
                db.SetParameterValue("LicenseeID", licenseeId);

                var dataset = db.ExecuteDataSet();
                return dataset.Tables.Count > 0 ? dataset.Tables[0] : new DataTable();
            }
        }

        public GroupCommMetaModel GetById(int groupCommId, int licenseeId)
        {
            const string UspName = "Lrf_GroupCommGetById";

            using (var db = GetObjectDb(UspName))
            {
                db.SetParameterValue("LicenseeId", licenseeId);
                db.SetParameterValue("GroupCommID", groupCommId);

                return db.QueryEntity<GroupCommMetaModel>();
            }
        }

        public IResultInfo InsertGroup(GroupCommMetaModel item, IRequest request, int userId)
        {
            const string UspName = "Lrf_LicenseeGroupCommIns";

            using (var db = GetObjectDb(UspName))
            {
                db.SetParameterValue("LicenseeID", item.LicenseeId);
                db.SetParameterValue("GroupName", item.GroupName);
                db.SetParameterValue("SBComm", item.SBComm);
                db.SetParameterValue("ParlayComm", item.ParlayComm);
                db.SetParameterValue("NGComm", item.NGComm);
                db.SetParameterValue("RNGComm", item.CSComm);
                db.SetParameterValue("GroupColor", item.GroupColor);
                db.SetParameterValue("UserID", userId);
                db.SetParameterValue("IP", request.Ip);
                db.SetParameterValue("langkey", request.Language);
                db.SetParameterValue("IsDefault", item.IsDefaultGroup);
                db.SetParameterValue("IsResetDefault", item.IsResetDefault);

                db.SetParameterValue("GroupCommID", null);
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
                db.SetParameterValue("LicenseeID", newItem.LicenseeId);
                db.SetParameterValue("GroupCommId", newItem.GroupCommId);
                db.SetParameterValue("GroupName", newItem.GroupName);
                db.SetParameterValue("SbComm", newItem.SBComm);
                db.SetParameterValue("ParlayComm", newItem.ParlayComm);
                db.SetParameterValue("NGComm", newItem.NGComm);
                db.SetParameterValue("RNGComm", newItem.CSComm);
                db.SetParameterValue("IsUpdateNG", newItem.IsUpdateNG);
                db.SetParameterValue("IsUpdateRNG", newItem.IsUpdateCS);
                db.SetParameterValue("GroupColor", newItem.GroupColor);
                db.SetParameterValue("UserId", userId);
                db.SetParameterValue("Ip", request.Ip);
                db.SetParameterValue("langkey", request.Language);
                db.SetParameterValue("IsDefault", newItem.IsDefaultGroup);
                db.SetParameterValue("IsResetDefault", newItem.IsResetDefault);
                db.SetParameterValue("StrCustID", null);
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
                db.SetParameterValue("LicenseeId", licenseeId);
                db.SetParameterValue("GroupCommId", groupId);
                db.SetParameterValue("UserId", userId);
                db.SetParameterValue("Ip", ip);
                db.SetParameterValue("langkey", language);
                db.SetParameterValue("IsRemovePlayercomm", isRemoveCommSetting);
                db.SetParameterValue("IsResetDefault", isResetDefault);
                AddResultParam(db);
                db.SetParameterValue("StrCustID", null);
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
                db.SetParameterValue("LicenseeId", licenseeId);
                db.SetParameterValue("GroupName", groupName);

                return Converter.ToInt(db.ExecuteScalar()) != 1;
            }
        }
    }
}