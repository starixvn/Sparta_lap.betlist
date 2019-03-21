using Fanex.RemotingRef;

namespace Fanex.Athena.VendorAdapter.Interfaces
{
    public interface ICacheRefAdapter
    {
#pragma warning disable CS3003 // Type is not CLS-compliant
        IRemotingRefService RemotingRefService { get; }
#pragma warning restore CS3003 // Type is not CLS-compliant

        string GetBettype(object bettypeId);

        string GetBettype(int betType, bool liveIndicator);

        string GetBettypeWithProductName(int betType, bool liveIndicator = false);

        string[] GetCombinationTypeCombo(string betTeam, string betCheck, decimal handicap1, decimal handicap2);

        string GetLeagueName(object leagueId);

        string GetSportName(object sportId, string languageCode = null);

        string GetTeamName(object teamId);
    }
}