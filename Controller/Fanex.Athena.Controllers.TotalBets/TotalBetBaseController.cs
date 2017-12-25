// --------------------------------------------------------------------------
// <copyright file="TotalBetController : TotalBetBaseController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.TotalBets
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using ONEbook.UM.Filters;
    using Service.Core.Helpers;

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    public class TotalBetBaseController : BaseController
    {
        public TotalBetBaseController()
        {
        }

        public TotalBetBaseController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper)
        : base(languageService, sessionService, appSetting, clientViewHelper)
        {
        }
    }
}