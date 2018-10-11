namespace Fanex.Athena.DataAccess.BetList.UnitTests
{
    using System;
    using System.Data;
    using Fanex.BetList.Core.Entities;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;
    using UnitTest.Common.NUnitExtensions;

    public partial class BetListDataAccessTest
    {
        private const string GetSystemParlayDetailSp = "Lrf_BetList_SelCombinationParlayDetail";

        [Test]
        public void GetSystemParlayDetail_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetSystemParlayDetailSp)).Returns(ObjectDb);
            var metaModel = GetSystemParlayDetailMetaModel();

            var dataset = new DataSet();
            ObjectDb.ExecuteDataSet().Returns(dataset);

            // Action
            DataAccess.GetSystemParlayDetail(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("refno", DbType.Int64, metaModel.RefNo);
                ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                ObjectDb.Received(1).AddInParameter("movebetsday", DbType.DateTime, metaModel.MoveBetsDay);
                ObjectDb.Received(1).AddInParameter("balanceupday", DbType.DateTime, metaModel.BalanceUpDay);            
                ObjectDb.Received(1).AddInParameter("BaseCurrency", DbType.String, metaModel.BaseCurrency);
                ObjectDb.Received(1).AddOutParameter("outstanding", DbType.Currency, 8);
            });
        }

        [Test]
        public void GetSystemParlayDetail_WhenCall_GetTicketDataCorrectly()
        {
            // Arrange
            var expectedResult = FakeHelper.Create<SystemParlayTicket>();
            var metaModel = GetSystemParlayDetailMetaModel();
            DataAccess.GetObjectDb(Arg.Is(GetSystemParlayDetailSp)).Returns(ObjectDb);
            ObjectDb.ExecuteDataSet().Returns(CreateFakeDataSetForSystemParlayTickeData(expectedResult));

            // Action
            var result = DataAccess.GetSystemParlayDetail(metaModel);

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
            DataAccess.GetObjectDb(Arg.Is(GetSystemParlayDetailSp)).Returns(ObjectDb);
            ObjectDb.ExecuteDataSet().Returns(CreateFakeDataSetForSystemParlaySerialData(expectedResult));

            // Action
            var result = DataAccess.GetSystemParlayDetail(metaModel);

            // Assert
            Assert.That(result.SerialList[0], Tobe.SameProperties(expectedResult));
        }

        [Test]
        public void GetSystemParlayDetail_WhenObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                GetSystemParlayDetailSp,
                () => DataAccess.GetSystemParlayDetail(new SystemParlayDetailMetaModel()));
        }

        private static DataSet CreateFakeDataSetForSystemParlayTickeData(SystemParlayTicket data)
        {
            var dataSet = new DataSet();
            var table = new DataTable();
            dataSet.Tables.Add(table);
            table.Columns.Add("comm", typeof(decimal));
            table.Columns.Add("modds", typeof(decimal));
            table.Columns.Add("playercomm", typeof(decimal));
            table.Columns.Add("serial", typeof(long));
            table.Columns.Add("stake", typeof(decimal));
            table.Columns.Add("status", typeof(string));
            table.Columns.Add("statusid", typeof(int));
            table.Columns.Add("transid", typeof(long));
            table.Columns.Add("winlost", typeof(decimal));
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