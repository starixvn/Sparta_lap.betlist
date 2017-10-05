namespace Fanex.Athena.DataAccess.BetList.UnitTests
{
    using Data;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Extensions;

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
            _objectDb.WhenForAnyArgs(x => x.ExecuteReader()).DoNotCallBase();
            object param = null;
            _objectDb.ExecuteReader(Arg.Do<object>(x => param = x));

            // Action
            _dataAccess.GetDetail(metaModel);

            // Assert
            Assert.That(param, Tobe.SameProperties(expectedParam));
        }

        [Test]
        public void GetDetail_WhenCall_DataReturnCorrectly()
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
            _objectDb.WhenForAnyArgs(x => x.ExecuteReader()).DoNotCallBase();
            object param = null;
            _objectDb.ExecuteReader(Arg.Do<object>(x => param = x));

            // Action
            _dataAccess.GetDetail(metaModel);

            // Assert
            Assert.That(param, Tobe.SameProperties(expectedParam));
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