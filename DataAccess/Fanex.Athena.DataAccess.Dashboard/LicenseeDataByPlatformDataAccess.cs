namespace Fanex.Athena.DataAccess.Dashboard
{
    using System.Collections.Generic;
    using System.Data;
    using Core;
    using Core.Exception;
    using Data;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.Dashboard.Interfaces;
    using Fanex.Common.Extensions;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using Session.Interfaces;

    public class LicenseeDataByPlatformDataAccess : BaseDataAccess, ILicenseeDataByPlatformDataAccess
    {
        public LicenseeDataByPlatformDataAccess(IAppSettings appSetting, ISessionStorage sessionStorage)
            : base(appSetting, sessionStorage)
        {
        }

        public WidgetByPlatformViewModel GetWinLoss(DashboardBaseModel metaModel)
        {
            return GetData(metaModel, "Lrf_GetLicenseeWLByPlatform", "LicenseeWL");
        }

        public WidgetByPlatformViewModel GetTurnover(DashboardBaseModel metaModel)
        {
            return GetData(metaModel, "Lrf_GetLicenseeTurnoverByPlatform", "Turnover");
        }

        private static WidgetByPlatformViewModel ReadData(IDataReader reader, string winlossColumnName, string baseCurrency)
        {
            var winlossColumn = winlossColumnName + baseCurrency;

            var dataList = new List<KeyValuePair<string, decimal>>();
            var total = 0m;
            while (reader.Read())
            {
                var platformType = reader.ToInt("PlatformType");
                var key = PlatformKey.GetPlatformName(platformType);
                var value = reader.ToDecimal(winlossColumn);
                total += value;
                dataList.Add(new KeyValuePair<string, decimal>(key, value));
            }

            return new WidgetByPlatformViewModel
            {
                Data = dataList,
                Total = total
            };
        }

        private WidgetByPlatformViewModel GetData(DashboardBaseModel metaModel, string uspName, string winlossColumnName)
        {
            try
            {
                using (var db = this.GetObjectDb(uspName))
                {
                    var param = new
                    {
                        LicenseeID = metaModel.LicenseeId,
                        metaModel.CurrencyIDList,
                        FromDate = metaModel.FromDate,
                        ToDate = metaModel.ToDate
                    };

                    using (var reader = db.ExecuteReader(param))
                    {
                        return ReadData(reader, winlossColumnName, metaModel.BaseCurrency);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, uspName, metaModel);
            }
        }
    }
}