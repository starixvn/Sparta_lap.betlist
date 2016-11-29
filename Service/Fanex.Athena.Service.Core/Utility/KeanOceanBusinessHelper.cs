namespace Fanex.Athena.Service.Core.Utility
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Users;

    /// <summary>
    /// <![CDATA[https://nexdev.net/projects/lrf-licensee-risk-and-forecast/wiki/Add_INH_for_KeenOcean.]]>
    ///  This business class is used for special business of KeanOcean.
    ///  Remove CNY=13 , Rename HKD=66 to CNY.
    /// </summary>
    public static class KeanOceanBusinessHelper
    {
        public static IList<CurrencyUser> ModifyCurrenciesForKeenOcean(IList<CurrencyUser> currencies)
        {
            currencies = currencies.Where(x => x.CurrencyId != CurrencyType.Cny).ToList();

            var currency = currencies.FirstOrDefault(x => x.CurrencyId == CurrencyType.Hkd && !x.CurrencyName.Equals("CNY"))
;
            if (currency != null)
            {
                currency.CurrencyName = "CNY";
            }

            return currencies;
        }

        public static void KeepHKDCurrencyName(IList<CurrencyUser> currencies)
        {
            var currency = currencies.FirstOrDefault(x => x.CurrencyId == CurrencyType.Hkd);
            if (currency != null)
            {
                currency.CurrencyName = "HKD";
            }
        }

        public static bool VerifyKeanOceanCurrencyRequest(IDictionary<string, int> currenyList, int licenseeId, int selectedCurrencyId)
        {
            return currenyList.Values.Any(i => i == selectedCurrencyId) || (
                licenseeId == LicenseeIdentityDefinition.KeenOcean && selectedCurrencyId == CurrencyType.Cny);
        }

        public static void AddHKDCurrencyForTurnOverReport(IDictionary<string, int> currenyList, int licenseeId)
        {
            if (licenseeId == LicenseeIdentityDefinition.KeenOcean && !currenyList.Keys.Contains("CNY"))
            {
                currenyList.Add("CNY", CurrencyType.Hkd);
            }
        }
    }
}