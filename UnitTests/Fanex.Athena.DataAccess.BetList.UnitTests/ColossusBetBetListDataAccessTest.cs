namespace Fanex.Athena.DataAccess.BetList.UnitTests
{
    using System;
    using System.Data;
    using System.Linq;
    using Core.UnitTests.TestHelpers;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class ColossusBetBetListDataAccessTest : BaseDataAccessHelperTest<ColossusBetBetListDataAccess>
    {
        private const string GetSiteIdSp = "Acc_CustomerSelSiteByCustId";
        private const string GetDataSp = "Lrf_BetList_CB_Detail";
        private const int SoldBetType = 18002;

        [Test]
        public void GetSiteId_WhenCall_DataReturnCorrectly()
        {
            // Arrange
            const int CustId = 999;
            const int ExpectedSiteId = 100;
            DataAccess.GetObjectDb(Arg.Is(GetSiteIdSp)).Returns(ObjectDb);
            ObjectDb.GetParameterValue("siteid").Returns(ExpectedSiteId);

            // Action
            var result = DataAccess.GetSiteId(CustId);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("custid", DbType.Int32, CustId);
                ObjectDb.Received().AddOutParameter("siteid", DbType.Int32, 0);

                Assert.That(result, Is.EqualTo(ExpectedSiteId));
            });
        }

        [Test]
        public void GetSiteId_WhenObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                GetSiteIdSp,
                () => DataAccess.GetSiteId(1));
        }

        [TestCase(true, 1)]
        [TestCase(false, 0)]
        public void GetData_WhenCall_DataReturnCorrectly(bool isSoldBetType, decimal expectedTotalSoldWinLoss)
        {
            // Arrange
            var metaModel = new ColossusBetDetailMetaModel
            {
                BaseCurrency = "USD",
                AgentListXml = "<xml>",
                CustId = 999,
                RefNo = 1000
            };
            DataAccess.GetObjectDb(Arg.Is(GetDataSp)).Returns(ObjectDb);
            Reader.Read().Returns(true, false);

            var readerDataMapping = new
            {
                TransDate = "1/1/2017",
                CommUSD = 123,
                BetType = isSoldBetType ? SoldBetType : 0,
                WinlostUSD = isSoldBetType ? expectedTotalSoldWinLoss : 1234
            };
            MapReaderData(readerDataMapping);

            // Action
            var result = DataAccess.GetData(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList.Count(), Is.EqualTo(1));
                Assert.That(result.DataList.ElementAt(0).TransDate, Is.EqualTo(new DateTime(2017, 1, 1)));
                Assert.That(result.DataList.ElementAt(0).Commission, Is.EqualTo(123));
                Assert.That(result.TotalComm, Is.EqualTo(123));
                Assert.That(result.TotalSoldWinLoss, Is.EqualTo(expectedTotalSoldWinLoss));
            });
        }

        [Test]
        public void GetData_WhenObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                GetDataSp,
                () => DataAccess.GetData(new ColossusBetDetailMetaModel()));
        }
    }
}