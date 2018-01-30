namespace Fanex.Athena.DataAccess.BetSettings.Helpers
{
    using System.Collections.Generic;
    using System.Data;
    using Common;
    using Model.BetSettings.DTO;
    using Model.Core.Helper;
    using Resources;
    using VendorAdapter;

    public static class BetSettingHelper
    {
        private static readonly IDictionary<string, string> SportNameMap = new Dictionary<string, string>
        {
            { "99MP", nameof(ViewResources.MixSportsParlay) },
            { "151_31", nameof(ViewResources.HorseRacingSATote) },
            { "151_41", nameof(ViewResources.HorseRacingUK_IrishTote) },
            { "151_51", nameof(ViewResources.HorseRacingAustTote) },
            { "152_41", nameof(ViewResources.GreyhoundUK_IrishTote) },
            { "152_51", nameof(ViewResources.GreyhoundAustTote) },
            { "153_51", nameof(ViewResources.HarnessAustTote) }
        };

        public static void RenderSport(IDataReader reader, string sportId, ICollection<BetSetting> resultList, bool isNumberGame = false)
        {
            var sportSuffix = sportId != "1" ? "v" + sportId : string.Empty;
            var sportName = CacheRefAdapter.Instance.GetSportName(sportId);

            sportName = string.IsNullOrEmpty(sportName) ? GetSportName(sportId) : sportName;

            var sport = new BetSetting
            {
                MaxBet = Converter.ToDecimal(reader["maxbet" + sportSuffix]),
                MinBet = Converter.ToDecimal(reader["minbet" + sportSuffix]),
                MaxPerMatch = Converter.ToDecimal(reader["MaxPerMatch" + sportSuffix]),
                MaxPerMatchMax = Converter.ToDecimal(reader["MaxPerMatch" + sportSuffix + "_Max"]),
                MaxPerMatchMin = Converter.ToDecimal(reader["MaxPerMatch" + sportSuffix + "_Min"]),
                BetValueMax = Converter.ToDecimal(reader["MaxBet" + sportSuffix + "_Min"]),
                BetValueMin = Converter.ToDecimal(reader["MinBet" + sportSuffix + "_Max"]),
                BetSettingType = sportName,
                SportId = sportId
            };

            if (isNumberGame)
            {
                sport.MaxPerBall = Converter.ToDecimal(reader["maxperball" + sportSuffix]);
                sport.MaxPerBallMax = Converter.ToDecimal(reader["maxperball" + sportSuffix + "_Max"]);
            }

            resultList.Add(sport);
        }

        private static string GetSportName(string sportId)
        {
            if (!SportNameMap.ContainsKey(sportId))
            {
                return ViewResources.OtherSport;
            }

            var resourceKey = SportNameMap[sportId];
            return ViewResources.ResourceManager.GetString(resourceKey);
        }
    }
}