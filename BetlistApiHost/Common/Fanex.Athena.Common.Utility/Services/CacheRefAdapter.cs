using System;
using System.Globalization;
using System.Threading;
using Fanex.Athena.Common.Constant;
using Fanex.Athena.Common.Constant.Helpers;
using Fanex.Athena.Common.Utility.Services.Interfaces;
using Fanex.Athena.Resources;
using Fanex.RemotingRef;

namespace Fanex.Athena.Common.Utility.Services
{
    public class CacheRefAdapter : ICacheRefAdapter
    {
        private const string MicrogamingResourceKey = "MG_Casino";

        private readonly Lazy<IRemotingRefService> _remotingRefService = new Lazy<IRemotingRefService>(() => new RemotingRefService());

        public static CacheRefAdapter Instance { get; } = new CacheRefAdapter();

#pragma warning disable CS3003 // Type is not CLS-compliant
        public IRemotingRefService RemotingRefService => _remotingRefService.Value;
#pragma warning restore CS3003 // Type is not CLS-compliant

        public string GetMicroGamingGameName(string resourceId)
        {
            if (!IsValidId(resourceId))
            {
                return string.Empty;
            }

            return RemotingRefService.GetResourceDataByRefIdAndResourceId(MicrogamingResourceKey, resourceId, GetCurrentIBCLanguageCode(), string.Empty);
        }

        public string GetBettype(object bettypeId)
        {
            if (!IsValidId(bettypeId))
            {
                return string.Empty;
            }

            return RemotingRefService.GetBetTypeName(bettypeId.ToString(), GetCurrentIBCLanguageCode());
        }

        public string GetSportName(object sportId, string languageCode = null)
        {
            if (!IsValidId(sportId))
            {
                return string.Empty;
            }

            return RemotingRefService.GetSportName(sportId.ToString(), GetCurrentIBCLanguageCode(languageCode), string.Empty);
        }

        public string GetTeamName(object teamId)
        {
            if (!IsValidId(teamId))
            {
                return string.Empty;
            }

            return RemotingRefService.GetTeamName(teamId.ToString(), GetCurrentIBCLanguageCode(), string.Empty);
        }

        public string GetLeagueName(object leagueId)
        {
            if (!IsValidId(leagueId))
            {
                return string.Empty;
            }

            return RemotingRefService.GetLeagueName(leagueId.ToString(), GetCurrentIBCLanguageCode(), string.Empty);
        }

        /// <summary>
        /// Get bet type text by bet type and live indicator.
        /// </summary>
        /// <param name="betType">Bet type value.</param>
        /// <param name="liveIndicator">Live or NonLive.</param>
        /// <returns>Bet type text.</returns>
        public string GetBettype(int betType, bool liveIndicator)
        {
            const int SBMinOfMaxBettype = 120;
            if (betType == 0)
            {
                return string.Empty;
            }

            var betTypeName = RemotingRefService.GetBetTypeName(betType.ToString(CultureInfo.InvariantCulture), GetCurrentIBCLanguageCode());

            if (betType == (int)BetType.NgNumberGroup)
            {
                betTypeName += liveIndicator ? " (" + ViewResources.LiveGame + ")" : " (" + ViewResources.PreGame + ")";
                return betTypeName;
            }

            if (betType >= (int)BetType.NG1StBallOU && betType < SBMinOfMaxBettype)
            {
                return GetNumberGameBettypeName(betType, liveIndicator, betTypeName);
            }

            return betTypeName;
        }

        public string GetBettypeWithProductName(int betType, bool liveIndicator = false)
        {
            var betTypeName = GetBettype(betType, liveIndicator);

            if (BetTypeHelper.IsVirtualSports2(betType))
            {
                return betTypeName + " (" + ViewResources.ProductVS2 + ")";
            }

            return betTypeName;
        }

        public string[] GetCombinationTypeCombo(string betTeam, string betCheck, decimal handicap1, decimal handicap2)
        {
            return RemotingRefService.GetCombinationTypeCombo(betTeam, betCheck, Convert.ToInt32(handicap1), Convert.ToInt32(handicap2), GetCurrentIBCLanguageCode());
        }

        private static string GetNumberGameBettypeName(int betType, bool liveIndicator, string betTypeName)
        {
            if ((betType == (int)BetType.NgOuFt || betType == (int)BetType.NgOeFt) && liveIndicator)
            {
                return ViewResources.Next + " " + betTypeName;
            }

            if (betType == (int)BetType.NgOeFt)
            {
                return ViewResources.FT + " " + betTypeName;
            }

            return betTypeName;
        }

        private static bool IsValidId(object id)
        {
            return id != null && id.ToString() != string.Empty && id.ToString() != "0";
        }

        private static string GetCurrentIBCLanguageCode(string languageCode = null)
        {
            return string.IsNullOrEmpty(languageCode) ? Thread.CurrentThread.CurrentUICulture.IetfLanguageTag : languageCode;
        }
    }
}