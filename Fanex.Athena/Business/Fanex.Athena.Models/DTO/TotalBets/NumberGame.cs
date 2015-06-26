// -----------------------------------------------------------------------
// <copyright file="NumberGame.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.TotalBets
{
    /// <summary>
    /// NumberGame class.
    /// </summary>
    public class NumberGame : TotalBetsBase
    {
        public string MatchCode { get; set; }

        public string NumberGameName
        {
            get { return "Number Game No " + MatchCode; }
        }

        public decimal Warrior2ND { get; set; }

        public decimal Warrior3RD { get; set; }

        public decimal OverUnderFirstBall { get; set; }

        public decimal OddEvenFirstBall { get; set; }

        public decimal OverUnderLastBall { get; set; }

        public decimal OddEvenFullTime { get; set; }

        public decimal OddEvenLastBall { get; set; }

        public decimal OverUnderNext { get; set; }

        public decimal OddEvenNext { get; set; }

        public decimal HighLow { get; set; }

        #region Sum Stake

        public decimal RunningStakeOverUnderFirstBall { get; set; }

        public decimal RunningStakeOddEvenFirstBall { get; set; }

        public decimal RunningStakeOverUnderLastBall { get; set; }

        public decimal RunningStakeOddEvenFullTime { get; set; }

        public decimal RunningStakeOddEvenLastBall { get; set; }

        public decimal RunningStakeOverUnderNext { get; set; }

        public decimal RunningStakeOddEvenNext { get; set; }

        public decimal RunningStakeHighLow { get; set; }

        #endregion Sum Stake

        public decimal NextComboOo { get; set; }

        public decimal NextComboUo { get; set; }

        public decimal NextComboOe { get; set; }

        public decimal NextComboUE { get; set; }

        public decimal NumberWheelLiveGame { get; set; }

        public decimal NumberWheelPreGame { get; set; }

        // Bet Count
        public int BetCountWarrior { get; set; }

        public int BetCountNextCombo { get; set; }

        public int BetCountOu1St { get; set; }

        public int BetCountOuLast { get; set; }

        public int BetCountOe1St { get; set; }

        public int BetCountOeLast { get; set; }

        public int BetCountOeFt { get; set; }

        public int BetCountOuNext { get; set; }

        public int BetCountOeNext { get; set; }

        public int BetCountHighLow { get; set; }

        public int BetCountNumberWheelLive { get; set; }

        public int BetCountNumberWheelPre { get; set; }
    }
}