// -----------------------------------------------------------------------
// <copyright file="RacingBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using Common;
    using Core.Exception;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Data;
    using Interfaces;
    using Logging;
    using Model.BetSettings.DTO;

    public class RacingBetSettingDataAccess : BaseDataAccess, IRacingBetSettingDataAccess
    {
        private static readonly IDictionary<string, string> RacingSportSpNameMap = new Dictionary<string, string>
        {
            { "151_31", "Lrf_CustomerUpdMember_MinMaxBet151_31" },
            { "151_41", "Lrf_CustomerUpdMember_MinMaxBet151_41" },
            { "151_51", "Lrf_CustomerUpdMember_MinMaxBet151_51" },
            { "152_41", "Lrf_CustomerUpdMember_MinMaxBet152_41" },
            { "152_51", "Lrf_CustomerUpdMember_MinMaxBet152_51" },
            { "153_51", "Lrf_CustomerUpdMember_MinMaxBet153_51" }
        };

        public IList<BetSetting> GetBetSetting(int custId)
        {
            var uspName = "Lrf_CustomerSelEditRB";

            using (var db = this.GetObjectDb(uspName))
            {
                db.AddInParameter("CustID", DbType.Int32, custId);
                using (var reader = db.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var isAllowedSaToteCurrency = IsAllowedSaToteCurrency(custId);
                        return RenderRacingBetSetting(reader, isAllowedSaToteCurrency);
                    }
                }
            }

            return new List<BetSetting>();
        }

        public IDictionary<string, string> UpdateBetSetting(IDictionary<string, RacingBetSetting> racingBetSettingList)
        {
            var code = new StringBuilder();
            var message = new StringBuilder();

            var result = new Dictionary<string, string>();

            foreach (var racingBetSetting in racingBetSettingList.Where(r => r.Value.IsDataChange))
            {
                var uspName = RacingSportSpNameMap[racingBetSetting.Key];
                RacingUpdatingProcess(uspName, racingBetSetting.Value);

                if (racingBetSetting.Value.ResultInfo.Code != 0)
                {
                    code.Append(";" + racingBetSetting.Value.ResultInfo.Code);
                    message.Append(";" + racingBetSetting.Value.ResultInfo.Message);
                }
            }

            result.Add("Code", code.ToString());
            result.Add("Message", message.ToString());

            return result;
        }

        private static IList<BetSetting> RenderRacingBetSetting(IDataReader reader, bool isAllowSaToteCurrency)
        {
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

        private bool IsAllowedSaToteCurrency(int custId)
        {
            const string UspName = "Lrf_CheckHRAllowedCurrency";

            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("custid", DbType.Int32, custId);

                    db.AddOutParameter("allowed", DbType.Boolean, sizeof(bool));

                    db.ExecuteNonQuery();

                    return Converter.ToBool(db.GetParameterValue("allowed"));
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, new { });
            }
        }

        private void RacingUpdatingProcess(string uspName, CommonBetSetting racingBetSetting)
        {
            try
            {
                using (var db = this.GetObjectDb(uspName))
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
    }
}