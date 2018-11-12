namespace Fanex.Athena.DataAccess.BetSettings.Utility
{
    using System.Collections.Generic;
    using System.Data;
    using Common;
    using Model.BetSettings.DTO;
    using Resources;
    using VendorAdapter.Interfaces;

    public class BetSettingReader : IBetSettingReader
    {
        private static readonly IDictionary<string, string> SportNameMap = new Dictionary<string, string>
        {
            { "99MP", nameof(ViewResources.MixSportsParlay) }
        };

        private readonly ICacheRefAdapter _cacheRefAdapter;

        public BetSettingReader(ICacheRefAdapter cacheRefAdapter)
        {
            _cacheRefAdapter = cacheRefAdapter;
        }

        public BetSetting ReadForSport(IDataReader reader, string sportId, bool isNumberGame = false)
        {
            var sportSuffix = sportId != "1" ? "v" + sportId : string.Empty;
            var sportName = _cacheRefAdapter.GetSportName(sportId);

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