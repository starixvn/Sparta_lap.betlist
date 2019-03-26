// -----------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Model.Core.MetaModel;

    public class ContentBaseController : CultureBaseController
    {
        protected ContentBaseController(
           ILanguageService languageService,
           ISessionService sessionService,
           IAppSettings appSetting)
           : base(languageService, sessionService, appSetting)
        {
        }

        protected void SetConvetedUSDCurrencyId(BaseMetaModel metaModel)
        {
            metaModel.CurrencyId = GetConvetedUSDCurrencyId(metaModel.LicenseeId);
        }

        protected int GetConvetedUSDCurrencyId(int licenseeId)
        {
            return GetBaseUsdLicensees().Contains(licenseeId.ToString())
                ? BaseCurrencyType.Usd : BaseCurrencyType.All;
        }

        protected void SetConvetedUSDCurrencyName(BaseMetaModel metaModel)
        {
            metaModel.BaseCurrency = GetBaseUsdLicensees().Contains(metaModel.LicenseeId.ToString()) ? "usd" : "eur";
        }

        private IEnumerable<string> GetBaseUsdLicensees()
        {
            var items = AppSettings.GetValue("BaseUsdLicensees");
            if (!string.IsNullOrEmpty(items))
            {
                return items.Split(',');
            }

            return new string[] { };
        }
    }
}