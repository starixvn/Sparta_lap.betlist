namespace Fanex.Athena.DataAccess.Dashboard
{
    using System.Collections.Generic;
    using System.Data;
    using Core.Exception;
    using Data;
    using Fanex.Athena.DataAccess.Dashboard.Interfaces;
    using Fanex.Common.Extensions;
    using Logging;
    using Model.Core.DataDomain;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;

    public class TurnoverByPlatformDataAccess : IWidgetByPlatformDataAccess
    {
        public WidgetByPlatformViewModel GetData(DashboardBaseModel metaModel)
        {
            const string UspName = "Lrf_GetLicenseeTurnoverByPlatform";
            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    var param = new
                    {
                        LicenseeID = metaModel.LicenseeId,
                        UserID = metaModel.UserId,
                        CurrencyID = metaModel.CurrencyId,
                        FromDate = metaModel.FromDate,
                        ToDate = metaModel.ToDate,
                        MoveBetsDay = metaModel.MoveBetsDay
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
                var key = PlatformType.GetPlatformName(platformType);
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