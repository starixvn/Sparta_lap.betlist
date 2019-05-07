namespace Fanex.Athena.Controllers.Base.UnitTests
{
    using Extension.Utility;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    internal class FakeCultureBaseController : CultureBaseController
    {
        internal FakeCultureBaseController(ILanguageService languageService, ISessionService sessionService, IAppSettings appSetting)
            : base(languageService, sessionService, appSetting)
        {
        }

        internal new void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }
    }
}