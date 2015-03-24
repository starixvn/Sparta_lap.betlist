// -----------------------------------------------------------------------
// <copyright file="BetSettingMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Customer.BetSetting
{
    using Fanex.Athena.Models.DataDomain;

    /// <summary>
    /// BetSettingMetaModel class.
    /// </summary>
    public class BetSettingMetaModel
    {
        public BetSettingMetaModel()
        {
            TabId = ProductType.Sportsbook;
            BetSettingName = "Sportsbook";
            Editable = true;
            IsAllowLiveCasino = false;
        }

        public ProductType TabId { get; set; }

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

        public bool IsAllowVoidBridge { get; set; }
    }
}