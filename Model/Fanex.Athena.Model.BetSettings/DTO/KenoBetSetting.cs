// -----------------------------------------------------------------------
// <copyright file="KenoBetSetting.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetSettings.DTO
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class KenoBetSetting
    {
        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }

        public string GameTypeID { get; set; }
    }
}