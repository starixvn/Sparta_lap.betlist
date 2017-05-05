namespace Fanex.Athena.DataAccess.BetList.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Data;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ColossusBetBetListDataAccessTest
    {
        private IObjectDb _objectDb;
        private ColossusBetBetListDataAccess _dataAccess;

        [OneTimeSetUp]
        public void SetUp()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<ColossusBetBetListDataAccess>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
        }

        [Test]
        public void GetSiteId_WhenCall_DataReturnCorrectly()
        {
            // Arrange
            var custId = 999;
            var expectedSiteId = 100;
            _dataAccess.GetObjectDb(Arg.Is<string>("Acc_CustomerSelSiteByCustId")).Returns(_objectDb);
            _objectDb.GetParameterValue("siteid").Returns(expectedSiteId);

            // Action
            var result = _dataAccess.GetSiteId(custId);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received().AddInParameter("custid", DbType.Int32, custId);
                _objectDb.Received().AddOutParameter("siteid", DbType.Int32, 0);

                Assert.That(result, Is.EqualTo(expectedSiteId));
            });
        }

        [Test]
        public void GetData_WhenCall_DataReturnCorrectly()
        {
            // Arrange
            var metaModel = new ColossusBetDetailMetaModel
            {
                BaseCurrency = "USD",
                AgentListXml = "<xml>",
                CustId = 999,
                RefNo = 1000
            };
            var reader = Substitute.For<IDataReader>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_BetList_CB_Detail")).Returns(_objectDb);
            _objectDb.ExecuteReader().ReturnsForAnyArgs(reader);
            reader.Read().Returns(true, false);

            var readerDataMapping = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "TransDate", "1/1/2017" },
                { "commUSD", 123 }
            };

            MapReaderData(reader, readerDataMapping);

            // Action
            var result = _dataAccess.GetData(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList.Count(), Is.EqualTo(1));
                Assert.That(result.DataList.ElementAt(0).TransDate, Is.EqualTo(new DateTime(2017, 1, 1)));
                Assert.That(result.DataList.ElementAt(0).Commission, Is.EqualTo(123));
                Assert.That(result.TotalComm, Is.EqualTo(123));
            });
        }

        private static void MapReaderData(IDataReader reader, Dictionary<string, object> readerDataMapping)
        {
            reader[Arg.Any<string>()].Returns(x =>
            {
                var paramName = x.ArgAt<string>(0);
                if (readerDataMapping.ContainsKey(paramName))
                {
                    return readerDataMapping[paramName];
                }

                return string.Empty;
            });
        }
    }
}