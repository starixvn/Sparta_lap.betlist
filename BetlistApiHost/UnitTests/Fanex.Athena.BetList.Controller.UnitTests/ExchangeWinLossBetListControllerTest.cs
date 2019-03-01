namespace Fanex.Athena.BetList.Controller.UnitTests
{
    using Model.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Interfaces;
    using UnitTest.Common.Helpers;

    public class ExchangeWinLossBetListControllerTest
    {
        private IExchangeBetListService _service;

        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            _service = Substitute.For<IExchangeBetListService>();
        }

        [OneTimeTearDown]
        public void FixtureTearDown()
        {
            _service = Substitute.For<IExchangeBetListService>();
        }

        public void GetData_WhenCall_CallServiceToGetData()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<ExchangeBetListMetaModel>();

                // Action
                controller.GetData(metaModel);

                // Assert
                _service.Received().GetBetListData(metaModel);
            }
        }

        public void Detail_WhenCall_CallServiceToGetData()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<ExchangeBetListDetailMetaModel>();

                // Action
                controller.Detail(metaModel);

                // Assert
                _service.Received().GetTicketDetailList(metaModel);
            }
        }

        private ExchangeWinLossBetListController CreateController()
        {
            return new ExchangeWinLossBetListController(_service);
        }
    }
}