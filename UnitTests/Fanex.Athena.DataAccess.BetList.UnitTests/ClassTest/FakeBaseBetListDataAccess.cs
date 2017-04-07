namespace Fanex.Athena.DataAccess.BetList.UnitTests.ClassTest
{
    using System.Data;
    using Fanex.Athena.Extension.Utility;
    using Model.BetList.DTO;

    public class FakeBaseBetListDataAccess : BaseBetListDataAccess
    {
        public new SortableList<LrfTicket> ReadBetListTicket(IDataReader dataReader, string baseCurrency, long custId = 0)
        {
            return BaseBetListDataAccess.ReadBetListTicket(dataReader, baseCurrency, custId);
        }

        public new string NumberReport(object value)
        {
            return BaseBetListDataAccess.NumberReport(value);
        }
    }
}