namespace Fanex.Athena.BetList.Controller.UnitTests
{
    using System.Threading.Tasks;
    using DataAccess.Interfaces;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public class CancelledBetsBetListControllerTest
    {
        private ICancelledBetListDataAccess _cancelledBetListDataAccess;

        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            _cancelledBetListDataAccess = Substitute.For<ICancelledBetListDataAccess>();
        }

        [Test]
        public async Task GetCustomerList_WhenCall_GetDataFromDataAccess()
        {
            using (var controller = new CancelledBetsBetListController(_cancelledBetListDataAccess))
            {
                // Arrange
                var agentListXml = FakeHelper.Create<string>();

                // Action
                await controller.GetCustomerList(agentListXml).ConfigureAwait(false);

                // Assert
                await _cancelledBetListDataAccess.Received(1).GetCancelBetCustList(agentListXml);
            }
        }
    }
}