namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core.Configuration.Interfaces;
    using Data;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Configuration;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class PlatformWinLossDataAccessTest
    {
        private PlatformWinLossDataAccess _dataAccess;
        private BalanceUp _balance;
        private PlatformWinLossMetaModel _metaModel;

        [OneTimeSetUp]
        public void Setup()
        {
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _balance = new BalanceUp
            {
                MoveBetsDay = new DateTime(2016, 8, 10),
                BalanceUpDay = new DateTime(2016, 8, 31)
            };
            balanceUpDataAccess.GetBalanceUp().Returns(_balance);

            _dataAccess = Substitute.ForPartsOf<PlatformWinLossDataAccess>(balanceUpDataAccess);

            _metaModel = new PlatformWinLossMetaModel
            {
                LicenseeId = 1,
                BaseCurrency = "USD",
                AgentListXml = "<xml>",
                Year = 2017,
                Month = 1,
                FromDay = 1,
                ToDay = 2
            };
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _dataAccess = null;
            _metaModel = null;
            _balance = null;
        }

        [Test]
        public void GetSummaryData_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_PlatformWinloss")).Returns(objectDb);

            // Action
            _dataAccess.GetSummaryData(_metaModel);

            // Assert
            objectDb.Received().AddInParameter("PlatformType", DbType.Byte, _metaModel.PlatformType);
            objectDb.Received().AddInParameter("movebetsday", DbType.DateTime, _balance.MoveBetsDay);
            objectDb.Received().AddInParameter("DrawFilter", DbType.Int16, _metaModel.DrawFilter);
            objectDb.Received().AddInParameter("FromDate", DbType.DateTime, _metaModel.FromDate);
            objectDb.Received().AddInParameter("ToDate", DbType.DateTime, _metaModel.ToDate);
            objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
        }

        [Test]
        public void GetSummaryData_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_PlatformWinloss")).Returns(objectDb);
            objectDb.ExecuteReader().Returns(reader);
            reader.Read().Returns(true, false);
            var readerDataMapping = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "PlatformType", PlatformType.Asia },
                { "TurnoverUSD", 100 },
                { "productprefix", "SB" }
            };

            DataAccessTestHelper.MapReaderData(reader, readerDataMapping);

            // Action
            var result = _dataAccess.GetSummaryData(_metaModel);
            var firstItem = result.DataList.First();
            var secondItem = result.DataList.ElementAt(1);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList.Count, Is.EqualTo(2));
                Assert.That(firstItem.PlatformType, Is.EqualTo(PlatformType.Asia));
                Assert.That(firstItem.Name, Is.EqualTo(ViewResources.Asia));
                Assert.That(firstItem.Turnover, Is.EqualTo(100));

                Assert.That(secondItem.Parent, Is.EqualTo(PlatformType.Asia.ToString()));
                Assert.That(secondItem.Name, Is.EqualTo(ViewResources.ProductSB));
                Assert.That(secondItem.Turnover, Is.EqualTo(100));

                Assert.That(result.TotalData.TurnoverPercent, Is.EqualTo(100));
            });
        }

        [Test]
        public void GetMemberData_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_PlatformWinlossDetail")).Returns(objectDb);

            // Action
            _dataAccess.GetMemberData(_metaModel);

            // Assert
            objectDb.Received().AddInParameter("PlatformType", DbType.Byte, _metaModel.PlatformType);
            objectDb.Received().AddInParameter("movebetsday", DbType.DateTime, _balance.MoveBetsDay);
            objectDb.Received().AddInParameter("DrawFilter", DbType.Int16, _metaModel.DrawFilter);
            objectDb.Received().AddInParameter("FromDate", DbType.DateTime, _metaModel.FromDate);
            objectDb.Received().AddInParameter("ToDate", DbType.DateTime, _metaModel.ToDate);
            objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
        }

        [Test]
        public void GetMemberData_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_PlatformWinlossDetail")).Returns(objectDb);
            objectDb.ExecuteReader().Returns(reader);
            reader.Read().Returns(true, false);
            var readerDataMapping = new
            {
                TurnoverUSD = 100,
                WinlostUSD = 200
            };

            DataAccessTestHelper.MapReaderData(reader, readerDataMapping);

            // Action
            var result = _dataAccess.GetMemberData(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList.Count, Is.EqualTo(1));
                Assert.That(result.DataList.ElementAt(0).Turnover, Is.EqualTo(100));
                Assert.That(result.DataList.ElementAt(0).CustomerWinLoss, Is.EqualTo(200));

                Assert.That(result.TotalData.Turnover, Is.EqualTo(100));
            });
        }
    }
}