// --------------------------------------------------------------------------
// <copyright file="CacheRef.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------
namespace Fanex.Athena.Models.Cache
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Threading;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.ViewModel;
    using RemotingRef;

    /// <summary>
    /// CacheRef class.
    /// </summary>
    public sealed class CacheRef
    {
        private static IDictionary<string, string> _mapLanguages  = new Dictionary<string, string>()
            {
                { "en-US", "en" },
                { "zh-TW", "ch" },
                { "zh-CN", "cs" },
                { "zh-Hans", "zhcn" },
                { "ja-JP", "jp" },
                { "th-TH", "th" },
                { "ko-KR", "ko" },
                { "vi-VN", "vn" }
            };

        private CacheRef()
        {
        }

        public static string GetBettype(object bettypeId)
        {
            return RefData.GetBetTypeName(bettypeId.ToString(), GetCurrentIBCLanguageCode(), string.Empty);
        }

        public static string GetSportName(object sportId)
        {
            return RefData.GetSportName(sportId.ToString(), GetCurrentIBCLanguageCode(), string.Empty);
        }

        public static string GetTeamName(object teamId)
        {
            return RefData.GetTeamName(teamId.ToString(), GetCurrentIBCLanguageCode(), string.Empty);
        }

        public static string GetLeagueName(object leagueId)
        {
            return RefData.GetLeagueName(leagueId.ToString(), GetCurrentIBCLanguageCode(), string.Empty);
        }

        public static string GetFncIndexName(object indexId)
        {
            return RefData.GetFncIndexName(indexId.ToString(), GetCurrentIBCLanguageCode());
        }

        public static string GetHorseTeamName(object codeId)
        {
            return RefData.GetHorseTeamName(codeId.ToString(), GetCurrentIBCLanguageCode());
        }

        public static string GetOddsTypeName(object oddsTypeId)
        {
            return RefData.GetOddsTypeName(oddsTypeId.ToString(), GetCurrentIBCLanguageCode());
        }

        public static string GetCasinoBetItem(object betTeam)
        {
            var betTeamString = betTeam == null ? string.Empty : betTeam.ToString();
            return RefData.GetCasinoBetItem(betTeamString, GetCurrentIBCLanguageCode());
        }

        public static string GetRouletteBetItem(object betTeam)
        {
            var betTeamString = betTeam == null ? string.Empty : betTeam.ToString();
            return RefData.GetRouletteBetItem(betTeamString, GetCurrentIBCLanguageCode());
        }

        /// <summary>
        /// Get bet type text by bet type and live indicator.
        /// </summary>
        /// <param name="betType">Bet type value.</param>
        /// <param name="liveIndicator">Live or NonLive.</param>
        /// <returns>Bet type text.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan",
                    Justification = "Reviewed. Suppression is OK here.")]
        public static string GetBettype(int betType, bool liveIndicator)
        {
            const int SBMinOfMaxBettype = 120;

            var betTypeName = RefData.GetBetTypeName(betType.ToString(CultureInfo.InvariantCulture), GetCurrentIBCLanguageCode(), string.Empty);

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
            }

            return betTypeName;
        }

        private static string GetCurrentIBCLanguageCode()
        {
            var languageCode = Thread.CurrentThread.CurrentUICulture.IetfLanguageTag;
            string result;
            if (!_mapLanguages.TryGetValue(languageCode, out result))
            {
                return "en";
            }

            return result;
        }
    }
}