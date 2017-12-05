namespace Fanex.Athena.DataAccess.BetList.UnitTests
{
    using System.Data;
    using System.Linq;
    using Core.Utilities;
    using Data;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;
    using UnitTest.Common.NUnitExtensions;

    [TestFixture]
    public class CashOutBetListDataAccessTest
    {
        private IObjectDb _objectDb;
        private CashOutBetListDataAccess _dataAccess;

        [OneTimeSetUp]
        public void SetUp()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<CashOutBetListDataAccess>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
        }

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

            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_BetList_CashOut_Detail")).Returns(_objectDb);
            object param = null;
            _objectDb.ExecuteReader(Arg.Do<object>(x => param = x));

            // Action
            _dataAccess.GetDetail(metaModel);

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
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_BetList_CashOut_Detail")).Returns(_objectDb);
            _dataAccess.GetSafeDataReader(Arg.Any<IDataReader>(), Arg.Any<string[]>()).ReturnsForAnyArgs(safeReader);

            safeReader.Read().Returns(true, false);

            safeReader.GetString("username").Returns(UserName);
            safeReader.GetDecimal("playercommusd").Returns(PlayerCommUsd);

            // Action
            var result = _dataAccess.GetDetail(new CashOutBetListDetailMetaModel
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
            const int BetType = 1;
            const decimal Stake = 2;

            var reader = Substitute.For<IDataReader>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_BetList_CashOut_Detail")).Returns(_objectDb);
            _objectDb.ExecuteReader(null).ReturnsForAnyArgs(reader);

            reader.Read().Returns(false, true, false);
            reader.NextResult().Returns(true);

            DataAccessTestHelper.MapReaderData(
                reader,
                new
                {
                    BetType = BetType,
                    StakeUsd = Stake
                });

            // Action
            var result = _dataAccess.GetDetail(new CashOutBetListDetailMetaModel
            {
                BaseCurrency = "usd"
            });

            var firstCashoutTicket = result.CashOutTicketList.First();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(firstCashoutTicket.BetType, Is.EqualTo(BetType));
                Assert.That(firstCashoutTicket.Stake, Is.EqualTo(Stake));
            });
        }

        private CashOutBetListDetailMetaModel GetFakeBetListMetaModel()
        {
            return new CashOutBetListDetailMetaModel
            {
                TransId = 1,
                RefNo = 2,
                BetType = 3,
                BetdaqId = 4,
                AgentListXml = "xml"
            };
        }
    }
}