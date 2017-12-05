namespace Fanex.Athena.Model.BetSettings.MetaModel
{
    public class BetSettingCasinoStatusMetaModel : CoreCasinoStatusMetaModel
    {
        public int LicenseeId { get; set; }

        public string UserName { get; set; }

        public bool IsSyncCasino { get; set; }
    }
}