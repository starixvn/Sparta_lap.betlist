// --------------------------------------------------------------------------
// <copyright file="OutstandingDailyStatementBetListController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Base;
    using Base.Attributes;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Service.StatelessCore.Interfaces;
    using Model.BetList.DTO;
    using Model.Core.DataDomain;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.CustomerInfoCustomerList)]
    public class OutstandingDailyStatementBetListController : BaseApiController
    {
        private readonly IBridgeClientService _bridgeClientService;

        private readonly IAuthorizationService _authorizationService;

        public OutstandingDailyStatementBetListController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService languageService,
            IBridgeClientService bridgeClientService,
            IAuthorizationService authorizationService)
        : base(sessionService, productService, languageService)
        {
            _bridgeClientService = bridgeClientService;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        public async Task<dynamic> Index(BetListMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            metaModel.UserId = SessionService.UserSession.UserId;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.ToDate = metaModel.ToDate.Date.AddDays(1).AddMinutes(-1);
            metaModel.IsShowPlayerCom = licenseeSession.IsAllowedCustomerComm;
            if (_authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, metaModel.TransId))
            {
                var betlistAPIUrl = new Uri($"http://localhost:55050/OutstandingDailyStatementBetList/GetData");
                return await _bridgeClientService.PostAsJsonAsync<BetListMetaModel, dynamic>(betlistAPIUrl, metaModel).ConfigureAwait(false);
            }

            return new List<DailyStatement>();
        }
    }
}