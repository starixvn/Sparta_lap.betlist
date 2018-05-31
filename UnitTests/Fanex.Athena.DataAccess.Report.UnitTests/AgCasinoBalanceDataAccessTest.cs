namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Data;
    using System.Linq;
    using Core.UnitTests.TestHelpers;
    using Model.Core.DataDomain;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class AgCasinoBalanceDataAccessTest : BaseDataAccessHelperTest<AgCasinoBalanceDataAccess>
    {
        private const string GetSummaryBalanceSp = "Lrf_Rpt_AGCasinoBalance";

        private const string GetCustomerBalanceDetailSp = "LRF_Rpt_AGCasinoBalance_Mem";

        private const string GetAgentBalanceDetailSp = "LRF_Rpt_AGCasinoBalance_Ag";

        protected override void SetUpDataAccess()
        {
            DataAccess = Substitute.ForPartsOf<AgCasinoBalanceDataAccess>(CreateSessionStorage());
        }

        [Test]
        public void GetSummaryBalance_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetSummaryBalanceSp)).Returns(ObjectDb);
            var metaModel = CreateMetaModel();

            // Action
            DataAccess.GetSummaryBalance(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                ObjectDb.Received(1).AddInParameter("fromdate", DbType.Date, metaModel.FromDate.Date);
                ObjectDb.Received(1).AddInParameter("todate", DbType.Date, metaModel.ToDate.Date);
            });
        }

        [Test]
        public void GetSummaryBalance_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetSummaryBalanceSp)).Returns(ObjectDb);
            var metaModel = CreateMetaModel();
            var expected = new AgCasinoBalance
            {
                SystemId = string.Empty,
                ExternalId = string.Empty,
                CurrencyId = CurrencyId,
                Date = metaModel.DrillDownFromDate
            };
            ConfigureReaderForGetSummaryWinlossMethod(expected);

            // Action
            var result = DataAccess.GetSummaryBalance(metaModel).DataList.First();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.SystemId, Is.EqualTo(expected.SystemId));
                Assert.That(result.ExternalId, Is.EqualTo(expected.ExternalId));
                Assert.That(result.CustomerId, Is.EqualTo(expected.CustomerId));
                Assert.That(result.CurrencyId, Is.EqualTo(expected.CurrencyId));
                Assert.That(result.CurrencyName, Is.EqualTo(CurrencyName));
                Assert.That(result.CustomerWinLoss, Is.EqualTo(expected.CustomerWinLoss));
                Assert.That(result.LicenseeWL, Is.EqualTo(expected.LicenseeWL));
                Assert.That(result.TurnOver, Is.EqualTo(expected.TurnOver));
                Assert.That(result.TotalTransaction, Is.EqualTo(expected.TotalTransaction));
                Assert.That(result.Date, Is.EqualTo(expected.Date));
                Assert.That(result.OpeningBalance, Is.EqualTo(expected.OpeningBalance));
                Assert.That(result.TransferIn, Is.EqualTo(expected.TransferIn));
                Assert.That(result.TransferOut, Is.EqualTo(expected.TransferOut));
                Assert.That(result.ClosingBalance, Is.EqualTo(expected.ClosingBalance));
            });
        }

        [Test]
        public void GetSummaryBalance_ObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                GetSummaryBalanceSp,
                () => DataAccess.GetSummaryBalance(new ThirdPartyWinLossMetaModel()));
        }

        [Test]
        public void GetCustomerBalanceDetail_WhenCall_ParametersReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetCustomerBalanceDetailSp)).Returns(ObjectDb);
            var metaModel = CreateMetaModel();

            const int TotalRows = 10;
            ObjectDb.GetParameterValue("TotalRows").Returns(TotalRows);

            // Action
            DataAccess.GetCustomerBalanceDetail(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("custid", DbType.Int32, metaModel.DrillDownCustomerId);
                ObjectDb.Received(1).AddInParameter("pagesize", DbType.Int32, metaModel.PageSize);
                ObjectDb.Received(1).AddInParameter("pageindex", DbType.Int32, metaModel.PageIndex);
                ObjectDb.Received(1).AddOutParameter("TotalRows", DbType.Int32, TotalRows);
                ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                ObjectDb.Received(1).AddInParameter("fromdate", DbType.Date, metaModel.DrillDownFromDate);
                ObjectDb.Received(1).AddInParameter("todate", DbType.Date, metaModel.DrillDownToDate);
            });
        }

        [Test]
        public void GetCustomerBalanceDetail_ObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                GetCustomerBalanceDetailSp,
                () => DataAccess.GetCustomerBalanceDetail(new ThirdPartyWinLossMetaModel()));
        }

        [Test]
        public void GetCustomerBalanceDetail_ExportExcel_RemovePagingFromParameters()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetCustomerBalanceDetailSp)).Returns(ObjectDb);

            const int PageIndex = 1;
            const int PageSize = SystemConstants.MaxDataTotalRow;
            var metaModel = CreateMetaModel();
            metaModel.IsExportExcel = true;

            const int TotalRows = 10;
            ObjectDb.GetParameterValue("TotalRows").Returns(TotalRows);

            // Action
            DataAccess.GetCustomerBalanceDetail(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("pagesize", DbType.Int32, PageSize);
                ObjectDb.Received(1).AddInParameter("pageindex", DbType.Int32, PageIndex);
            });
        }

        [Test]
        public void GetCustomerBalanceDetail_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetCustomerBalanceDetailSp)).Returns(ObjectDb);
            var metaModel = CreateMetaModel();

            const int TotalRows = 10;
            ObjectDb.GetParameterValue("TotalRows").Returns(TotalRows);

            var expected = new AgCasinoBalance
            {
                SystemId = string.Empty,
                ExternalId = string.Empty,
                CurrencyId = CurrencyId,
                Date = new DateTime(2016, 8, 29)
            };
            ConfigureReaderForGetSummaryWinlossMethod(expected);

            // Action
            var result = DataAccess.GetCustomerBalanceDetail(metaModel).DataList[0];

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.SystemId, Is.EqualTo(expected.SystemId));
                Assert.That(result.ExternalId, Is.EqualTo(expected.ExternalId));
                Assert.That(result.CustomerId, Is.EqualTo(expected.CustomerId));
                Assert.That(result.CurrencyId, Is.EqualTo(expected.CurrencyId));
                Assert.That(result.CurrencyName, Is.EqualTo(CurrencyName));
            });
        }

        [Test]
        public void GetAgentBalanceDetail_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetAgentBalanceDetailSp)).Returns(ObjectDb);
            var metaModel = CreateMetaModel();

            var expected = new AgCasinoBalance
            {
                SystemId = string.Empty,
                ExternalId = string.Empty,
                CurrencyId = CurrencyId,
                Date = new DateTime(2016, 8, 29)
            };
            ConfigureReaderForGetSummaryWinlossMethod(expected);

            // Action
            var result = DataAccess.GetAgentBalanceDetail(metaModel).DataList[0];

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.SystemId, Is.EqualTo(expected.SystemId));
                Assert.That(result.ExternalId, Is.EqualTo(expected.ExternalId));
                Assert.That(result.CustomerId, Is.EqualTo(expected.CustomerId));
                Assert.That(result.CurrencyId, Is.EqualTo(expected.CurrencyId));
                Assert.That(result.CurrencyName, Is.EqualTo(CurrencyName));
            });
        }

        [Test]
        public void GetAgentBalanceDetailSp_ObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                GetAgentBalanceDetailSp,
                () => DataAccess.GetAgentBalanceDetail(new ThirdPartyWinLossMetaModel()));
        }

        [Test]
        public void GetData_DrillDownLevel2_CalculateIndexByPaging()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetAgentBalanceDetailSp)).Returns(ObjectDb);
            var metaModel = CreateMetaModel();
            metaModel.PageIndex = 2;
            metaModel.PageSize = 100;
            metaModel.DrillDownLevel = 2;

            const int ExpectedIndex = 101;

            var expected = new AgCasinoBalance
            {
                SystemId = string.Empty,
                ExternalId = string.Empty,
                CurrencyId = CurrencyId,
                Date = new DateTime(2016, 8, 29)
            };
            ConfigureReaderForGetSummaryWinlossMethod(expected);

            // Action
            var result = DataAccess.GetAgentBalanceDetail(metaModel).DataList[0];

            // Assert
            Assert.That(result.Index, Is.EqualTo(ExpectedIndex));
        }

        [Test]
        public void GetData_HasUserNameColumn_ReturnCorrectDataForSystemId()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetAgentBalanceDetailSp)).Returns(ObjectDb);
            var metaModel = CreateMetaModel();

            const string UserName = "User name test";

            var expected = new AgCasinoBalance
            {
                SystemId = UserName,
                ExternalId = string.Empty,
                CurrencyName = "CurrencyNameTest",
                Date = new DateTime(2016, 8, 29),
            };
            ConfigureReaderForGetSummaryWinlossMethod(expected);

            Reader.FieldCount.Returns(1);
            Reader.GetName(Arg.Any<int>()).Returns("UserName");

            // Action
            var result = DataAccess.GetAgentBalanceDetail(metaModel).DataList[0];

            // Assert
            Assert.That(result.SystemId, Is.EqualTo(UserName));
        }

        [Test]
        public void GetData_HasNickNameColumn_ReturnCorrectDataForExternalId()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetAgentBalanceDetailSp)).Returns(ObjectDb);
            var metaModel = CreateMetaModel();

            const string NickName = "Nick name test";

            var expected = new AgCasinoBalance
            {
                SystemId = string.Empty,
                ExternalId = NickName,
                CurrencyName = "CurrencyNameTest",
                Date = new DateTime(2016, 8, 29),
            };
            ConfigureReaderForGetSummaryWinlossMethod(expected);

            Reader.FieldCount.Returns(1);
            Reader.GetName(Arg.Any<int>()).Returns("NickName");

            // Action
            var result = DataAccess.GetAgentBalanceDetail(metaModel).DataList[0];

            // Assert
            Assert.That(result.ExternalId, Is.EqualTo(NickName));
        }

        [Test]
        public void GetData_HasCustIdColumn_ReturnCorrectDataForCustomerId()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetAgentBalanceDetailSp)).Returns(ObjectDb);
            var metaModel = CreateMetaModel();

            const int CustId = 100;

            var expected = new AgCasinoBalance
            {
                SystemId = string.Empty,
                ExternalId = string.Empty,
                CustomerId = CustId,
                CurrencyName = "CurrencyNameTest",
                Date = new DateTime(2016, 8, 29),
            };
            ConfigureReaderForGetSummaryWinlossMethod(expected);

            Reader.FieldCount.Returns(1);
            Reader.GetName(Arg.Any<int>()).Returns("CustId");

            // Action
            var result = DataAccess.GetAgentBalanceDetail(metaModel).DataList[0];

            // Assert
            Assert.That(result.CustomerId, Is.EqualTo(CustId));
        }

        [Test]
        public void GetData_HasCurrencyColumn_ReturnCorrectDataForCurrencyId()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetAgentBalanceDetailSp)).Returns(ObjectDb);
            var metaModel = CreateMetaModel();

            const int Currency = 8;

            var expected = new AgCasinoBalance
            {
                SystemId = string.Empty,
                ExternalId = string.Empty,
                CurrencyId = Currency,
                CurrencyName = "CurrencyNameTest",
                Date = new DateTime(2016, 8, 29),
            };
            ConfigureReaderForGetSummaryWinlossMethod(expected);

            Reader.FieldCount.Returns(1);
            Reader.GetName(Arg.Any<int>()).Returns("Currency");

            // Action
            var result = DataAccess.GetAgentBalanceDetail(metaModel).DataList[0];

            // Assert
            Assert.That(result.CurrencyId, Is.EqualTo(Currency));
        }

        [Test]
        public void GetData_HasDateColumn_ReturnCorrectDataForDate()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetAgentBalanceDetailSp)).Returns(ObjectDb);
            var metaModel = CreateMetaModel();

            var date = new DateTime(2017, 1, 20);

            var expected = new AgCasinoBalance
            {
                SystemId = string.Empty,
                ExternalId = string.Empty,
                CurrencyName = "CurrencyNameTest",
                Date = date,
            };
            ConfigureReaderForGetSummaryWinlossMethod(expected);

            Reader.FieldCount.Returns(1);
            Reader.GetName(Arg.Any<int>()).Returns("Date");

            // Action
            var result = DataAccess.GetAgentBalanceDetail(metaModel).DataList[0];

            // Assert
            Assert.That(result.Date, Is.EqualTo(date));
        }

        private static ThirdPartyWinLossMetaModel CreateMetaModel()
        {
            return new ThirdPartyWinLossMetaModel
            {
                CurrencyId = 1,
                FromDate = new DateTime(2016, 8, 10),
                ToDate = new DateTime(2016, 8, 29),
                DrillDownFromDate = new DateTime(2016, 8, 10),
                DrillDownToDate = new DateTime(2016, 8, 29),
                BaseCurrency = "UUS"
            };
        }

        private void ConfigureReaderForGetSummaryWinlossMethod(AgCasinoBalance expected)
        {
            Reader.Read().Returns(true, false);

            Reader["username"].Returns(expected.SystemId);
            Reader["nickname"].Returns(expected.ExternalId);
            Reader["custid"].Returns(expected.CustomerId);
            Reader["currency"].Returns(expected.CurrencyId);
            Reader["winlostuus"].Returns(expected.CustomerWinLoss);
            Reader["licenseewinlostuus"].Returns(expected.LicenseeWL);
            Reader["turnoveruus"].Returns(expected.TurnOver);
            Reader["totaltrans"].Returns(expected.TotalTransaction);
            Reader["date"].Returns(expected.Date);
            Reader["OpeningBalanceuus"].Returns(expected.OpeningBalance);
            Reader["TransferInuus"].Returns(expected.TransferIn);
            Reader["TransferOutuus"].Returns(expected.TransferOut);
            Reader["ClosingBalanceuus"].Returns(expected.ClosingBalance);

            foreach (var currencySuffix in SystemConstants.BaseCurrencyList)
            {
                Reader["winlost" + currencySuffix].Returns(1);
                Reader["licenseewinlost" + currencySuffix].Returns(0);
                Reader["turnover" + currencySuffix].Returns(0);
                Reader["OpeningBalance" + currencySuffix].Returns(0);
                Reader["TransferIn" + currencySuffix].Returns(0);
                Reader["TransferOut" + currencySuffix].Returns(0);
                Reader["ClosingBalance" + currencySuffix].Returns(0);
            }
        }
    }
}