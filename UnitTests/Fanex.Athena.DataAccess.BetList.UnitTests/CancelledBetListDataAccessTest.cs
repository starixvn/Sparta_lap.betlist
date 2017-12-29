namespace Fanex.Athena.DataAccess.BetList.UnitTests
{
    using System.Collections.Generic;
    using System.Data;
    using Core.UnitTests.TestHelpers;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class CancelledBetListDataAccessTest : BaseDataAccessHelperTest<CancelledBetListDataAccess>
    {
        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            DataAccess.GetObjectDb(Arg.Is("Lrf_BetList_SelDeleted_UserList")).Returns(ObjectDb);
        }

        [Test]
        public void GetCancelBetCustList_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var metaModel = GetFakeBetListMetaModel();

            // Action
            DataAccess.GetCancelBetCustList(metaModel);

            // Assert
            ObjectDb.Received().AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
        }

        [Test]
        public void GetCancelBetCustList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            var metaModel = GetFakeBetListMetaModel();
            var expected = GetFakeDataReturn();
            ObjectDb.Query<CustomerCancelBetItem>().Returns(new List<CustomerCancelBetItem> { expected });

            // Action
            var result = DataAccess.GetCancelBetCustList(metaModel)[0];

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        private BetListMetaModel GetFakeBetListMetaModel()
        {
            return new BetListMetaModel
            {
                LicenseeId = 1,
                UserId = 2,
                CurrencyId = 3
            };
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