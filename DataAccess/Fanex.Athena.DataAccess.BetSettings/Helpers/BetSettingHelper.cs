namespace Fanex.Athena.DataAccess.BetSettings.Helpers
{
    using System.Collections.Generic;
    using System.Data;
    using Common;
    using Model.BetSettings.DTO;
    using Resources;
    using VendorAdapter;

    public static class BetSettingHelper
    {
        private static readonly IDictionary<string, string> SportNameMap = new Dictionary<string, string>
        {
            { "99MP", nameof(ViewResources.MixSportsParlay) }
        };

        public static BetSetting RenderSport(IDataReader reader, string sportId, bool isNumberGame = false)
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

            return sport;
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