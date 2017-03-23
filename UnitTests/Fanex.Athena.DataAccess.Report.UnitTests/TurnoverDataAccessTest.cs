namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Data;
    using Model.Core.MetaModel.Product;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class TurnoverDataAccessTest
    {
        private TurnoverDataAccess _dataAccess;

        [OneTimeSetUp]
        public void Setup()
        {
            _dataAccess = Substitute.ForPartsOf<TurnoverDataAccess>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _dataAccess = null;
        }

        [Test]
        public void GetDataList_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var metaModel = new TurnoverMetaModel
            {
                Username = "abc",
                AgentListXml = "xml",
                Year = 2017,
                Month = 3,
                FromDay = 1,
                ToDay = 2,
                ProductOptions = new List<ProductOption>
                {
                    new ProductOption { Code = "SB", Status = true },
                    new ProductOption { Code = "NG", Status = false }
                }
            };
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_TurnOver")).Returns(objectDb);

            // Action
            _dataAccess.GetDataList(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                objectDb.Received().AddInParameter("username", DbType.String, metaModel.Username);
                objectDb.Received().AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                objectDb.Received().AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
                objectDb.Received().AddInParameter("todate", DbType.DateTime, metaModel.ToDate);
                objectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
                objectDb.Received().AddInParameter("showNG", DbType.Boolean, false);
            });
        }

        [Test]
        public void GetDataList_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_TurnOver")).Returns(objectDb);
            objectDb.ExecuteReader().ReturnsForAnyArgs(reader);
            reader.Read().Returns(true, false);
            var readerDataMapping = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "OpeningBalanceUSD", 999 }
            };

            MapReaderData(reader, readerDataMapping);

            // Action
            var result = _dataAccess.GetDataList(new TurnoverMetaModel
            {
                BaseCurrency = "USD"
            });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(1));
                Assert.That(result[0].OpeningBalance, Is.EqualTo(999));
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