using Fanex.RemotingRef;

namespace Fanex.Athena.Common.Utility.Services.Interfaces
{
    public interface ICacheRefAdapter
    {
#pragma warning disable CS3003 // Type is not CLS-compliant
        IRemotingRefService RemotingRefService { get; }
#pragma warning restore CS3003 // Type is not CLS-compliant

        string[] GetCombinationTypeCombo(string betTeam, string betCheck, decimal handicap1, decimal handicap2);

        string GetLeagueName(object leagueId);

        string GetSportName(object sportId, string languageCode = null);
    }
}