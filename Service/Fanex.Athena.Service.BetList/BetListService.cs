// -----------------------------------------------------------------------
// <copyright file="BetListService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetList
{
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.DataAccess.BetList.Interfaces;
    using Fanex.Athena.Model.BetList.DTO;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Service.BetList.Interfaces;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Fanex.BetList.Core.Entities;
    using System.Collections.Generic;

    public class BetListService : IBetListService
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        private readonly IBetListDataAccess _betlistDataAccess;

        private readonly IAuthorizationService _authorizationService;

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

        public TicketResult GetMixParlayDetailTicket(IDictionary<int, string> currencyList, int licenseeId, long transId, int custId)
        {
            return _authorizationService.HasPermissionOnData(licenseeId, custId, transId)
                        ? _betlistDataAccess.GetMixParlayDetailTicket(currencyList, transId, custId)
                        : new TicketResult();
        }

        [System.CLSCompliant(false)]
        public SystemParlayData GetSystemParlayDetail(SystemParlayDetailMetaModel metaModel)
        {
            return _betlistDataAccess.GetSystemParlayDetail(metaModel);
        }

        public TicketResult GetBetListData(BetListMetaModel metaModel, IDictionary<int, string> currencyList, string uspKey)
        {
            AssignBalanceUpDates(metaModel);

            return _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, metaModel.TransId)
                        ? _betlistDataAccess.GetBetListData(metaModel, currencyList, uspKey)
                        : new TicketResult();
        }

        private void AssignBalanceUpDates(BetListMetaModel metaModel)
        {
            var balanceUp = _balanceUpDataAccess.GetBalanceUp();
            metaModel.MoveBetsDay = balanceUp.MoveBetsDay;
            metaModel.BalanceUpDay = balanceUp.BalanceUpDay;
        }
    }
}