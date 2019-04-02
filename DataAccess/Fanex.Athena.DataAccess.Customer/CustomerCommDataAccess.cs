namespace Fanex.Athena.DataAccess.Customer
{
    using System;
    using System.Data;
    using System.Threading.Tasks;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Customer.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.MetaModel;
    using Fanex.Data;

    public class CustomerCommDataAccess : BaseDataAccess, ICustomerCommDataAccess
    {
        /// <summary>
        ///     Move customers to new group including 2 following steps:
        ///     1: Remove all customers commission in group.
        ///     2: Update new customers commission in this group.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <returns>The result value.</returns>
        public async Task<IResultInfo> MoveCustomersToGroupAsync(MoveToGroupComMetaModel metaModel)
        {
            const string UspName = "Lrf_CustomerMoveGroupComm";

            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("GroupCommId", metaModel.GroupComId);
                db.SetParameterValue("IsUpdateNG", metaModel.IsAllowedNG);
                db.SetParameterValue("IsUpdateRNG", metaModel.IsAllowedRNG);
                AddCommonParameters(db, metaModel);
                return await GetExecutionResult(db);
            }
        }

        public async Task<IResultInfo> RemoveCustomersFromGroupAsync(RemoveFromGroupComMetaModel metaModel)
        {
            const string UspName = "Lrf_CustomerRemoveGroupComm";

            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("IsRemovePlayercomm", metaModel.IsRemovePlayercomm);
                AddCommonParameters(db, metaModel);
                return await GetExecutionResult(db);
            }
        }

        public async Task<IResultInfo> UpdateCustomersCommissionAsync(UpdatedCustomerComMetaModel metaModel)
        {
            const string UspName = "Lrf_LicenseeCustCommUpd";

            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("SBComm", metaModel.SBComm);
                db.SetParameterValue("ParlayComm", metaModel.ParlayComm);
                db.SetParameterValue("NGComm", metaModel.NGComm);
                db.SetParameterValue("RNGComm", metaModel.CSComm);
                db.SetParameterValue("IsUpdateNG", metaModel.IsAllowedNG);
                db.SetParameterValue("IsUpdateRNG", metaModel.IsAllowedCS);
                AddCommonParameters(db, metaModel);
                return await GetExecutionResult(db);
            }
        }

        private static void AddCommonParameters(IObjectDb db, BaseCustomerComMetaModel metaModel)
        {
            db.SetParameterValue("LicenseeId", metaModel.LicenseeId);
            db.SetParameterValue("UserId", metaModel.UserId);
            db.SetParameterValue("Ip", metaModel.Ip);
            db.SetParameterValue("langkey", metaModel.Language);
            db.SetParameterValue("strCustIDOriginal", metaModel.CustomerToSQLParam);

            db.SetParameterValue("strCustID", null);

            AddResultParam(db);
        }

        private static async Task<IResultInfo> GetExecutionResult(IObjectDb db)
        {
            await db.ExecuteNonQueryAsync().ConfigureAwait(false);

            var result = GetResult(db);
            result.ExtendedData = Convert.ToString(db.GetParameterValue("strCustId"));
            return result;
        }
    }
}