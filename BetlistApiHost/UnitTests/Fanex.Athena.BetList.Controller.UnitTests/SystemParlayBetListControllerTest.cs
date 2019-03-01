namespace Fanex.Athena.BetList.Controller.UnitTests
{
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Model.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Interfaces;
    using UnitTest.Common.Helpers;

    public class SystemParlayBetListControllerTest
    {
        [Test]
        public void Detail_WhenCall_CallCorrectMethod()
        {
            var ticketHelper = Substitute.For<ITicketHelper>();
            var betListBuilder = Substitute.For<IBetListHTMLBuilder>();
            var betListService = Substitute.For<IBetListService>();

            using (var controller = new SystemParlayBetListController(ticketHelper, betListBuilder, betListService))
            {
                // Arrang
                var metaModel = FakeHelper.Create<SystemParlayDetailMetaModel>();

                var systemParlayData = new SystemParlayData();
                betListService.GetSystemParlayDetail(metaModel).Returns(systemParlayData);

                // Action
                var result = controller.Detail(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    betListService.Received().GetSystemParlayDetail(metaModel);
                    betListBuilder.Received().BuildSystemParlayDetail(ticketHelper, systemParlayData);
                });
            }
        }
    }
}