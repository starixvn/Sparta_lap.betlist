// -----------------------------------------------------------------------
// <copyright file="BetListService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetList
{
    using System.Collections.Generic;
    using Fanex.Athena.DataAccess.BetList;
    using Fanex.Athena.DataAccess.BetList.Interfaces;
    using Fanex.Athena.Model.BetList.DTO;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Service.BetList.Interfaces;
    using Fanex.Athena.Service.Core.Configuration;
    using Fanex.Athena.Service.Core.Configuration.Interfaces;
    using Fanex.Athena.Service.Core.Users;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Fanex.BetList.Core.Entities;

    public class BetListService : IBetListService
    {
        #region Fields

        private readonly IBalanceUpService _balanceUpService;
        private readonly IBetListDataAccess _betlistDataAccess;
        private readonly IAuthorizationService _authorizationService;

        #endregion Fields

        #region Constructor

        public BetListService()
        {
            _balanceUpService = new BalanceUpService();
            _betlistDataAccess = new BetListDataAccess();
            _authorizationService = new AuthorizationService();
        }

        [System.CLSCompliant(false)]
        public BetListService(
            IBalanceUpService balanceUpService, 
            IBetListDataAccess betlistDataAccess, 
            IAuthorizationService authorizationService)
        {
            _balanceUpService = balanceUpService;
            _betlistDataAccess = betlistDataAccess;
            _authorizationService = authorizationService;
        }

        #endregion Constructor

        #region IBetListService Members

        /// <summary>
        /// Get Mix Parlay Detail data.
        /// </summary>
        /// <param name="transId">The transaction identity.</param>
        /// <returns>The ticket result.</returns>
        public TicketResult GetMixParlayDetailTicket(int licenseeId, long transId)
        {
            return _authorizationService.HasPermissionOnData(licenseeId, 0, transId)
                        ? _betlistDataAccess.GetMixParlayDetailTicket(transId)
                        : new TicketResult();
        }

        /// <summary>
        /// Get System Parlay Detail data.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The system parlay.</returns>
        [System.CLSCompliant(false)]
        public SystemParlayData GetSystemParlayDetail(SystemParlayDetailMetaModel metaModel)
        {
            return _betlistDataAccess.GetSystemParlayDetail(metaModel);
        }

        /// <summary>
        /// Get Bet List Data.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="uspKey">The store procedure key.</param>
        /// <returns>The ticket result.</returns>
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

        #endregion IBetListService Members

        #region Update Match

        private void UpdateCommonInfo(BetListMetaModel metaModel)
        {
            var balanceUpInfo = _balanceUpService.BalanceUpInfo;
            metaModel.MoveBetsDay = balanceUpInfo.MoveBetsDay;
            metaModel.BalanceUpDay = balanceUpInfo.BalanceUpDay;
        }

        #endregion Update Match
    }
}