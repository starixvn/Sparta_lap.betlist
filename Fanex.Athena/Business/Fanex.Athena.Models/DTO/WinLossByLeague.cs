// -----------------------------------------------------------------------
// <copyright file="WinLossByLeague.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.DTO
{
    using Fanex.Athena.Model.Core.Helper;

    /// <summary>
    /// WinLossByProduct class.
    /// </summary>
    public class WinLossByLeague
    {
        private string _homeName;

        private string _awayName;

        public int Index { get; set; }

        public int MatchId { get; set; }

        public int HomeId { get; set; }

        public string HomeName
        {
            get
            {
                if (_homeName == null)
                {
                    _homeName = CacheRef.GetTeamName(HomeId);
                }

                return _homeName;
            }
        }

        public int AwayId { get; set; }

        public string AwayName
        {
            get
            {
                if (_awayName == null)
                {
                    _awayName = CacheRef.GetTeamName(AwayId);
                }

                return _awayName;
            }
        }

        public int BetCount { get; set; }

        public decimal BACommission { get; set; }  

        public string ExternalId { get; set; }

        public string SystemId { get; set; }

        public int CustomerId { get; set; }
        
        public decimal TurnOver { get; set; }

        public decimal LicenseeWL { get; set; }

        public decimal AmountDue { get; set; }

        public decimal CustomerWinLoss { get; set; }
    }
}