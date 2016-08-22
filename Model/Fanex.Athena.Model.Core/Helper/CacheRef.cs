// --------------------------------------------------------------------------
// <copyright file="CacheRef.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------
namespace Fanex.Athena.Model.Core.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Threading;
    using Fanex.Athena.Model.Core.DataDomain;
    using RemotingRef;
    using Resources;

    /// <summary>
    /// CacheRef class.
    /// </summary>
    public sealed class CacheRef
    {
        private const string MicrogamingResourceKey = "MG_Casino";

        private static IDictionary<string, string> _mapLanguages = new Dictionary<string, string>()
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

        public static string GetMicroGamingGameName(string resourceId)
        {
            return RefData.GetResourceDataByRefIdAndResourceId(MicrogamingResourceKey, resourceId, GetCurrentIBCLanguageCode(), string.Empty);
        }

        public static string GetBettype(object bettypeId)
        {
            return RefData.GetBetTypeName(bettypeId.ToString(), GetCurrentIBCLanguageCode(), string.Empty);
        }

        public static string GetSportName(object sportId, string languageCode = null)
        {
            return RefData.GetSportName(sportId.ToString(), GetCurrentIBCLanguageCode(languageCode), string.Empty);
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
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
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

        public static string[] GetCombinationTypeCombo(string betTeam, string betCheck, decimal handicap1, decimal handicap2)
        {
            return RefData.GetCombinationTypeCombo(betTeam, betCheck, Convert.ToInt32(handicap1), Convert.ToInt32(handicap2), GetCurrentIBCLanguageCode());
        }

        private static string GetCurrentIBCLanguageCode(string languageCode = null)
        {
            if (string.IsNullOrEmpty(languageCode))
            {
                languageCode = Thread.CurrentThread.CurrentUICulture.IetfLanguageTag;
            }

            if (_mapLanguages.ContainsKey(languageCode))
            {
                return _mapLanguages[languageCode];
            }

            return "en";
        }
    }
}