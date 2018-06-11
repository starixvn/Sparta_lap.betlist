namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using Core.Configuration.Interfaces;
    using Core.UnitTests.TestHelpers;
    using Fanex.Athena.UnitTest.Common.NUnitExtensions;
    using Model.Core.DTO.Configuration;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class RacingCommissionDataAccessTest : BaseDataAccessHelperTest<RacingCommissionDataAccess>
    {
        private const string SpName = "Lrf_Rpt_CommByBettype_HR";

        private readonly BalanceUp _balance = new BalanceUp
        {
            MoveBetsDay = new DateTime(2016, 8, 10),
            BalanceUpDay = new DateTime(2016, 8, 31)
        };

        private readonly RacingCommissionMetaModel _metaModel = new RacingCommissionMetaModel
        {
            LicenseeId = 1,
            BaseCurrency = "USD",
            AgentListXml = "xml",
            FromDate = new DateTime(2017, 1, 1),
            ToDate = new DateTime(2017, 2, 2),
            PageIndex = 2,
            PageSize = 100
        };

        protected override void SetUpDataAccess()
        {
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();

            balanceUpDataAccess.GetBalanceUp().Returns(_balance);

            DataAccess = Substitute.ForPartsOf<RacingCommissionDataAccess>(
                AppSetting,
                CreateSessionStorage(),
                balanceUpDataAccess);
        }

        [Test]
        public void GetDataList_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);

            dynamic uspParam = null;
            ObjectDb.ExecuteReader(Arg.Do<dynamic>(arg => uspParam = arg));
            var expectedResult = new
            {
                AgentList = "xml",
                FromDate = _metaModel.FromDate,
                ToDate = _metaModel.ToDate,
                MoveBetsDay = _balance.MoveBetsDay,
                PageSize = _metaModel.PageSize,
                PageIndex = _metaModel.PageIndex
            };

            // Action
            DataAccess.GetData(_metaModel);

            // Assert
            Assert.That(uspParam, Tobe.SameProperties(expectedResult));
        }

        [Test]
        public void GetDataList_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);

            Reader.Read().Returns(true, false, true, false);
            Reader.NextResult().Returns(true);
            var readerDataMapping = new
            {
                custID = 999,
                Tote_WinUSD = 100,
                SumTote_WinUSD = 1000
            };

            MapReaderData(readerDataMapping);

            // Action
            var result = DataAccess.GetData(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList.Count, Is.EqualTo(1));
                Assert.That(result.DataList[0].CustID, Is.EqualTo(999));
                Assert.That(result.DataList[0].ToteWin, Is.EqualTo(100));

                Assert.That(result.TotalData.ToteWin, Is.EqualTo(1000));
            });
        }

        [Test]
        public void GetDataList_WhenReceiveObjectDbException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                SpName,
                () => DataAccess.GetData(_metaModel));
        }
    }
}