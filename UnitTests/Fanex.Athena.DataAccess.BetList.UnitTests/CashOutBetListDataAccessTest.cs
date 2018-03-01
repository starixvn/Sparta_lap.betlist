namespace Fanex.Athena.DataAccess.BetList.UnitTests
{
    using System.Data;
    using System.Linq;
    using Core.UnitTests.TestHelpers;
    using Core.Utilities;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.NUnitExtensions;

    public class CashOutBetListDataAccessTest : BaseDataAccessHelperTest<CashOutBetListDataAccess>
    {
        private const string UspName = "Lrf_BetList_CashOut_Detail";

        [Test]
        public void GetDetail_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var metaModel = GetFakeBetListMetaModel();
            var expectedParam = new
            {
                metaModel.TransId,
                metaModel.RefNo,
                metaModel.BetType,
                OriginalTransId = metaModel.BetdaqId,
                AgentList = metaModel.AgentListXml
            };

            DataAccess.GetObjectDb(Arg.Is(UspName)).Returns(ObjectDb);
            object param = null;
            ObjectDb.ExecuteReader(Arg.Do<object>(x => param = x));

            // Action
            DataAccess.GetDetail(metaModel);

            // Assert
            Assert.That(param, Tobe.SameProperties(expectedParam));
        }

        [Test]
        public void GetDetail_WhenCall_OriginalTicketReturnCorrectly()
        {
            // Arrange
            const string UserName = "user1";
            const int PlayerCommUsd = 999;

            var safeReader = Substitute.For<ISafeDataReader>();
            DataAccess.GetObjectDb(Arg.Is(UspName)).Returns(ObjectDb);
            DataAccess.GetSafeDataReader(Arg.Any<IDataReader>(), Arg.Any<string[]>()).ReturnsForAnyArgs(safeReader);

            safeReader.Read().Returns(true, false);

            safeReader.GetString("username").Returns(UserName);
            safeReader.GetDecimal("playercommusd").Returns(PlayerCommUsd);

            // Action
            var result = DataAccess.GetDetail(new CashOutBetListDetailMetaModel
            {
                BaseCurrency = "usd"
            });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.OriginalTicket.UserName, Is.EqualTo(UserName));
                Assert.That(result.OriginalTicket.PlayerComm, Is.EqualTo(PlayerCommUsd));
            });
        }

        [Test]
        public void GetDetail_WhenCall_CashOutTicketListReturnCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(UspName)).Returns(ObjectDb);

            Reader.Read().Returns(false, true, false);
            Reader.NextResult().Returns(true);

            var dataReader = new
            {
                BetType = 1,
                StakeUsd = 2
            };
            MapReaderData(dataReader);

            // Action
            var result = DataAccess.GetDetail(new CashOutBetListDetailMetaModel
            {
                BaseCurrency = "usd"
            });

            var firstCashoutTicket = result.CashOutTicketList.First();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(firstCashoutTicket.BetType, Is.EqualTo(dataReader.BetType));
                Assert.That(firstCashoutTicket.Stake, Is.EqualTo(dataReader.StakeUsd));
            });
        }

        [Test]
        public void GetDetail_WhenObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                UspName,
                () => DataAccess.GetDetail(new CashOutBetListDetailMetaModel
                {
                    BaseCurrency = "usd"
                }));
        }

        private CashOutBetListDetailMetaModel GetFakeBetListMetaModel()
        {
            return new CashOutBetListDetailMetaModel
            {
                TransId = 1,
                RefNo = 2,
                BetType = 3,
                BetdaqId = 4,
                AgentListXml = "xml",
                BaseCurrency = "USD"
            };
        }
    }
}