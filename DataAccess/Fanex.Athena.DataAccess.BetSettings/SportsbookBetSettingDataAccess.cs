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
    using Fanex.Athena.DataAccess.Core.Extensions;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Model.BetSettings.ViewModel;
    using Utility;
    using Session.Interfaces;

    public class SportsbookBetSettingDataAccess : BaseDataAccess, ISportsbookBetSettingDataAccess
    {
        private const string MixedSportId = "99MP";

        private readonly IAppSettings _appSettings;

        private readonly IBetSettingReader _betSettingReader;

        public SportsbookBetSettingDataAccess()
        public SportsbookBetSettingDataAccess(IAppSettings appSetting, ISessionStorage sessionStorage)
            : base(appSetting, sessionStorage)
        {
            _appSettings = appSetting;
            _betSettingReader = new BetSettingReader();
        }

        public SportsbookBetSettingDataAccess(IAppSettings appSettings, IBetSettingReader betSettingReader)
        {
            _appSettings = appSettings;
            _betSettingReader = betSettingReader;
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

                        var dataList = GetBetSettings(reader, _appSettings.IsShowMixedSport);
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

            sportList.Add(_betSettingReader.ReadForSport(reader, ((int)SportType.HorseRacingFixedOdds).ToString()));

            return sportList;
        }
    }
}