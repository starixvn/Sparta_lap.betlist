// -----------------------------------------------------------------------
// <copyright file="BetListService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetList
{
    using System.Collections.Generic;
    using DataAccess.Core.Configuration;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.DataAccess.BetList;
    using Fanex.Athena.DataAccess.BetList.Interfaces;
    using Fanex.Athena.Model.BetList.DTO;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Service.BetList.Interfaces;
    using Fanex.Athena.Service.Core.Configuration.Interfaces;
    using Fanex.Athena.Service.Core.Users;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Fanex.BetList.Core.Entities;

    public class BetListService : IBetListService
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        private readonly IBetListDataAccess _betlistDataAccess;

        private readonly IAuthorizationService _authorizationService;

        public BetListService()
        {
            _balanceUpDataAccess = new BalanceUpDataAccess();
            _betlistDataAccess = new BetListDataAccess();
            _authorizationService = new AuthorizationService();
        }

        [System.CLSCompliant(false)]
        public BetListService(
            IBalanceUpDataAccess balanceUpDataAccess,
            IBetListDataAccess betlistDataAccess,
            IAuthorizationService authorizationService)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
            _betlistDataAccess = betlistDataAccess;
            _authorizationService = authorizationService;
        }

        public TicketResult GetMixParlayDetailTicket(int licenseeId, long transId)
        {
            return _authorizationService.HasPermissionOnData(licenseeId, 0, transId)
                        ? _betlistDataAccess.GetMixParlayDetailTicket(transId)
                        : new TicketResult();
        }

        [System.CLSCompliant(false)]
        public SystemParlayData GetSystemParlayDetail(SystemParlayDetailMetaModel metaModel)
        {
            return _betlistDataAccess.GetSystemParlayDetail(metaModel);
        }

        public TicketResult GetBetListData(BetListMetaModel metaModel, string uspKey)
        {
            UpdateCommonInfo(metaModel);

            return _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, metaModel.TransId)
                        ? _betlistDataAccess.GetBetListData(metaModel, uspKey)
                        : new TicketResult();
        }

        /// <summary>
        /// Get Daily Statement Data of Customer.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="isAllowCustComm">The allowed customer commission.</param>
        /// <returns>The daily-statement list.</returns>
        public IList<DailyStatement> GetDailyStatementData(BetListMetaModel metaModel, bool isAllowCustComm)
        {
            return _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, metaModel.TransId)
                        ? _betlistDataAccess.GetDailyStatementData(metaModel, isAllowCustComm)
                         : new List<DailyStatement>();
        }

        private void UpdateCommonInfo(BetListMetaModel metaModel)
        {
            var balanceUp = _balanceUpDataAccess.GetBalanceUp();
            metaModel.MoveBetsDay = balanceUp.MoveBetsDay;
            metaModel.BalanceUpDay = balanceUp.BalanceUpDay;
        }
    }
}