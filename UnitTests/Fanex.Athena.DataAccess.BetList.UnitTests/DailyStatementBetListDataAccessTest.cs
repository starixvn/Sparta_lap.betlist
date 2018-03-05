namespace Fanex.Athena.DataAccess.BetList.UnitTests
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Globalization;
    using Core.Configuration.Interfaces;
    using Core.UnitTests.TestHelpers;
    using Data;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.Core.DTO.Configuration;
    using NSubstitute;
    using NUnit.Framework;

    public class DailyStatementBetListDataAccessTest : BaseDataAccessHelperTest<DailyStatementBetListDataAccess>
    {
        private const string UspName = "Lrf_Statement_Mem";

        private static readonly BalanceUp BalanceUp = new BalanceUp
        {
            MoveBetsDay = new DateTime(2016, 1, 1),
            BalanceUpDay = new DateTime(2016, 2, 1)
        };

        private BetListMetaModel _metaModel;

        private static IEnumerable TestCasesValue
        {
            get
            {
                yield return new TestCaseData(new
                {
                    TransDesc = "Test",
                    TotalAmt = 200,
                    ExpectedTransType = "0",
                    ExpectedCreditDebit = 0,
                    TransType = "bal",
                    ExpectedTransTypeHasBetList = false,
                    AllowCustComm = true,
                    Commission = 100,
                    ExpectedBalance = 200, // =  TotalAmt
                    ExternalId = (string)null,
                    ExpectedExternalId = "0",
                    SystemId = (string)null,
                    ExpectedSystemId = "0"
                });
                yield return new TestCaseData(new
                {
                    TransDesc = "outstanding bet",
                    TotalAmt = 200,
                    ExpectedTransType = "3",
                    ExpectedCreditDebit = 0, // = TotalAmt
                    TransType = "no bal",
                    ExpectedTransTypeHasBetList = false,
                    AllowCustComm = true,
                    Commission = 100,
                    ExpectedBalance = 100, // =  ExpectedCreditDebit + Commission
                    ExternalId = (string)null,
                    ExpectedExternalId = "0",
                    SystemId = (string)null,
                    ExpectedSystemId = "0"
                });
                yield return new TestCaseData(new
                {
                    TransDesc = "betting statement",
                    TotalAmt = 200,
                    ExpectedTransType = "1",
                    ExpectedCreditDebit = 200, // = TotalAmt
                    TransType = "no bal",
                    ExpectedTransTypeHasBetList = false,
                    AllowCustComm = false,
                    Commission = 100,
                    ExpectedBalance = 200, // =  ExpectedCreditDebit
                    ExternalId = "2",
                    ExpectedExternalId = "2",
                    SystemId = "3",
                    ExpectedSystemId = "3"
                });
                yield return new TestCaseData(new
                {
                    TransDesc = "betting statement",
                    TotalAmt = 200,
                    ExpectedTransType = "1",
                    ExpectedCreditDebit = 200, // = TotalAmt
                    TransType = "gp",
                    ExpectedTransTypeHasBetList = true,
                    AllowCustComm = false,
                    Commission = 100,
                    ExpectedBalance = 0,
                    ExternalId = "4",
                    ExpectedExternalId = "4",
                    SystemId = "5",
                    ExpectedSystemId = "5"
                });
            }
        }

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            _metaModel = CreateBetListMetaModel();
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            balanceUpDataAccess.GetBalanceUp().Returns(BalanceUp);

            ObjectDb = Substitute.For<IObjectDb>();
            DataAccess = Substitute.ForPartsOf<DailyStatementBetListDataAccess>(balanceUpDataAccess);
        }

        [Test]
        public void GetDailyStatementData_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(UspName)).Returns(ObjectDb);

            // Action
            DataAccess.GetDailyStatementData(_metaModel, true);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("CustID", DbType.Int32, _metaModel.CustId);
                ObjectDb.Received().AddInParameter("FromDate", DbType.DateTime, _metaModel.FromDate);
                ObjectDb.Received().AddInParameter("ToDate", DbType.DateTime, _metaModel.ToDate);
                ObjectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, BalanceUp.MoveBetsDay);
                ObjectDb.Received().AddInParameter("BalanceUpDay", DbType.DateTime, BalanceUp.BalanceUpDay);
                ObjectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
                ObjectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
                ObjectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
            });
        }

        [TestCaseSource(nameof(TestCasesValue))]
        public void GetDailyStatementData_WhenCall_DataReturnedCorrectly(dynamic testCaseData)
        {
            // Arrange
            _metaModel.ExternalId = testCaseData.ExternalId;
            _metaModel.SystemId = testCaseData.SystemId;

            DataAccess.GetObjectDb(Arg.Is(UspName)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);

            ConfigureReaderForGetDailyStatementData(_metaModel, testCaseData);

            var expected = GetDailyStatementData(_metaModel, testCaseData);

            // Action
            var result = DataAccess.GetDailyStatementData(_metaModel, testCaseData.AllowCustComm);
            var firstItem = result[0];

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(firstItem.TransType, Is.EqualTo(expected.TransType));
                Assert.That(firstItem.CustId, Is.EqualTo(expected.CustId));
                Assert.That(firstItem.ExternalId, Is.EqualTo(expected.ExternalId));
                Assert.That(firstItem.SystemId, Is.EqualTo(expected.SystemId));
                Assert.That(firstItem.FullStatementDate, Is.EqualTo(expected.FullStatementDate));
                Assert.That(firstItem.TransTypeHasBetList, Is.EqualTo(expected.TransTypeHasBetList));
                Assert.That(firstItem.Index, Is.EqualTo(expected.Index));
                Assert.That(firstItem.Date, Is.EqualTo(expected.Date));
                Assert.That(firstItem.Remark, Is.EqualTo(expected.Remark));
                Assert.That(firstItem.Turnover, Is.EqualTo(expected.Turnover));
                Assert.That(firstItem.CreditDebit, Is.EqualTo(expected.CreditDebit));
                Assert.That(firstItem.Commission, Is.EqualTo(expected.Commission));
                Assert.That(firstItem.Balance, Is.EqualTo(expected.Balance));
            });
        }

        [Test]
        public void GetDailyStatementData_WhenObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                UspName,
                () => DataAccess.GetDailyStatementData(_metaModel, true));
        }

        private static BetListMetaModel CreateBetListMetaModel()
        {
            return new BetListMetaModel
            {
                BaseCurrency = "BaseCurrencyTest",
                CustId = 1,
                FromDate = new DateTime(2016, 8, 10),
                ToDate = new DateTime(2016, 8, 19),
                PageSize = 10,
                PageIndex = 1,
                ExternalId = "ExternalIdTest",
                SystemId = "SystemIdTest",
            };
        }

        private static DailyStatement GetDailyStatementData(BetListMetaModel metaModel, dynamic testCaseData)
        {
            return new DailyStatement
            {
                TransType = testCaseData.ExpectedTransType,
                CustId = metaModel.CustId,
                ExternalId = testCaseData.ExpectedExternalId,
                SystemId = testCaseData.ExpectedSystemId,
                FullStatementDate = "Friday <br/> 8/19/2016 12:00:00 AM",
                TransTypeHasBetList = testCaseData.ExpectedTransTypeHasBetList,
                Index = 1,
                Date = "8/19/2016",
                Remark = $"<span class='bl-underdog'>{testCaseData.TransType.ToUpper(CultureInfo.CurrentCulture)} <br/>- {testCaseData.TransDesc}</span>",
                Turnover = "<span class='bl-underdog'>11.50</span>",
                CreditDebit = $"<span class='bl-underdog'>{testCaseData.ExpectedCreditDebit:#,##0.00}</span>",
                Commission = $"<span class='bl-underdog'>{testCaseData.Commission:#,##0.00}</span>",
                Balance = $"<span class='bl-underdog'>{testCaseData.ExpectedBalance:#,##0.00}</span>",
            };
        }

        private void ConfigureReaderForGetDailyStatementData(BetListMetaModel metaModel, dynamic testCaseData)
        {
            var commissionColumn = "commission" + metaModel.BaseCurrency;
            var totalAmtColumn = "totalamt" + metaModel.BaseCurrency;
            var turnoverColumn = "turnover" + metaModel.BaseCurrency;

            Reader["winlostdate"].Returns(new DateTime(2016, 8, 19));
            Reader["transtype"].Returns((string)testCaseData.TransType);
            Reader["transdesc"].Returns((string)testCaseData.TransDesc);
            Reader[commissionColumn].Returns((decimal)testCaseData.Commission);
            Reader[turnoverColumn].Returns(11.5m);
            Reader[totalAmtColumn].Returns((decimal)testCaseData.TotalAmt);
        }
    }
}