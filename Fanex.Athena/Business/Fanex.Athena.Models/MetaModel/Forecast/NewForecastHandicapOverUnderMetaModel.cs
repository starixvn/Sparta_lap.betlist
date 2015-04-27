// -----------------------------------------------------------------------
// <copyright file="NewForecastHandicapOverUnderMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Forecast
{
    /// <summary>
    /// NewForecast HDP OU MetaModel class.
    /// </summary>
    public class NewForecastHandicapOverUnderMetaModel : ForecastBaseMetaModel
    {
        public NewForecastHandicapOverUnderMetaModel()
        {
            BetTypeString = "1";
            CurrencyId = 0;
            LeagueId = 0;
            MatchId = 0;
            IsShowFH = true;
            IsShowFT = true;
        }

        public int MatchId { get; set; }

        public int HomeTeamId { get; set; }

        public int AwayTeamId { get; set; }

        public bool IsNeutral { get; set; }

        public string HomeTeamName { get; set; }

        public string AwayTeamName { get; set; }

        public bool IsShowFH { get; set; }

        public bool IsShowFT { get; set; }

        public bool IsValid
        {
            get
            {
                return LeagueId > 0 && MatchId > 0;
            }
        }
    }
}