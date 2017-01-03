// --------------------------------------------------------------------------
// <copyright file="TotalBetController : TotalBetBaseController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controllers.TotalBets
{
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Core.Users.Interfaces;

    public class TotalBetBaseController : ReadonlyBaseController
    {
        public TotalBetBaseController()
        {
        }

        public TotalBetBaseController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting)
            : base(languageService, sessionService, appSetting)
        {
        }
    }
}