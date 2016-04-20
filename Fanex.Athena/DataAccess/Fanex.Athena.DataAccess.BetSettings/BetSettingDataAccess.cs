// -----------------------------------------------------------------------
// <copyright file="BetSettingDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings
{
    using System.Collections.Generic;
    using System.Data;
    using Common;
    using Core;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.BetSettings.Interfaces;
    using Fanex.Athena.DataAccess.Core.Extensions;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.BetSettings;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Data;

    public class BetSettingDataAccess : BaseDataAccess, IBetSettingDataAccess
    {
        public IList<BetSetting> GetSportBookBetSetting(int custId, ref EsportSettingType esportSetting)
        {
            var uspName = "Lrf_CustomerSelEditCustomer";
            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("CustID", DbType.Int32, custId);
                using (var reader = db.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        esportSetting = reader.HasColumn("DisableEsports") ?
                            (EsportSettingType)Converter.ToInt(reader["DisableEsports"])
                            : EsportSettingType.NoneOffering;
                        return RenderSportBookBetSetting(reader);
                    }
                }
            }

            return new List<BetSetting>();
        }

        public IResultInfo UpdateSportsBookBetSetting(CommonBetSetting data, IDictionary<string, object> setting)
        {
            var uspName = "Lrf_CustomerUpdMember_All";

            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("licenseeid", DbType.Int32, data.LicenseeId);
                db.AddInParameter("ip", DbType.String, data.Ip);
                db.AddInParameter("language", DbType.String, data.Language);
                db.AddInParameter("userid", DbType.Int32, data.UserId);
                db.AddInParameter("custid", DbType.Int32, data.CustId);
                foreach (var item in setting)
                {
                    if (item.Key.Contains("UpdateMinMaxBet"))
                    {
                        db.AddInParameter(item.Key, DbType.Boolean, item.Value);
                    }
                    else
                    {
                        db.AddInParameter(item.Key, DbType.Decimal, item.Value);
                    }
                }

                db.AddOutParameter("ErrCode", DbType.String, 600);
                db.AddOutParameter("ErrMsg", DbType.String, 8000);

                db.ExecuteNonQuery();

                return GetResult(db);
            }
        }

        public IResultInfo UpdateEsportStatus(CommonBetSetting setting, bool isDisabled)
        {
            var uspName = "Lrf_CustomerUpdEsportStatus";

            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("ip", DbType.String, setting.Ip);
                db.AddInParameter("Langkey", DbType.String, setting.Language);
                db.AddInParameter("userid", DbType.Int32, setting.UserId);
                db.AddInParameter("custid", DbType.Int32, setting.CustId);
                db.AddInParameter("DisableEsports", DbType.Boolean, isDisabled);
                db.AddOutParameter("ErrCode", DbType.String, 600);
                db.AddOutParameter("ErrMsg", DbType.String, 8000);

                db.ExecuteNonQuery();

                return GetResult(db);
            }
        }

        private IList<BetSetting> RenderSportBookBetSetting(IDataReader reader)
        {
            if (reader.FieldCount == 0)
            {
                return null;
            }

            var sportList = new List<BetSetting>();

            BetSettingHelper.RenderSport(reader, "1", sportList); // Soccer
            BetSettingHelper.RenderSport(reader, "2", sportList); // Basket ball
            BetSettingHelper.RenderSport(reader, "3", sportList); // Football
            BetSettingHelper.RenderSport(reader, "5", sportList); // Tennis
            BetSettingHelper.RenderSport(reader, "8", sportList); // Baseball
            BetSettingHelper.RenderSport(reader, "10", sportList); // Golf
            BetSettingHelper.RenderSport(reader, "11", sportList); // Moto

            // Remove Muay Thai, feature #55015.
            // BetSettingHelper.RenderSport(reader, "44", sportList); // Muay Thai
            BetSettingHelper.RenderSport(reader, "99", sportList); // Other sports

            if (AppSettings.IsShowMixedSport)
            {
                BetSettingHelper.RenderSport(reader, "99MP", sportList);
            }

            BetSettingHelper.RenderSport(reader, "154", sportList); // Horse Racing Fixed Odds

            return sportList;
        }
    }
}