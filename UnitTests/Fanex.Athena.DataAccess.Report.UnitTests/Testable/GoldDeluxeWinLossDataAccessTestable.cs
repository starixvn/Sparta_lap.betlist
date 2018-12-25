using System.Collections.Generic;
using System.Data;
using Fanex.Athena.Model.Report.DTO;
using Fanex.Athena.Model.Report.MetaModel;
using Fanex.Data;

namespace Fanex.Athena.DataAccess.Report.UnitTests.Testable
{
    public class GoldDeluxeWinLossDataAccessTestable : GoldDeluxeWinLossDataAccess
    {
        public void Public_ReadSummaryTotal(IDataReader reader, IDictionary<string, GoldDeluxeWinLoss> summaryTotal)
        {
            ReadSummaryTotal(reader, summaryTotal);
        }

        public void Public_UpdateUspParams(IObjectDb db, ThirdPartyWinLossMetaModel metaModel, bool drilldown)
        {
            UpdateUspParams(db, metaModel, drilldown);
        }

        public void Public_PostReadItem(IDataReader reader, GoldDeluxeWinLoss item, string baseCurrency)
        {
            PostReadItem(reader, item, baseCurrency);
        }

        public IEnumerable<GoldDeluxeWinLoss> Public_CalculateTotal(IEnumerable<GoldDeluxeWinLoss> dataList)
        {
            return CalculateTotal(dataList);
        }
    }
}