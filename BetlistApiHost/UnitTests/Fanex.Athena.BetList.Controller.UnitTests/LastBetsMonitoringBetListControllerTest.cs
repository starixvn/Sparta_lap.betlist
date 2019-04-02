namespace Fanex.Athena.BetList.Controller.UnitTests
{
    using System.Threading.Tasks;
    using Common.Utility.Collections;
    using Fanex.BetList.Core.Entities;
    using Model.DTO;
    using Model.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Interfaces;
    using UnitTest.Common.Helpers;

    public class LastBetsMonitoringBetListControllerTest : BaseControllerHelperTest
    {
        private ILastBetsMonitoringBetListService _lastBetsMonitoringBetListService;

        private ITicketHelper _ticketHelper;

        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            _lastBetsMonitoringBetListService = Substitute.For<ILastBetsMonitoringBetListService>();

            _ticketHelper = Substitute.For<ITicketHelper>();
        }

        [Test]
        public async Task GetData_NoData_ReturnNoNewData()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<LastBetsMonitoringMetaModel>();

                _lastBetsMonitoringBetListService.GetLastBetsMonitoringData(metaModel, metaModel.UserCurrencyList).Returns(new TicketResult());

                // Action
                var result = (await controller.GetData(metaModel)).Data;

                // Assert
                Assert.That(result.GetType().GetProperty("NewRow").GetValue(result, null), Is.False);
            }
        }

        [Test]
        public async Task GetData_HasData_ReturnCorrectData()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<LastBetsMonitoringMetaModel>();

                var ticket = FakeHelper.Create<LrfTicket>();
                ticket.BetTypeId = 1;
                var ticketResult = new TicketResult { MainTicketList = new SortableList<LrfTicket> { ticket } };
                _lastBetsMonitoringBetListService.GetLastBetsMonitoringData(metaModel, metaModel.UserCurrencyList).Returns(ticketResult);

                // Action
                var result = (await controller.GetData(metaModel)).Data;

                // Assert
                Assert.Multiple(() => 
                {
                    AssertClearData(ticketResult);
                    Assert.That(result.GetType().GetProperty("NewRow").GetValue(result, null), Is.True);
                    Assert.That(result.GetType().GetProperty("Rows").GetValue(result, null), Is.Not.Null);
                    Assert.That(result.GetType().GetProperty("LastTransId").GetValue(result, null), Is.EqualTo(ticket.TransId));
                });
            }
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(12)]
        [TestCase(20)]
        public async Task GetData_BuildOdds_DecimalOddsBetType_BuildCorrectOddString(int betType)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<LastBetsMonitoringMetaModel>();

                var ticket = FakeHelper.Create<LrfTicket>();
                ticket.BetTypeId = betType;
                var ticketResult = new TicketResult { MainTicketList = new SortableList<LrfTicket> { ticket } };
                _lastBetsMonitoringBetListService.GetLastBetsMonitoringData(metaModel, metaModel.UserCurrencyList).Returns(ticketResult);

                // Action
                var result = (await controller.GetData(metaModel)).Data;
                var data = (string)result.GetType().GetProperty("Rows").GetValue(result, null);

                // Assert
                Assert.That(data.Contains("underdog"), Is.True);
            }
        }

        private LastBetMonitoringBetListController CreateController()
        {
            return new LastBetMonitoringBetListController(
                 _lastBetsMonitoringBetListService,
                 _ticketHelper);
        }
    }
}