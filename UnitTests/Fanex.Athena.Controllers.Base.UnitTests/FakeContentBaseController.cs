namespace Fanex.Athena.Controllers.Base.UnitTests
{
    using Extension.Utility;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Model.Core.MetaModel;
    using Service.Core.Users.Interfaces;

    internal class FakeContentBaseController : ContentBaseController
    {
        internal FakeContentBaseController(ILanguageService languageService, ISessionService sessionService, IAppSettings appSetting)
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
    }
}