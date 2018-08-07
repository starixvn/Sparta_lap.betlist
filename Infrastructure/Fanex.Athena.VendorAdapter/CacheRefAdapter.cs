using System;
using System.Globalization;
using System.Threading;
using Fanex.Athena.Constant.DataDomain;
using Fanex.Athena.Constant.Helper;
using Fanex.Athena.Extension.Utility;
using Fanex.Athena.Resources;
using Fanex.Athena.VendorAdapter.Interfaces;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.TicketHelper;
using Fanex.RemotingRef;

namespace Fanex.Athena.VendorAdapter
{
    public class CacheRefAdapter : ICacheRefAdapter
    {
        private const string MicrogamingResourceKey = "MG_Casino";
        private readonly Lazy<IRemotingRefService> _remotingRefService = new Lazy<IRemotingRefService>(() => new RemotingRefService());
        private readonly Lazy<ITicketHelper> _ticketHelper = new Lazy<ITicketHelper>(() => new TicketHelper());
#pragma warning disable CS3003 // Type is not CLS-compliant
        public ITicketHelper TicketHelper => _ticketHelper.Value;
#pragma warning restore CS3003 // Type is not CLS-compliant
#pragma warning disable CS3003 // Type is not CLS-compliant
        public IRemotingRefService RemotingRefService => _remotingRefService.Value;
#pragma warning restore CS3003 // Type is not CLS-compliant

        /// <summary>
        /// Thread safe
        /// </summary>
        public static CacheRefAdapter Instance { get; } = new CacheRefAdapter();

        public string GetMicroGamingGameName(string resourceId)
        {
            return RemotingRefService.GetResourceDataByRefIdAndResourceId(MicrogamingResourceKey, resourceId, GetCurrentIBCLanguageCode(), string.Empty);
        }

        public string GetBettype(object bettypeId)
        {
            return RemotingRefService.GetBetTypeName(bettypeId.ToString(), GetCurrentIBCLanguageCode());
        }

        public string GetSportName(object sportId, string languageCode = null)
        {
            return RemotingRefService.GetSportName(sportId.ToString(), GetCurrentIBCLanguageCode(languageCode), string.Empty);
        }

        public string GetTeamName(object teamId)
        {
            return RemotingRefService.GetTeamName(teamId.ToString(), GetCurrentIBCLanguageCode(), string.Empty);
        }

        public string GetLeagueName(object leagueId)
        {
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

            var betTypeName = RemotingRefService.GetBetTypeName(betType.ToString(CultureInfo.InvariantCulture), GetCurrentIBCLanguageCode());

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
            return RemotingRefService.GetCombinationTypeCombo(betTeam, betCheck, Convert.ToInt32(handicap1), Convert.ToInt32(handicap2), GetCurrentIBCLanguageCode());
        }

        public string GetOddsTypeName(object oddsTypeId)
        {
            return TicketHelper.GetOddsTypeNameById(oddsTypeId);
        }

        private static string GetCurrentIBCLanguageCode(string languageCode = null)
        {
            return string.IsNullOrEmpty(languageCode) ? Thread.CurrentThread.CurrentUICulture.IetfLanguageTag : languageCode;
        }
    }
}