// -----------------------------------------------------------------------
// <copyright file="RacingBetSettingDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core.Exception;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Data;
    using Interfaces;
    using Logging;
    using Models.DTO.BetSettings;

    public class RacingBetSettingDataAccess : BaseDataAccess, IRacingBetSettingDataAccess
    {
        public IList<BetSetting> GetBetSetting(int custId, bool isAllowSaToreCurrency)
        {
            var uspName = "Lrf_CustomerSelEditRB";

            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("CustID", DbType.Int32, custId);
                using (var reader = db.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return RenderRacingBetSetting(reader, isAllowSaToreCurrency);
                    }
                }
            }

            return new List<BetSetting>();
        }

        public IDictionary<string, string> UpdateBetSetting(Dictionary<string, RacingBetSetting> racingBetSettingList)
        {
            var code = string.Empty;
            var message = string.Empty;

            var result = new Dictionary<string, string>();

            foreach (var racingBetSetting in racingBetSettingList.Where(r => r.Value.IsDataChange))
            {
                switch (racingBetSetting.Key)
                {
                    case "151_31":
                        Update15131(racingBetSetting.Value);
                        break;

                    case "151_41":
                        Update15141(racingBetSetting.Value);
                        break;

                    case "151_51":
                        Update15151(racingBetSetting.Value);
                        break;

                    case "152_41":
                        Update15241(racingBetSetting.Value);
                        break;

                    case "152_51":
                        Update15251(racingBetSetting.Value);
                        break;

                    case "153_51":
                        Update15351(racingBetSetting.Value);
                        break;
                }

                if (racingBetSetting.Value.ResultInfo.Code == 0)
                {
                    continue;
                }

                code += ";" + racingBetSetting.Value.ResultInfo.Code;
                message += ";" + racingBetSetting.Value.ResultInfo.Message;
            }

            result.Add("Code", code);
            result.Add("Message", message);

            return result;
        }

        private static void Update15131(CommonBetSetting racingBetSetting)
        {
            var uspName = "Lrf_CustomerUpdMember_MinMaxBet151_31";

            RacingUpdatingProcess(uspName, racingBetSetting);
        }

        private static void Update15141(CommonBetSetting racingBetSetting)
        {
            var uspName = "Lrf_CustomerUpdMember_MinMaxBet151_41";

            RacingUpdatingProcess(uspName, racingBetSetting);
        }

        private static void Update15151(CommonBetSetting racingBetSetting)
        {
            var uspName = "Lrf_CustomerUpdMember_MinMaxBet151_51";

            RacingUpdatingProcess(uspName, racingBetSetting);
        }

        private static void Update15241(CommonBetSetting racingBetSetting)
        {
            var uspName = "Lrf_CustomerUpdMember_MinMaxBet152_41";

            RacingUpdatingProcess(uspName, racingBetSetting);
        }

        private static void Update15251(CommonBetSetting racingBetSetting)
        {
            var uspName = "Lrf_CustomerUpdMember_MinMaxBet152_51";

            RacingUpdatingProcess(uspName, racingBetSetting);
        }

        private static void Update15351(CommonBetSetting racingBetSetting)
        {
            var uspName = "Lrf_CustomerUpdMember_MinMaxBet153_51";

            RacingUpdatingProcess(uspName, racingBetSetting);
        }

        private static void RacingUpdatingProcess(string uspName, CommonBetSetting racingBetSetting)
        {
            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    db.AddInParameter("custid", DbType.Int32, racingBetSetting.CustId);
                    db.AddInParameter("userid", DbType.Int32, racingBetSetting.UserId);
                    db.AddInParameter("minbet", DbType.Decimal, racingBetSetting.MinBet);
                    db.AddInParameter("maxbet", DbType.Decimal, racingBetSetting.MaxBet);
                    db.AddInParameter("maxpermatch", DbType.Decimal, racingBetSetting.MaxPerMatch);
                    db.AddInParameter("language", DbType.String, racingBetSetting.Language);
                    db.AddInParameter("ip", DbType.String, racingBetSetting.Ip);
                    AddResultParam(db);
                    db.ExecuteNonQuery();

                    var result = GetResult(db);

                    racingBetSetting.ResultInfo = (ResultInfo)result;
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName, "No params");
            }
        }

        private static IList<BetSetting> RenderRacingBetSetting(IDataReader reader, bool isAllowSaToteCurrency)
        {
            if (reader.FieldCount == 0)
            {
                return null;
            }

            var racingList = new List<BetSetting>();

            BetSettingHelper.RenderSport(reader, "151_41", racingList); // Horse racing UK

            if (isAllowSaToteCurrency)
            {
                BetSettingHelper.RenderSport(reader, "151_31", racingList); // Horse racing SA
            }

            BetSettingHelper.RenderSport(reader, "152_51", racingList); // Greyhound Aust
            BetSettingHelper.RenderSport(reader, "153_51", racingList); // Harness Aust

            return racingList;
        }
    }
}