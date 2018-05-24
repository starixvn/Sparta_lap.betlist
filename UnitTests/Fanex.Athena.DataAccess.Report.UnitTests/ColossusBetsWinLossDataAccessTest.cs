namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Data;
    using Core.Configuration.Interfaces;
    using Core.UnitTests.TestHelpers;
    using Data;
    using Extension.Helper;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Configuration;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ColossusBetsWinLossDataAccessTest : BaseDataAccessHelperTest<ColossusBetsWinLossDataAccess>
    {
        private const string GetSummaryWinlossSp = "Lrf_Rpt_ColossusBetsWinloss";

        private const string GetWinlossDetailSp = "Lrf_Rpt_ColossusBetsWinloss_detail";

        private readonly DateTime _moveBetsDay = new DateTime(2010, 1, 1);

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            var balanceUp = new BalanceUp
            {
                MoveBetsDay = _moveBetsDay
            };
            balanceUpDataAccess.GetBalanceUp().Returns(balanceUp);

            DataAccess = Substitute.ForPartsOf<ColossusBetsWinLossDataAccess>(
                CreateSessionStorage(), 
                balanceUpDataAccess);
        }

        [OneTimeSetUp]
        public override void SetUp()
        {
            ObjectDb = Substitute.For<IObjectDb>();
            base.SetUp();
        }

        [Test]
        public void GetSummaryWinloss_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetSummaryWinlossSp)).Returns(ObjectDb);
            var metaModel = CreateMetaModel();

            // Action
            DataAccess.GetSummaryWinloss(metaModel);

            // Assert
            AssertUspParameters(metaModel);
        }

        [Test]
        public void GetSummaryWinloss_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetSummaryWinlossSp)).Returns(ObjectDb);
            var metaModel = CreateMetaModel();
            var expected = new ColossusBetWinLoss
            {
                SystemId = string.Empty,
                ExternalId = string.Empty,
                CurrencyId = CurrencyId,
                Date = new DateTime(2016, 8, 29)
            };
            ConfigureReader(expected);

            // Action
            var result = DataAccess.GetSummaryWinloss(metaModel).DataList[0];

            // Assert
            AssertDataReturned(result, expected);
        }

        [Test]
        public void GetSummary_WhenObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                GetSummaryWinlossSp,
                () => DataAccess.GetSummaryWinloss(new ColossusBetsWinLossMetaModel()));
        }

        [Test]
        public void GetWinlossDetail_WhenCall_ParameterPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetWinlossDetailSp)).Returns(ObjectDb);
            var metaModel = CreateMetaModel();
            metaModel.DrillDownLevel = 1;

            // Action
            DataAccess.GetWinlossDetail(metaModel);

            // Assert
            AssertUspParameters(metaModel);
        }

        [Test]
        public void GetWinlossDetail_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetWinlossDetailSp)).Returns(ObjectDb);
            var metaModel = CreateMetaModel();

            var expected = new ColossusBetWinLoss
            {
                SystemId = string.Empty,
                ExternalId = string.Empty,
                CurrencyId = CurrencyId,
                Date = new DateTime(2016, 8, 29)
            };
            ConfigureReader(expected);

            // Action
            var result = DataAccess.GetWinlossDetail(metaModel).DataList[0];

            // Assert
            AssertDataReturned(result, expected);
        }

        [Test]
        public void GetWinlossDetail_WhenObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                GetWinlossDetailSp,
                () => DataAccess.GetWinlossDetail(new ColossusBetsWinLossMetaModel()));
        }

        [Test]
        public void ReadData_HasSystemIdColumn_ReturnCorrectDataForSystemId()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetWinlossDetailSp)).Returns(ObjectDb);
            var metaModel = CreateMetaModel();

            var expected = new ColossusBetWinLoss
            {
                SystemId = "SystemId Test",
                ExternalId = string.Empty,
                CurrencyId = CurrencyId,
                Date = new DateTime(2016, 8, 29)
            };
            ConfigureReader(expected);

            Reader.FieldCount.Returns(1);
            Reader.GetName(Arg.Any<int>()).Returns("systemid");
            Reader["systemid"].Returns(expected.SystemId);

            // Action
            var result = DataAccess.GetWinlossDetail(metaModel).DataList[0];

            // Assert
            Assert.That(result.SystemId, Is.EqualTo(expected.SystemId));
        }

        [Test]
        public void ReadData_HasExternalIdColumn_ReturnCorrectDataForExternalId()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetWinlossDetailSp)).Returns(ObjectDb);
            var metaModel = CreateMetaModel();

            var expected = new ColossusBetWinLoss
            {
                SystemId = string.Empty,
                ExternalId = "ExternalId Test",
                CurrencyId = CurrencyId,
                Date = new DateTime(2016, 8, 29)
            };
            ConfigureReader(expected);

            Reader.FieldCount.Returns(1);
            Reader.GetName(Arg.Any<int>()).Returns("externalid");
            Reader["externalid"].Returns(expected.ExternalId);

            // Action
            var result = DataAccess.GetWinlossDetail(metaModel).DataList[0];

            // Assert
            Assert.That(result.ExternalId, Is.EqualTo(expected.ExternalId));
        }

        [Test]
        public void ReadData_HasCustIdColumn_ReturnCorrectDataForCustomerId()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetWinlossDetailSp)).Returns(ObjectDb);
            var metaModel = CreateMetaModel();

            var expected = new ColossusBetWinLoss
            {
                SystemId = string.Empty,
                ExternalId = string.Empty,
                CurrencyId = CurrencyId,
                Date = new DateTime(2016, 8, 29),
                CustomerId = 5
            };
            ConfigureReader(expected);

            Reader.FieldCount.Returns(1);
            Reader.GetName(Arg.Any<int>()).Returns("custid");
            Reader["custid"].Returns(expected.CustomerId);

            // Action
            var result = DataAccess.GetWinlossDetail(metaModel).DataList[0];

            // Assert
            Assert.That(result.CustomerId, Is.EqualTo(expected.CustomerId));
        }

        [Test]
        public void ReadData_Level0_ReturnCorrectDateFromDB()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetWinlossDetailSp)).Returns(ObjectDb);
            var metaModel = CreateMetaModel();

            var expected = new ColossusBetWinLoss
            {
                SystemId = string.Empty,
                ExternalId = string.Empty,
                CurrencyId = CurrencyId,
                Date = new DateTime(2016, 8, 29)
            };
            ConfigureReader(expected);

            // Action
            var result = DataAccess.GetWinlossDetail(metaModel).DataList[0];

            // Assert
            Assert.That(
                result.Date,
                Is.EqualTo(ConvertHelper.ToDateTime(expected.Date.ToString("yyyyMMdd"), metaModel.ReportBy == ReportByType.Monthly)));
        }

        [Test]
        public void ReadData_LevelIsNotZero_ReturnCorrectDateFromMetaModel()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetWinlossDetailSp)).Returns(ObjectDb);
            var metaModel = CreateMetaModel();
            metaModel.DrillDownLevel = 1;

            var expected = new ColossusBetWinLoss
            {
                SystemId = string.Empty,
                ExternalId = string.Empty,
                CurrencyId = CurrencyId,
                Date = new DateTime(2016, 8, 25)
            };
            ConfigureReader(expected);

            // Action
            var result = DataAccess.GetWinlossDetail(metaModel).DataList[0];

            // Assert
            Assert.That(
                result.Date,
                Is.EqualTo(ConvertHelper.ToDateTime(metaModel.DrillDownFromDate.ToString("yyyyMMdd"), metaModel.ReportBy == ReportByType.Monthly)));
        }

        private static ColossusBetsWinLossMetaModel CreateMetaModel()
        {
            return new ColossusBetsWinLossMetaModel
            {
                CurrencyId = 1,
                DrillDownCustomerId = 2,
                FromDate = new DateTime(2016, 8, 10),
                ToDate = new DateTime(2016, 8, 29),
                DrillDownFromDate = new DateTime(2016, 8, 10),
                DrillDownToDate = new DateTime(2016, 8, 29),
                BaseCurrency = "UUS",
                TicketType = 2
            };
        }

        private static void AssertDataReturned(ColossusBetWinLoss actual, ColossusBetWinLoss expected)
        {
            Assert.That(actual.SystemId, Is.EqualTo(expected.SystemId));
            Assert.That(actual.ExternalId, Is.EqualTo(expected.ExternalId));
            Assert.That(actual.CustomerId, Is.EqualTo(expected.CustomerId));
            Assert.That(actual.CurrencyId, Is.EqualTo(expected.CurrencyId));
            Assert.That(actual.CurrencyName, Is.EqualTo(CurrencyName));
            Assert.That(actual.TurnOver, Is.EqualTo(expected.TurnOver));
            Assert.That(actual.TotalTransaction, Is.EqualTo(expected.TotalTransaction));
            Assert.That(actual.Date, Is.EqualTo(expected.Date));
        }

        private void ConfigureReader(ColossusBetWinLoss expected)
        {
            Reader.Read().Returns(true, false);

            Reader["currencyid"].Returns(expected.CurrencyId);
            Reader["totaltrans"].Returns(expected.TotalTransaction);
            Reader["reporttype"].Returns(expected.Date.ToString("yyyyMMdd"));
            Reader["turnoveruus"].Returns(0);
            Reader["winningprizeuus"].Returns(1);
            Reader["cashoutamtuus"].Returns(0);
            Reader["cashinamtuus"].Returns(0);
            Reader["grandamtuus"].Returns(0);
            Reader["commamtuus"].Returns(0);

            foreach (var suffix in SystemConstants.BaseCurrencyList)
            {
                Reader["turnover" + suffix].Returns(0);
                Reader["winningprize" + suffix].Returns(1);
                Reader["cashoutamt" + suffix].Returns(0);
                Reader["cashinamt" + suffix].Returns(0);
                Reader["grandamt" + suffix].Returns(0);
                Reader["commamt" + suffix].Returns(0);
            }
        }

        private void AssertUspParameters(ColossusBetsWinLossMetaModel metaModel)
        {
            ObjectDb.Received(1).AddInParameter("fromdate", DbType.Date, metaModel.DrillDownFromDate);
            ObjectDb.Received(1).AddInParameter("todate", DbType.Date, metaModel.DrillDownToDate);
            ObjectDb.Received(1).AddInParameter("movebetsday", DbType.DateTime, _moveBetsDay);
            ObjectDb.Received(1).AddInParameter("ReportType", DbType.Int32, metaModel.ReportBy);
            ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            ObjectDb.Received(1).AddInParameter("TicketType", DbType.Int32, metaModel.TicketType);
        }
    }
}