namespace Fanex.Athena.DataAccess.Dashboard
{
    using System.Collections.Generic;
    using System.Data;
    using Core;
    using Core.Exception;
    using Data;
    using Fanex.Athena.DataAccess.Dashboard.Interfaces;
    using Fanex.Common.Extensions;
    using Logging;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;

    public class TurnoverByPlatformDataAccess : BaseDataAccess, IWidgetByPlatformDataAccess
    {
        public WidgetByPlatformViewModel GetData(DashboardBaseModel metaModel)
        {
            const string UspName = "Lrf_GetLicenseeTurnoverByPlatform";
            try
            {
                using (var db = this.GetObjectDb(UspName))
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
                        return ReadData(reader, metaModel.BaseCurrency);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, metaModel);
            }
        }

        private static WidgetByPlatformViewModel ReadData(IDataReader reader, string baseCurrency)
        {
            var turnoverColumn = "Turnover" + baseCurrency;

            var dataList = new List<KeyValuePair<string, decimal>>();
            var total = 0m;
            while (reader.Read())
            {
                var platformType = reader.ToInt("PlatformType");
                var key = PlatformKey.GetPlatformName(platformType);
                var value = reader.ToDecimal(turnoverColumn);
                total += value;
                dataList.Add(new KeyValuePair<string, decimal>(key, value));
            }

            return new WidgetByPlatformViewModel
            {
                Data = dataList,
                Total = total
            };
        }
    }
}