namespace Fanex.Athena.BetList.Controller.UnitTests
{
    using System.Threading.Tasks;
    using Fanex.Athena.BetList.DataAccess.Interfaces;
    using Model.DTO;
    using Model.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public class OutstandingDailyStatementBetListControllerTest
    {
        private IDailyStatementBetListDataAccess _dailyStatementBetListDataAccess;

        [SetUp]
        public void SetUp()
        {
            _dailyStatementBetListDataAccess = Substitute.For<IDailyStatementBetListDataAccess>();
        }

        [TearDown]
        public void TearDown()
        {
            _dailyStatementBetListDataAccess = null;
        }

        [Test]
        public async Task GetData_WhenCall_ReturnCorrectData()
        {
            using (var controller = new OutstandingDailyStatementBetListController(_dailyStatementBetListDataAccess))
            {
                // Arrange
                var metaModel = FakeHelper.Create<BetListMetaModel>();

                var dataList = FakeHelper.CreateMany<DailyStatement>(2);
                _dailyStatementBetListDataAccess.GetDailyStatementData(metaModel).Returns(dataList);

                // Action
                var result = (await controller.GetData(metaModel)).Data;
                var main = result.GetType().GetProperty("Main").GetValue(result, null);
                var total = result.GetType().GetProperty("Total").GetValue(result, null);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(main, Is.EqualTo(dataList));
                    Assert.That(total.GetType().GetProperty("SumWinLoss").GetValue(total, null), Is.Zero);
                    Assert.That(total.GetType().GetProperty("SumLicenseeWl").GetValue(total, null), Is.Zero);
                });
            }
        }
    }
}