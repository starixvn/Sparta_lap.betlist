namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Configuration.Interfaces;
    using Core.UnitTests.TestHelpers;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Configuration;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;
    using UnitTest.Common.NUnitExtensions;

    [TestFixture]
    public class CasinoTurnoverDataAccessTest : BaseDataAccessHelperTest<CasinoTurnoverDataAccess>
    {
        private readonly DateTime _moveBetsDay = new DateTime(2018, 3, 5);

        private readonly CasinoTurnoverMetaModel _dummyMetaModel = new CasinoTurnoverMetaModel
        {
            IsConvertAll = false,
            Status = new[] { 0 },
            ProductCode = new[] { string.Empty }
        };

        protected override void SetUpDataAccess()
        {
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            balanceUpDataAccess.GetBalanceUp().Returns(new BalanceUp
            {
                MoveBetsDay = _moveBetsDay
            });

            DataAccess = Substitute.ForPartsOf<CasinoTurnoverDataAccess>(balanceUpDataAccess);
        }

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            DataAccess.GetObjectDb(Arg.Is("Lrf_Rpt_CasinoTurnover")).Returns(ObjectDb);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetData_WhenCall_ParametersPassedCorrectly(bool isExportExcel)
        {
            // Arrange
            var metaModel = FakeHelper.Create<CasinoTurnoverMetaModel>(x => x.IsExportExcel, isExportExcel);

            // Action
            DataAccess.GetData(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("fromdate", metaModel.FromDate.Date);
                ObjectDb.Received(1).SetParameterValue("todate", metaModel.ToDate.Date);
                ObjectDb.Received(1).SetParameterValue("basecurrency", metaModel.BaseCurrency);
                ObjectDb.Received(1).SetParameterValue("UserName", metaModel.UserName);
                ObjectDb.Received(1).SetParameterValue("StatusList", string.Join(",", metaModel.Status));
                ObjectDb.Received(1).SetParameterValue("ProductCodes", string.Join(",", metaModel.ProductCode));
                ObjectDb.Received(1).SetParameterValue("MoveBetsDay", _moveBetsDay);
                ObjectDb.Received(1).SetParameterValue("AgentList", metaModel.AgentListXml);

                ObjectDb.Received(1).SetParameterValue("PageSize", metaModel.IsExportExcel ? SystemConstants.MaxDataTotalRow : metaModel.PageSize);
                ObjectDb.Received(1).SetParameterValue("PageIndex", metaModel.IsExportExcel ? 1 : metaModel.PageIndex);
                ObjectDb.Received(1).SetParameterValue("TotalRows", null);
            });
        }

        [Test]
        public async Task GetData_NoData_ReturnEmptyResult()
        {
            // Arrange
            Reader.Read().Returns(false);
            Reader.NextResult().Returns(false);

            // Action
            var result = await DataAccess.GetData(_dummyMetaModel).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList, Is.Empty);
                Assert.That(result.TotalRows, Is.Zero);
                Assert.That(result.TotalData, Is.Null);
            });
        }

        [Test]
        public async Task GetData_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            var expectedResult = FakeHelper.Create<CasinoTurnover>(x => x.Product, "Number Game");
            expectedResult.Index = 1;
            expectedResult.RowSpan = 1;

            Reader.Read().Returns(true, false, true, false);
            Reader.NextResult().Returns(true);
            DataAccessTestHelper.MapReaderData(
                Reader, 
                new
                {
                    Date = expectedResult.Date,
                    UserName = expectedResult.SystemId,
                    NickName = expectedResult.ExternalId,
                    ProductCode = "NG",
                    BetCount = expectedResult.BetCount,
                    Turnover = expectedResult.Turnover,
                    ValidBetAmount = expectedResult.ValidBetAmount,
                    Winlost = expectedResult.CustomerWL
                });

            // Action
            var result = await DataAccess.GetData(_dummyMetaModel).ConfigureAwait(false);
            var firstItem = result.DataList.First();

            // Assert
            Assert.That(firstItem, Tobe.SameProperties(expectedResult));
        }
    }
}