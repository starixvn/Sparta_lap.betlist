// --------------------------------------------------------------------------
// <copyright file="BaseApiController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.Base
{
    using System.Linq;
    using System.Web.Http;
    using Attributes;
    using DataAccess.Core.Common;
    using DataAccess.Core.Common.Interfaces;
    using Model.Core.DataDomain;
    using ONEbook.UM.Filters;
    using Service.Core.Common;
    using Service.Core.Common.Interfaces;
    using Service.Core.Configuration;
    using Service.Core.Configuration.Interfaces;
    using Service.Core.Users;
    using Service.Core.Users.Interfaces;
    using Web.ActionFilter;

    [CustomApiAuthorize]
    [CheckUMFilter]
    [XFrameOptionApi]
    public class CommonApiController : ApiController
    {
        private readonly IMoveBetService _moveBetService;

        private readonly ILanguageService _languageService;

        private readonly ISportTypeDataAccess _sportTypeDataAccess;

        private readonly ISessionService _sessionService;

        public CommonApiController()
        {
            _moveBetService = new MoveBetService();
            _languageService = new LanguageService();
            _sportTypeDataAccess = new SportTypeDataAccess();
            _sessionService = new SessionService();
        }

        public CommonApiController(
            IMoveBetService moveBetService,
            ILanguageService languageService,
            ISportTypeDataAccess sportTypeDataAccess,
            ISessionService sessionService)
        {
            _moveBetService = moveBetService;
            _languageService = languageService;
            _sportTypeDataAccess = sportTypeDataAccess;
            _sessionService = sessionService;
        }

        [HttpGet]
        public string GetMoveBetNotification()
        {
            var language = _languageService.GetCurrentLanguage();

            return _moveBetService.GetMoveBetNotification(language);
        }

        [HttpGet]
        [System.CLSCompliant(false)]
        [FunctionAuthorization(PermissionKeys.BettingTicketCenter)]
        public dynamic GetSports(string productCode)
        {
            var sports = _sportTypeDataAccess.GetSportsAndProducts(
                _sessionService.LicenseeSession.LicenseeId,
                _sessionService.UserSession.Language,
                productCode);

            return sports.Where(x => x.ParentId > -1).Select(x => new { value = x.ID, lable = x.Name });
        }
    }
}