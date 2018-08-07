using System;
using System.Globalization;
using System.Threading;
using Fanex.Athena.Constant.DataDomain;
using Fanex.Athena.Constant.Helper;
using Fanex.Athena.Extension.Utility;
using Fanex.Athena.Resources;
using Fanex.Athena.VendorAdapter.Interfaces;
using Fanex.RemotingRef;
using RemotingRef;
using BetListCacheRef = Fanex.BetList.Core.TicketHelper.CacheRef;

namespace Fanex.Athena.VendorAdapter
{
    public class CacheRefAdapter : ICacheRefAdapter
    {
        private readonly IRemotingRefService _remotingRefService;

        private const string MicrogamingResourceKey = "MG_Casino";

        private static CacheRefAdapter _instance;

        public CacheRefAdapter(IRemotingRefService remotingRefService)
        {
            _remotingRefService = remotingRefService ?? new RemotingRefService();
        }
        public CacheRefAdapter() : this(null)
        {

        }
        public static CacheRefAdapter Instance
        {
            get
            {
                return _instance ?? (_instance = new CacheRefAdapter());
            }
        }

        public string GetMicroGamingGameName(string resourceId)
        {
            return _remotingRefService.GetResourceDataByRefIdAndResourceId(MicrogamingResourceKey, resourceId, GetCurrentIBCLanguageCode(), string.Empty);
        }

        public string GetBettype(object bettypeId)
        {
            return _remotingRefService.GetBetTypeName(bettypeId.ToString(), GetCurrentIBCLanguageCode());
        }

        public string GetSportName(object sportId, string languageCode = null)
        {
            return _remotingRefService.GetSportName(sportId.ToString(), GetCurrentIBCLanguageCode(languageCode), string.Empty);
        }

        public string GetTeamName(object teamId)
        {
            return _remotingRefService.GetTeamName(teamId.ToString(), GetCurrentIBCLanguageCode(), string.Empty);
        }

        public string GetLeagueName(object leagueId)
        {
            return _remotingRefService.GetLeagueName(leagueId.ToString(), GetCurrentIBCLanguageCode(), string.Empty);
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

            var betTypeName = _remotingRefService.GetBetTypeName(betType.ToString(CultureInfo.InvariantCulture), GetCurrentIBCLanguageCode());

            if (betType == (int)BetType.NgNumberGroup)
            {
                betTypeName += liveIndicator ? " (" + ViewResources.LiveGame + ")" : " (" + ViewResources.PreGame + ")";
                return betTypeName;
            }

            if (betType >= (int)BetType.NG1StBallOU && betType < SBMinOfMaxBettype)
            {
                if ((betType == (int)BetType.NgOuFt || betType == (int)BetType.NgOeFt) && liveIndicator)
                {
                    betTypeName = ViewResources.Next + " " + betTypeName;
                }

                if (betType == (int)BetType.NgOeFt && !liveIndicator)
                {
                    betTypeName = ViewResources.FT + " " + betTypeName;
                }

                return betTypeName;
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
            return _remotingRefService.GetCombinationTypeCombo(betTeam, betCheck, Convert.ToInt32(handicap1), Convert.ToInt32(handicap2), GetCurrentIBCLanguageCode());
        }

        public string GetOddsTypeName(object oddsTypeId)
        {
            return BetListCacheRef.GetOddsTypeName(oddsTypeId);
        }

        private static string GetCurrentIBCLanguageCode(string languageCode = null)
        {
            var languageKey = string.IsNullOrEmpty(languageCode) ? Thread.CurrentThread.CurrentUICulture.IetfLanguageTag : languageCode;

            if (Util.MapLanguages.ContainsKey(languageKey))
            {
                return Util.MapLanguages[languageKey];
            }

            return "en";
        }
    }
}