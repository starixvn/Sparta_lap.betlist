// -----------------------------------------------------------------------
// <copyright file="CasinoWinLossDetail.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Casino.DTO.RNGResultDetail
{
    /// <summary>
    /// CasinoWinLossDetail class.
    /// </summary>
    public class CasinoWinLossDetail
    {
        public int BetTypeId { get; set; }

        public string BetTypeName { get; set; }

        public decimal Bet { get; set; }

        public decimal Odds { get; set; }

        public decimal Win { get; set; }

        public decimal WinLoss { get; set; }

        public string BetContent { get; set; }

        public int GameTypeId { get; set; }

        public string Currency { get; set; }
    }
}