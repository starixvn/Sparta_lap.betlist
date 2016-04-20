namespace Fanex.Athena.DataAccess.BetSettings
{
    using System.Collections.Generic;
    using System.Data;
    using Common;
    using Fanex.Athena.Models.Cache;
    using Fanex.Athena.Models.DTO.BetSettings;
    using Resources;

    public class BetSettingHelper
    {
        public static void RenderSport(IDataReader reader, string sportId, List<BetSetting> resultList, bool isNumberGame = false)
        {
            var sportSuffix = sportId != "1" ? "v" + sportId : string.Empty;
            var sportName = CacheRef.GetSportName(sportId);

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
            var sportName = string.Empty;

            switch (sportId)
            {
                case "99MP":
                    sportName = ViewResources.MixSportsParlay;
                    break;

                case "151_31":
                    sportName = ViewResources.HorseRacingSATote;
                    break;

                case "151_41":
                    sportName = ViewResources.HorseRacingUK_IrishTote;
                    break;

                case "151_51":
                    sportName = ViewResources.HorseRacingAustTote;
                    break;

                case "152_41":
                    sportName = ViewResources.GreyhoundUK_IrishTote;
                    break;

                case "152_51":
                    sportName = ViewResources.GreyhoundAustTote;
                    break;

                case "153_51":
                    sportName = ViewResources.HarnessAustTote;
                    break;

                default:
                    sportName = ViewResources.OtherSport;
                    break;
            }

            return sportName;
        }
    }
}