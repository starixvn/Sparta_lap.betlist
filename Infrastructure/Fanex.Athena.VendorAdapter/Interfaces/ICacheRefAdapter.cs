namespace Fanex.Athena.VendorAdapter.Interfaces
{
    public interface ICacheRefAdapter
    {
        string GetBettype(object bettypeId);

        string GetBettype(int betType, bool liveIndicator);

        string GetBettypeWithProductName(int betType, bool liveIndicator = false);

        string[] GetCombinationTypeCombo(string betTeam, string betCheck, decimal handicap1, decimal handicap2);

        string GetLeagueName(object leagueId);

        string GetMicroGamingGameName(string resourceId);

        string GetSportName(object sportId, string languageCode = null);

        string GetTeamName(object teamId);

        string GetOddsTypeName(object oddsTypeId);
    }
}