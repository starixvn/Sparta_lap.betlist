// --------------------------------------------------------------------------
// <copyright file="OutstandingDailyStatementBetListController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Base;
    using Base.Attributes;
    using DataAccess.BetList;
    using DataAccess.BetList.Interfaces;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Model.BetList.DTO;
    using Model.Core.DataDomain;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users;
    using Service.Core.Users.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.CustomerInfoCustomerList)]
    public class OutstandingDailyStatementBetListController : BaseApiController
    {
        private readonly IAuthorizationService _authorizationService;

        private readonly IDailyStatementBetListDataAccess _dailyStatementBetListDataAccess;

        public OutstandingDailyStatementBetListController()
        {
            _authorizationService = new AuthorizationService();
            _dailyStatementBetListDataAccess = new DailyStatementBetListDataAccess();
        }

        public OutstandingDailyStatementBetListController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService languageService,
            IAuthorizationService authorizationService,
            IDailyStatementBetListDataAccess dailyStatementBetListDataAccess)
        : base(sessionService, productService, languageService)
        {
            _authorizationService = authorizationService;
            _dailyStatementBetListDataAccess = dailyStatementBetListDataAccess;
        }

        [HttpPost]
        public dynamic Index(BetListMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            metaModel.UserId = SessionService.UserSession.UserId;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.ToDate = metaModel.ToDate.Date.AddDays(1).AddMinutes(-1);

            return new
            {
                Main = GetData(metaModel, licenseeSession.IsAllowedCustomerComm),
                Total = new
                {
                    SumWinLoss = 0,
                    SumLicenseeWl = 0
                }
            };
        }

        private IEnumerable<DailyStatement> GetData(BetListMetaModel metaModel, bool isAllowCustComm)
        {
            return _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, metaModel.TransId)
                        ? _dailyStatementBetListDataAccess.GetDailyStatementData(metaModel, isAllowCustComm)
                         : new List<DailyStatement>();
        }
    }
}