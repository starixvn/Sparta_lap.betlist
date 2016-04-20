// -----------------------------------------------------------------------
// <copyright file="BetSetting.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.BetSettings
{
    using Fanex.Athena.Model.Core.DataDomain;

    /// <summary>
    /// BetSetting class.
    /// </summary>
    public class BetSetting
    {
        public BetSetting()
        {
            this.EsportSetting = EsportSettingType.NoneOffering;
        }

        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }

        public decimal MaxPerMatchMax { get; set; }

        public decimal MaxPerMatchMin { get; set; }

        public decimal MaxPerBall { get; set; }

        public decimal MaxPerBallMax { get; set; }

        public decimal BetValueMax { get; set; }

        public decimal BetValueMin { get; set; }

        public string BetSettingType { get; set; }

        public string SportId { get; set; }

        public EsportSettingType EsportSetting { get; set; }
    }
}