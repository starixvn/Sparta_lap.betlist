namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core.UnitTests.TestHelpers;
    using Model.Core.MetaModel.Product;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerPendingFundsDataAccessTest : BaseDataAccessHelperTest<CustomerPendingFundsDataAccess>
    {
        private const string SpName = "Lrf_Rpt_Outstanding";

        private CustomerPendingFundsMetaModel _metaModel;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);

            _metaModel = new CustomerPendingFundsMetaModel
            {
                ProductOptions = GetProductOptions(),
                BaseCurrency = "UUS"
            };
        }

        [Test]
        public void GetDataList_WhenCall_ParametersPassedCorrectly()
        {
            // Action
            DataAccess.GetDataList(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("pagesize", DbType.Int32, _metaModel.PageSize);
                ObjectDb.Received().AddInParameter("pageindex", DbType.Int32, _metaModel.PageIndex);
                ObjectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                ObjectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
                ObjectDb.Received().AddInParameter("showNG", DbType.Boolean, false);
            });
        }

        [Test]
        public void GetDataList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            Reader.Read().Returns(true, false, true, false);
            Reader.NextResult().Returns(true);

            var expectedTotalData = new CustPendingFunds
            {
                BetCounts = 1,
                PendingFunds = 20m
            };

            var expectedResult = new CustPendingFunds
            {
                CustId = 1,
                ExternalId = "ExternalIdTest",
                SystemId = "SystemIdTest",
                BetCounts = 10,
                PendingFunds = 15m
            };
            const int ExpectedTotalRow = 100;

            ConfigureReaderForTotalData(expectedTotalData, ExpectedTotalRow);
            ConfigureReaderForResult(expectedResult);

            // Action
            var result = DataAccess.GetDataList(_metaModel);
            var item = result.DataList.First();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(item.CustId, Is.EqualTo(expectedResult.CustId));
                Assert.That(item.ExternalId, Is.EqualTo(expectedResult.ExternalId));
                Assert.That(item.SystemId, Is.EqualTo(expectedResult.SystemId));
                Assert.That(item.BetCounts, Is.EqualTo(expectedResult.BetCounts));
                Assert.That(item.PendingFunds, Is.EqualTo(expectedResult.PendingFunds));

                Assert.That(result.TotalData.BetCounts, Is.EqualTo(expectedTotalData.BetCounts));
                Assert.That(result.TotalData.PendingFunds, Is.EqualTo(expectedTotalData.PendingFunds));

                Assert.That(result.TotalRows, Is.EqualTo(ExpectedTotalRow));
            });
        }

        [Test]
        public void GetDataList_NoData_ReturnEmptyResult()
        {
            // Arrange
            Reader.Read().Returns(false);
            Reader.NextResult().Returns(false);

            // Action
            var result = DataAccess.GetDataList(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList, Is.Empty);
                Assert.That(result.TotalRows, Is.Zero);
            });
        }

        [Test]
        public void GetDataList_NoTotalDataReturned_DataReturnedCorrectly()
        {
            // Arrange
            var expectedResult = new CustPendingFunds
            {
                ExternalId = "ExternalIdTest",
                SystemId = "SystemIdTest"
            };

            Reader.Read().Returns(true, false);
            Reader.NextResult().Returns(false);
            ConfigureReaderForResult(expectedResult);

            // Action
            var result = DataAccess.GetDataList(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList, Is.Not.Empty);
                Assert.That(result.TotalRows, Is.Zero);
            });
        }

        [Test]
        public void GetDataList_WhenObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                SpName,
                () => DataAccess.GetDataList(_metaModel));
        }

        private IList<ProductOption> GetProductOptions()
        {
            return new List<ProductOption>
            {
                new ProductOption { Code = "SB", Status = true },
                new ProductOption { Code = "NG", Status = false }
            };
        }

        private void ConfigureReaderForResult(CustPendingFunds expected)
        {
            var outstandingColumn = "Outstanding" + _metaModel.BaseCurrency;

            Reader["CustID"].Returns(expected.CustId);
            Reader["nickname"].Returns(expected.ExternalId);
            Reader["username"].Returns(expected.SystemId);
            Reader["BetCounts"].Returns(expected.BetCounts);
            Reader[outstandingColumn].Returns(expected.PendingFunds);
        }

        private void ConfigureReaderForTotalData(CustPendingFunds expected, int expectedTotalRow)
        {
            var outstandingColumn = "SumOutstanding" + _metaModel.BaseCurrency;

            Reader["SumBetCounts"].Returns(expected.BetCounts);
            Reader[outstandingColumn].Returns(expected.PendingFunds);

            Reader["TotalRows"].Returns(expectedTotalRow);
        }
    }
}