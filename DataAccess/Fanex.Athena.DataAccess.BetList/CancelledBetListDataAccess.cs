// -----------------------------------------------------------------------
// <copyright file="CancelledBetListDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetList
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.Model.BetList.DTO;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Data;
    using Interfaces;
    using Logging;

    public class CancelledBetListDataAccess : BaseDataAccess, ICancelledBetListDataAccess
    {
        private const string UspKey = "Lrf_BetList_SelDeleted_UserList";

        public IList<CustomerCancelBetItem> GetCancelBetCustList(BetListMetaModel metaModel)
        {
            try
            {
                using (var db = this.GetObjectDb(UspKey))
                {
                    db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
                    db.AddInParameter("userid", DbType.Int32, metaModel.UserId);
                    db.AddInParameter("CurrencyId", DbType.String, metaModel.CurrencyId.ToString());
                    return db.Query<CustomerCancelBetItem>().ToList();
                }
            }
            catch (ObjectDbException exception)
            {
                LogService.Error(exception.Message + "\n UspName=" + UspKey, exception);
                throw new DataAccessException(exception.Message, UspKey);
            }
        }
    }
}