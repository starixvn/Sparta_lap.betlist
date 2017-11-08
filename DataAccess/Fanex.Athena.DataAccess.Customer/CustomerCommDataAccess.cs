namespace Fanex.Athena.DataAccess.Customer
{
    using System;
    using System.Data;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Exception;
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
        public IResultInfo MoveCustomersToGroup(MoveToGroupComMetaModel metaModel)
        {
            const string UspName = "Lrf_CustomerMoveGroupComm";

            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("GroupCommId", DbType.Int32, metaModel.GroupComId);
                    db.AddInParameter("IsUpdateNG", DbType.Boolean, metaModel.IsAllowedNG);
                    db.AddInParameter("IsUpdateRacing", DbType.Boolean, metaModel.IsAllowedRC);
                    db.AddInParameter("IsUpdateLC", DbType.Boolean, metaModel.IsAllowedLC);
                    db.AddInParameter("IsUpdateRNG", DbType.Boolean, metaModel.IsAllowedRNG);
                    db.AddInParameter("IsUpdateAllbet", DbType.Boolean, metaModel.IsAllowedAB);
                    AddCommonParameters(db, metaModel);
                    return GetExecutionResult(db);
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName);
            }
        }

        public IResultInfo RemoveCustomersFromGroup(RemoveFromGroupComMetaModel metaModel)
        {
            const string UspName = "Lrf_CustomerRemoveGroupComm";

            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("IsRemovePlayercomm", DbType.Boolean, metaModel.IsRemovePlayercomm);
                    AddCommonParameters(db, metaModel);
                    return GetExecutionResult(db);
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName);
            }
        }

        public IResultInfo UpdateCustomersCommission(UpdatedCustomerComMetaModel metaModel)
        {
            const string UspName = "Lrf_LicenseeCustCommUpd";

            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("SBComm", DbType.Decimal, metaModel.SBComm);
                    db.AddInParameter("ParlayComm", DbType.Decimal, metaModel.ParlayComm);
                    db.AddInParameter("NGComm", DbType.Decimal, metaModel.NGComm);
                    db.AddInParameter("RacingComm", DbType.Decimal, metaModel.RCComm);
                    db.AddInParameter("LCComm", DbType.Decimal, metaModel.LCComm);
                    db.AddInParameter("RNGComm", DbType.Decimal, metaModel.CSComm);
                    db.AddInParameter("AllbetComm", DbType.Decimal, metaModel.ABComm);
                    db.AddInParameter("IsUpdateNG", DbType.Boolean, metaModel.IsAllowedNG);
                    db.AddInParameter("IsUpdateRacing", DbType.Boolean, metaModel.IsAllowedRC);
                    db.AddInParameter("IsUpdateLC", DbType.Boolean, metaModel.IsAllowedLC);
                    db.AddInParameter("IsUpdateRNG", DbType.Boolean, metaModel.IsAllowedCS);
                    db.AddInParameter("IsUpdateAllbet", DbType.Boolean, metaModel.IsAllowedAB);
                    AddCommonParameters(db, metaModel);
                    return GetExecutionResult(db);
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName);
            }
        }

        private static void AddCommonParameters(IObjectDb db, BaseCustomerComMetaModel metaModel)
        {
            db.AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
            db.AddInParameter("UserId", DbType.Int32, metaModel.UserId);
            db.AddInParameter("Ip", DbType.String, metaModel.Ip);
            db.AddInParameter("langkey", DbType.String, metaModel.Language);
            db.AddInParameter("strCustIDOriginal", DbType.String, metaModel.CustomerToSQLParam);

            db.AddOutParameter("strCustID", DbType.String, int.MaxValue);

            AddResultParam(db);
        }

        private static IResultInfo GetExecutionResult(IObjectDb db)
        {
            db.ExecuteNonQuery();
            var result = GetResult(db);
            result.ExtendedData = Convert.ToString(db.GetParameterValue("strCustId"));
            return result;
        }
    }
}