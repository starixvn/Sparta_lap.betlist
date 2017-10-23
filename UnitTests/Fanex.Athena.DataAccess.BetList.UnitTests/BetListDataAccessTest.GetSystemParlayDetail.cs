namespace Fanex.Athena.DataAccess.BetList.UnitTests
{
    using System;
    using System.Data;
    using Fanex.BetList.Core.Entities;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Extensions;

    [TestFixture]
    public partial class BetListDataAccessTest
    {
        [Test]
        public void GetSystemParlayDetail_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_BetList_SelCombinationParlayDetail")).Returns(_objectDb);
            var metaModel = GetSystemParlayDetailMetaModel();

            var dataset = new DataSet();
            _objectDb.ExecuteDataSet().Returns(dataset);

            // Action
            _dataAccess.GetSystemParlayDetail(metaModel);

            // Assert
            _objectDb.Received().AddInParameter("refno", DbType.Int64, metaModel.RefNo);
            _objectDb.Received().AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("movebetsday", DbType.DateTime, metaModel.MoveBetsDay);
            _objectDb.Received().AddInParameter("balanceupday", DbType.DateTime, metaModel.BalanceUpDay);
            _objectDb.Received().AddOutParameter("outstanding", DbType.Currency, 8);
            _objectDb.Received().AddOutParameter("outstandingeur", DbType.Currency, 8);
            _objectDb.Received().AddOutParameter("outstandingusd", DbType.Currency, 8);
            _objectDb.Received().AddOutParameter("outstandinggbp", DbType.Currency, 8);
        }

        [Test]
        public void GetSystemParlayDetail_WhenCall_GetTicketDataCorrectly()
        {
            // Arrange
            const string BaseCurrency = "uus";
            var expectedResult = new SystemParlayTicket
            {
                Comm = 1,
                MOdds = 2,
                PlayerComm = 3,
                Serial = 4,
                Stake = 5,
                Status = "Fun",
                StatusId = 6,
                TransId = 7,
                WinLost = 8,
                WinLostDate = new DateTime(2017, 1, 1)
            };
            var metaModel = GetSystemParlayDetailMetaModel(BaseCurrency);
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_BetList_SelCombinationParlayDetail")).Returns(_objectDb);
            _objectDb.ExecuteDataSet().Returns(CreateFakeDataSetForSystemParlayTickeData(BaseCurrency, expectedResult));

            // Action
            var result = _dataAccess.GetSystemParlayDetail(metaModel);

            // Assert
            Assert.That(result.TicketList[0], Tobe.SameProperties(expectedResult));
        }

        [Test]
        public void GetSystemParlayDetail_WhenCall_GetSerialDataCorrectly()
        {
            // Arrange
            var expectedResult = new SystemParlaySerial
            {
                Serial = 1,
                HomeId = 2,
                AwayId = 3
            };
            var metaModel = GetSystemParlayDetailMetaModel();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_BetList_SelCombinationParlayDetail")).Returns(_objectDb);
            _objectDb.ExecuteDataSet().Returns(CreateFakeDataSetForSystemParlaySerialData(expectedResult));

            // Action
            var result = _dataAccess.GetSystemParlayDetail(metaModel);

            // Assert
            Assert.That(result.SerialList[0], Tobe.SameProperties(expectedResult));
        }

        private static DataSet CreateFakeDataSetForSystemParlayTickeData(string baseCurrency, SystemParlayTicket data)
        {
            var dataSet = new DataSet();
            var table = new DataTable();
            dataSet.Tables.Add(table);
            table.Columns.Add("comm" + baseCurrency, typeof(decimal));
            table.Columns.Add("modds", typeof(decimal));
            table.Columns.Add("playercomm" + baseCurrency, typeof(decimal));
            table.Columns.Add("serial", typeof(long));
            table.Columns.Add("stake" + baseCurrency, typeof(decimal));
            table.Columns.Add("status", typeof(string));
            table.Columns.Add("statusid", typeof(int));
            table.Columns.Add("transid", typeof(long));
            table.Columns.Add("winlost" + baseCurrency, typeof(decimal));
            table.Columns.Add("winlostdate", typeof(DateTime));

            table.Rows.Add(
                data.Comm,
                data.MOdds,
                data.PlayerComm,
                data.Serial,
                data.Stake,
                data.Status,
                data.StatusId,
                data.TransId,
                data.WinLost,
                data.WinLostDate);

            return dataSet;
        }

        private static DataSet CreateFakeDataSetForSystemParlaySerialData(SystemParlaySerial data)
        {
            var dataSet = new DataSet();
            dataSet.Tables.Add(new DataTable());
            var table = new DataTable();
            dataSet.Tables.Add(table);
            table.Columns.Add("serial", typeof(long));
            table.Columns.Add("homeid", typeof(int));
            table.Columns.Add("awayid", typeof(int));

            table.Rows.Add(data.Serial, data.HomeId, data.AwayId);

            return dataSet;
        }

        private static SystemParlayDetailMetaModel GetSystemParlayDetailMetaModel(string baseCurrency = "")
        {
            return new SystemParlayDetailMetaModel
            {
                BaseCurrency = baseCurrency,
                RefNo = 1,
                UserId = 2,
                LicenseeId = 3,
                MoveBetsDay = new DateTime(2016, 8, 10),
                BalanceUpDay = new DateTime(2016, 8, 18)
            };
        }
    }
}