namespace Fanex.Athena.BetList.Controller.UnitTests
{
    using System.Threading.Tasks;
    using Fanex.Athena.BetList.Excel;
    using Model.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Interfaces;
    using UnitTest.Common.Helpers;

    public class ExchangeWinLossBetListControllerTest
    {
        private IExchangeBetListService _service;
        private IExchangeExcel _exchangeExcel;

        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            _service = Substitute.For<IExchangeBetListService>();
            _exchangeExcel = Substitute.For<IExchangeExcel>();
        }

        [OneTimeTearDown]
        public void FixtureTearDown()
        {
            _service = null;
            _exchangeExcel = null;
        }

        [Test]
        public async Task GetData_WhenCall_CallServiceToGetData()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<ExchangeBetListMetaModel>();

                // Action
                await controller.GetData(metaModel).ConfigureAwait(false);

                // Assert
                await _service.Received(1).GetBetListDataAsync(metaModel);
            }
        }

        [Test]
        public async Task Detail_WhenCall_CallServiceToGetData()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<ExchangeBetListDetailMetaModel>();

                // Action
                await controller.Detail(metaModel).ConfigureAwait(false);

                // Assert
                await _service.Received().GetTicketDetailListAsync(metaModel);
            }
        }

        private ExchangeWinLossBetListController CreateController()
        {
            return new ExchangeWinLossBetListController(_service, _exchangeExcel);
        }
    }
}