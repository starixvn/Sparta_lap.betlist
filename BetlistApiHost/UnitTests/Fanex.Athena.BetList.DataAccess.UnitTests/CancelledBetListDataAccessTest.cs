namespace Fanex.Athena.BetList.DataAccess.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Athena.BetList.DataAccess;
    using Athena.BetList.Model.DTO;
    using Common.DataAccess.UnitTests;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Extensions;

    public class CancelledBetListDataAccessTest : BaseDataAccessHelperTest<CancelledBetListDataAccess>
    {
        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            DataAccess.GetObjectDb(Arg.Is("Lrf_BetList_SelDeleted_UserList")).Returns(ObjectDb);
        }

        [Test]
        public async Task GetCancelBetCustList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            const string AgentList = "Sdfsdfsdf";
            var expected = GetFakeDataReturn();
            ObjectDb.QueryAsync<CustomerCancelBetItem>(Arg.Any<object>()).Returns(new List<CustomerCancelBetItem> { expected });

            // Action
            var result = (await DataAccess.GetCancelBetCustList(AgentList)).ElementAt(0);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
            await ObjectDb.Received(1).QueryAsync<CustomerCancelBetItem>(Arg.Is<object>(x => (string)x.GetValue("AgentList") == AgentList));
        }

        private CustomerCancelBetItem GetFakeDataReturn()
        {
            return new CustomerCancelBetItem
            {
                CustId = 1,
                UserName = "UserNameTest",
                NickName = "NickNameTest",
                BetCount = 10
            };
        }
    }
}