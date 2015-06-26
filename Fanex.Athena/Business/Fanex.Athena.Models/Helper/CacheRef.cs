// --------------------------------------------------------------------------
// <copyright file="CacheRef.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------
namespace Fanex.Athena.Models.Cache
{
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
        private CacheRef()
        {
        }

        public static string GetBettype(object bettypeId)
        {
            return RefData.GetBetTypeName(bettypeId.ToString(), GetCurrentLanguage(), string.Empty);
        }

        public static string GetSportName(object sportId)
        {
            return RefData.GetSportName(sportId.ToString(), GetCurrentLanguage(), string.Empty);
        }

        public static string GetTeamName(object teamId)
        {
            return RefData.GetTeamName(teamId.ToString(), GetCurrentLanguage(), string.Empty);
        }

        public static string GetLeagueName(object leagueId)
        {
            return RefData.GetLeagueName(leagueId.ToString(), GetCurrentLanguage(), string.Empty);
        }

        public static string GetFncIndexName(object indexId)
        {
            return RefData.GetFncIndexName(indexId.ToString(), GetCurrentLanguage());
        }

        public static string GetHorseTeamName(object codeId)
        {
            return RefData.GetHorseTeamName(codeId.ToString(), GetCurrentLanguage());
        }

        public static string GetOddsTypeName(object oddsTypeId)
        {
            return RefData.GetOddsTypeName(oddsTypeId.ToString(), GetCurrentLanguage());
        }

        public static string GetCasinoBetItem(object betTeam)
        {
            var betTeamString = betTeam == null ? string.Empty : betTeam.ToString();
            return RefData.GetCasinoBetItem(betTeamString, GetCurrentLanguage());
        }

        public static string GetRouletteBetItem(object betTeam)
        {
            var betTeamString = betTeam == null ? string.Empty : betTeam.ToString();
            return RefData.GetRouletteBetItem(betTeamString, GetCurrentLanguage());
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

            var betTypeName = RefData.GetBetTypeName(betType.ToString(CultureInfo.InvariantCulture), GetCurrentLanguage(), string.Empty);
            
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

        /// <summary>
        /// Get current language in IBC format.
        /// </summary>
        /// <returns>Language in IBC format.</returns>
        private static string GetCurrentLanguage()
        {
            var current = Thread.CurrentThread.CurrentUICulture.IetfLanguageTag;
            switch (current)
            {
                case "en-US": return "en";
                case "zh-TW": return "ch"; // Traditional
                case "zh-CN": return "cs"; // Simplified
                case "ja-JP": return "jp";
                case "th-TH": return "th"; // Thai
                case "ko-KR": return "ko"; // Korean
                case "vi-VN": return "vn"; // Vietnamese
                default: return "en";
            }
        }
    }
}