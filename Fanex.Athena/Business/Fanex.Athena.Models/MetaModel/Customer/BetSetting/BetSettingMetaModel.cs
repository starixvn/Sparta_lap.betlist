// -----------------------------------------------------------------------
// <copyright file="BetSettingMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Customer.BetSetting
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.Customer.BetSetting;
    using Fanex.Athena.Models.ViewModel;

    /// <summary>
    /// BetSettingMetaModel class.
    /// </summary>
    public class BetSettingMetaModel
    {
        public BetSettingMetaModel()
        {
            ProductId = ProductTypes.Sportsbook;
            BetSettingName = ViewResources.Sportsbook;
            Editable = true;
        }

        public int ProductId { get; set; }

        public string SiteName { get; set; }

        public int CustId { get; set; }

        public int LicenessId { get; set; }

        public int CurrencyId { get; set; }

        public string UserName { get; set; }

        public string NickName { get; set; }

        public string BetSettingName { get; set; }

        public bool Editable { get; set; }

        public bool IsAllowSportsBook { get; set; }

        public bool IsAllowRacing { get; set; }

        public bool IsAllowLiveCasino { get; set; }

        public bool IsAllowVirtualSport { get; set; }

        public bool IsAllowNumberGame { get; set; }

        public bool IsAllowRngCasino { get; set; }

        public bool IsAllowKeno { get; set; }

        public bool IsAllowAgCasino { get; set; }

        public bool IsAllowGoldDeluxe { get; set; }

        public bool IsAllowMacau { get; set; }

        public bool IsAllowMicroGamingLiveCasino { get; set; }

        public bool IsAllowMicroGamingRNGCasino { get; set; }

        public bool IsAllowBetTrade { get; set; }

        public IList<BetSettingProduct> BetSettingProducts { get; set; }
    }
}