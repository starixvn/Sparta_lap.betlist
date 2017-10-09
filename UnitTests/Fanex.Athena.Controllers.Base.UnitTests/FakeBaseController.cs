namespace Fanex.Athena.Controllers.Base.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Extension.Utility;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Model.Core.MetaModel;
    using Service.Core.Users.Interfaces;

    public class FakeBaseController : BaseController
    {
        internal FakeBaseController(ILanguageService languageService, ISessionService sessionService, IAppSettings appSetting)
            : base(languageService, sessionService, appSetting)
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

        internal new IList<string> GetAllowLicenseesForStatusFunction()
        {
            return base.GetAllowLicenseesForStatusFunction();
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