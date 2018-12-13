// -----------------------------------------------------------------------
// <copyright file="CommonBetSetting.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetSettings.DTO
{
    /// <summary>
    /// CommonBetSetting class.
    /// </summary>
    public class CommonBetSetting
    {
        public int LicenseeId { get; set; }

        public string Ip { get; set; }

        public int UserId { get; set; }

        public string Language { get; set; }

        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }

        public int CustId { get; set; }
    }
}