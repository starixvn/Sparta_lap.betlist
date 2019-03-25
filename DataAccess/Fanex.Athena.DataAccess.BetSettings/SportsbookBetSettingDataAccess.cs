// -----------------------------------------------------------------------
// <copyright file="SportsbookBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings
{
    using System.Collections.Generic;
    using System.Data;
    using Constant.DataDomain;
    using Core;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.BetSettings.Interfaces;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Model.BetSettings.ViewModel;
    using Utility;

    public class SportsbookBetSettingDataAccess : BaseDataAccess, ISportsbookBetSettingDataAccess
    {
        private const string MixedSportId = "99MP";

        private readonly IAppSettings _appSettings;

        private readonly IBetSettingReader _betSettingReader;

        public SportsbookBetSettingDataAccess(
            IAppSettings appSetting,
            IBetSettingReader betSettingReader)
        {
            _appSettings = appSetting;
            _betSettingReader = betSettingReader;
        }

        public BetSettingViewModel GetBetSetting(int custId)
        {
            var uspName = "Lrf_CustomerSelEditCustomer";
            using (var db = this.GetObjectDb(uspName))
            {
                db.SetParameterValue("CustID", custId);
                using (var reader = db.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var dataList = GetBetSettings(reader, _appSettings.IsShowMixedSport);
                        return new BetSettingViewModel
                        {
                            MainData = dataList
                        };
                    }
                }
            }

            return new BetSettingViewModel
            {
                MainData = new List<BetSetting>()
            };
        }

        public IResultInfo UpdateBetSetting(CommonBetSetting data, IDictionary<string, object> setting)
        {
            var uspName = "Lrf_CustomerUpdMember_All";

            using (var db = this.GetObjectDb(uspName))
            {
                db.SetParameterValue("licenseeid", data.LicenseeId);
                db.SetParameterValue("ip", data.Ip);
                db.SetParameterValue("language", data.Language);
                db.SetParameterValue("userid", data.UserId);
                db.SetParameterValue("custid", data.CustId);
                foreach (var item in setting)
                {
                    db.SetParameterValue(item.Key, item.Key.Contains("UpdateMinMaxBet") ? DbType.Boolean : DbType.Decimal, item.Value);
                }

                db.SetParameterValue("ErrCode", null);
                db.SetParameterValue("ErrMsg", null);

                db.ExecuteNonQuery();

                return GetResult(db);
            }
        }

        private IList<BetSetting> GetBetSettings(IDataReader reader, bool isShowMixedSport)
        {
            var sportList = new List<BetSetting>();

            sportList.Add(_betSettingReader.ReadForSport(reader, ((int)SportType.Soccer).ToString()));
            sportList.Add(_betSettingReader.ReadForSport(reader, ((int)SportType.BasketBall).ToString()));
            sportList.Add(_betSettingReader.ReadForSport(reader, ((int)SportType.FootBall).ToString()));
            sportList.Add(_betSettingReader.ReadForSport(reader, ((int)SportType.Tennis).ToString()));
            sportList.Add(_betSettingReader.ReadForSport(reader, ((int)SportType.BaseBall).ToString()));
            sportList.Add(_betSettingReader.ReadForSport(reader, ((int)SportType.Golf).ToString()));
            sportList.Add(_betSettingReader.ReadForSport(reader, ((int)SportType.Motorsports).ToString()));
            sportList.Add(_betSettingReader.ReadForSport(reader, ((int)SportType.OtherSports).ToString()));
            if (isShowMixedSport)
            {
                sportList.Add(_betSettingReader.ReadForSport(reader, MixedSportId));
            }

            return sportList;
        }
    }
}