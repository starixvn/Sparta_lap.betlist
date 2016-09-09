namespace Fanex.Athena.DataAccess.BetList.Tests.ClassTest
{
    using System.Data;
    using Fanex.Athena.Extension.Utility;
    using Model.BetList.DTO;

    public class FakeBaseBetListDataAccess : BaseBetListDataAccess
    {
        public new SortableList<LrfTicket> ReadLrfTicket(IDataReader dataReader, string baseCurrency, long custId = 0)
        {
            return BaseBetListDataAccess.ReadLrfTicket(dataReader, baseCurrency, custId);
        }

        public new string NumberReport(object value)
        {
            return BaseBetListDataAccess.NumberReport(value);
        }
    }
}