// -----------------------------------------------------------------------
// <copyright file="NewForecastHdpOuMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Forecast
{
    /// <summary>
    /// NewForecast HDP OU MetaModel class.
    /// </summary>
    public class NewForecastHdpOuMetaModel : ForecastBaseMetaModel
    {
        public NewForecastHdpOuMetaModel()
        {
            BetTypeString = "1";
            CurrencyId = 0;
            LeagueId = 0;
            MatchId = 0;
            IsShowFh = true;
            IsShowFt = true;
        }

        public int MatchId { get; set; }

        public int HomeTeamId { get; set; }

        public int AwayTeamId { get; set; }

        public bool IsNeutral { get; set; }

        public string HomeTeamName { get; set; }

        public string AwayTeamName { get; set; }

        public bool IsShowFh { get; set; }

        public bool IsShowFt { get; set; }

        public bool IsValid
        {
            get
            {
                return LeagueId > 0 && MatchId > 0;
            }
        }
    }
}