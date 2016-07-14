// -----------------------------------------------------------------------
// <copyright file="CancelledBetListService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetList
{
    using System.Collections.Generic;
    using Core.Users;
    using Core.Users.Interfaces;
    using DataAccess.BetList;
    using DataAccess.BetList.Interfaces;
    using Fanex.Athena.Model.BetList.DTO;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Interfaces;

    public class CancelledBetListService : ICancelledBetListService
    {
        private readonly ICancelledBetListDataAccess _cancelledBetListDataAccess;

        private readonly IAuthorizationService _authorizationService;

        public CancelledBetListService()
        {
            _cancelledBetListDataAccess = new CancelledBetListDataAccess();
            _authorizationService = new AuthorizationService();
        }

        public IList<CustomerCancelBetItem> GetCancelBetCustList(BetListMetaModel metaModel)
        {
            return _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, metaModel.TransId)
                        ? _cancelledBetListDataAccess.GetCancelBetCustList(metaModel)
                         : new List<CustomerCancelBetItem>();
        }
    }
}