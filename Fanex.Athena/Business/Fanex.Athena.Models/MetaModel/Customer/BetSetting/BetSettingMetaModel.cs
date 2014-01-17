// -----------------------------------------------------------------------
// <copyright file="BetSettingMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Customer.BetSetting
{
    /// <summary>
    /// BetSettingMetaModel class.
    /// </summary>
    public class BetSettingMetaModel
    {
        public BetSettingMetaModel()
        {
            TabId = 1;
            BetSettingName = "Sportsbook";
            Editable = true;
            IsAllowLiveCasino = false;
        }

        public int TabId { get; set; }

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
    }
}