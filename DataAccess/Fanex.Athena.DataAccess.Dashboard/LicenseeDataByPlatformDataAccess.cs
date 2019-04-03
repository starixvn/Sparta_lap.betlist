namespace Fanex.Athena.DataAccess.Dashboard
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Core;
    using Fanex.Athena.DataAccess.Dashboard.Interfaces;
    using Fanex.Common.Extensions;
    using Model.Core.DataDomain;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;

    public class LicenseeDataByPlatformDataAccess : BaseDataAccess, ILicenseeDataByPlatformDataAccess
    {
        public Task<WidgetByPlatformViewModel> GetWinLoss(DashboardBaseModel metaModel)
        {
            return GetData(metaModel, "Lrf_GetLicenseeWLByPlatform", "LicenseeWL");
        }

        public Task<WidgetByPlatformViewModel> GetTurnover(DashboardBaseModel metaModel)
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
                var key = PlatformType.GetPlatformKey(platformType);
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

        private async Task<WidgetByPlatformViewModel> GetData(DashboardBaseModel metaModel, string uspName, string winlossColumnName)
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

                using (var reader = await db.ExecuteReaderAsync(param).ConfigureAwait(false))
                {
                    return ReadData(reader, winlossColumnName, metaModel.BaseCurrency);
                }
            }
        }
    }
}