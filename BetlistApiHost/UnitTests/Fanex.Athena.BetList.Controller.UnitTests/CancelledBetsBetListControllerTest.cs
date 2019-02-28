namespace Fanex.Athena.BetList.Controller.UnitTests
{
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
        public void GetCustomerList_WhenCall_GetDataFromDataAccess()
        {
            using (var controller = new CancelledBetsBetListController(_cancelledBetListDataAccess))
            {
                // Arrange
                var agentListXml = FakeHelper.Create<string>();

                // Action
                controller.GetCustomerList(agentListXml);

                // Assert
                _cancelledBetListDataAccess.Received().GetCancelBetCustList(agentListXml);
            }
        }
    }
}