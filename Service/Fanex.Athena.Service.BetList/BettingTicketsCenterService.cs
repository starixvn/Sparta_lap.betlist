namespace Fanex.Athena.Service.BetList
{
    using Core.Users;
    using Core.Users.Interfaces;
    using DataAccess.BetList;
    using DataAccess.BetList.Interfaces;
    using DataAccess.Core.Configuration;
    using DataAccess.Core.Configuration.Interfaces;
    using Interfaces;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;

    public class BettingTicketsCenterService : IBettingTicketsCenterService
    {
        private readonly IBettingTicketsCenterDataAccess _dataAccess;

        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        private readonly IAuthorizationService _authorizationService;

        public BettingTicketsCenterService()
        {
            _dataAccess = new BettingTicketsCenterDataAccess();
            _balanceUpDataAccess = new BalanceUpDataAccess();
            _authorizationService = new AuthorizationService();
        }

        public TicketResult GetBetListByRefno(RefNoBetListMetaModel metaModel)
        {
            if (!_authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, 0))
            {
                return null;
            }

            var balanceUp = _balanceUpDataAccess.GetBalanceUp();
            metaModel.BalanceUpDay = balanceUp.BalanceUpDay;
            metaModel.MoveBetsDay = balanceUp.MoveBetsDay;

            return _dataAccess.GetBetListByRefno(metaModel);
        }

        public TicketResult GetBetListForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel)
        {
            return _dataAccess.GetBetListForMultipleCustomers(metaModel);
        }

        public TicketResult GetBetListForSingleCustomer(SingleCustomersBetListMetaModel metaModel)
        {
            return _dataAccess.GetBetListForSingleCustomer(metaModel);
        }

        public TicketResult GetRunningBetList(BaseBettingTicketMetaModel metaModel)
        {
            return _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, 0)
                ? _dataAccess.GetRunningBetList(metaModel)
                : null;
        }
    }
}