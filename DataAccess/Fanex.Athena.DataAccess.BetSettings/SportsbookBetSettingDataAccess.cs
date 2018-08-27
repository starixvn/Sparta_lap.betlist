// -----------------------------------------------------------------------
// <copyright file="SportsbookBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings
{
    using System.Collections.Generic;
    using System.Data;
    using Core;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.BetSettings.Interfaces;
    using Fanex.Athena.DataAccess.Core.Extensions;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Helpers;
    using Model.BetSettings.ViewModel;

    public class SportsbookBetSettingDataAccess : BaseDataAccess, ISportsbookBetSettingDataAccess
    {
        private readonly IAppSettings _appSettings;

        public SportsbookBetSettingDataAccess()
        {
            _appSettings = new AppSettings();
        }

        public SportsbookBetSettingDataAccess(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public BetSettingViewModel GetBetSetting(int custId)
        {
            var uspName = "Lrf_CustomerSelEditCustomer";
            using (var db = this.GetObjectDb(uspName))
            {
                db.AddInParameter("CustID", DbType.Int32, custId);
                using (var reader = db.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var esportSetting = reader.HasColumn("DisableEsports") ?
                            (EsportSettingType)System.Convert.ToInt32(reader["DisableEsports"])
                            : EsportSettingType.NoneOffering;

                        var dataList = RenderSportBookBetSetting(reader, _appSettings.IsShowMixedSport);
                        return new BetSettingViewModel
                        {
                            MainData = dataList,
                            EsportSetting = esportSetting
                        };
                    }
                }
            }

            return new BetSettingViewModel
            {
                MainData = new List<BetSetting>(),
                EsportSetting = EsportSettingType.NoneOffering
            };
        }

        public IResultInfo UpdateBetSetting(CommonBetSetting data, IDictionary<string, object> setting)
        {
            var uspName = "Lrf_CustomerUpdMember_All";

            using (var db = this.GetObjectDb(uspName))
            {
                db.AddInParameter("licenseeid", DbType.Int32, data.LicenseeId);
                db.AddInParameter("ip", DbType.String, data.Ip);
                db.AddInParameter("language", DbType.String, data.Language);
                db.AddInParameter("userid", DbType.Int32, data.UserId);
                db.AddInParameter("custid", DbType.Int32, data.CustId);
                foreach (var item in setting)
                {
                    db.AddInParameter(item.Key, item.Key.Contains("UpdateMinMaxBet") ? DbType.Boolean : DbType.Decimal, item.Value);
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

            using (var db = this.GetObjectDb(uspName))
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

        private static IList<BetSetting> RenderSportBookBetSetting(IDataReader reader, bool isShowMixedSport)
        {
            var sportList = new List<BetSetting>();

            sportList.Add(BetSettingHelper.RenderSport(reader, "1")); // Soccer
            sportList.Add(BetSettingHelper.RenderSport(reader, "2")); // Basket ball
            sportList.Add(BetSettingHelper.RenderSport(reader, "3")); // Football
            sportList.Add(BetSettingHelper.RenderSport(reader, "5")); // Tennis
            sportList.Add(BetSettingHelper.RenderSport(reader, "8")); // Baseball
            sportList.Add(BetSettingHelper.RenderSport(reader, "10")); // Golf
            sportList.Add(BetSettingHelper.RenderSport(reader, "11")); // Moto

            // Remove Muay Thai, feature #55015.
            // BetSettingHelper.RenderSport(reader, "44", sportList); // Muay Thai
            sportList.Add(BetSettingHelper.RenderSport(reader, "99")); // Other sports

            if (isShowMixedSport)
            {
                sportList.Add(BetSettingHelper.RenderSport(reader, "99MP"));
            }

            sportList.Add(BetSettingHelper.RenderSport(reader, "154")); // Horse Racing Fixed Odds

            return sportList;
        }
    }
}