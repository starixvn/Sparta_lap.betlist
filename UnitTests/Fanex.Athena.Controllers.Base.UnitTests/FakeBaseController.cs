namespace Fanex.Athena.Controllers.Base.UnitTests
{
    using System.Web.Mvc;
    using Extension.Utility;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Model.Core.MetaModel;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;

    internal class FakeBaseController : BaseController
    {
        internal FakeBaseController(ILanguageService languageService, ISessionService sessionService, IAppSettings appSetting, IClientViewHelper clientViewHelper)
            : base(languageService, sessionService, appSetting, clientViewHelper)
        {
        }

        internal new void SetConvetedUSDCurrencyId(BaseMetaModel metaModel)
        {
            base.SetConvetedUSDCurrencyId(metaModel);
        }

        internal new void SetConvetedUSDCurrencyName(BaseMetaModel metaModel)
        {
            base.SetConvetedUSDCurrencyName(metaModel);
        }

        internal new void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }

        internal new ViewResult View(string viewName, string masterName, object model)
        {
            return base.View(viewName, masterName, model);
        }
    }
}