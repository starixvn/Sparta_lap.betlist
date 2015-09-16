// -----------------------------------------------------------------------
// <copyright file="CacheKey.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Configuration
{
    /// <summary>
    /// Cache key constant.
    /// </summary>
    public static class CacheKey
    {
        #region Report

        public static readonly string MemorycacheReportMobilewinloss = "MemoryCache_Report_MobileWinLoss";

        #endregion Report

        #region Common

        public static readonly string MemorycacheCommonMoveBet = "MemoryCache_Common_MoveBet";

        public static readonly string MemorycacheCommonBalanceup = "MemoryCache_Common_BalanceUp";

        public static readonly string MemorycacheCommonCurrency = "MemoryCache_Common_Currency";

        public static readonly string MemorycacheCommonCurrencyRevert = "MemoryCache_Common_Currency_Revert";

        public static readonly string MemorycacheCommonLicenseeCust = "MemoryCache_Common_LicenseeCust";

        public static readonly string MemorycacheCommonProduct = "MemoryCache_Common_Product";

        public static readonly string MemorycacheCommonLicenseeList = "MemoryCache_Common_Licenseelist";

        #endregion Common

        public static readonly string MemorycacheCommonCurrencyByUser = "MemoryCache_Common_Currency_By_User";
    }
}