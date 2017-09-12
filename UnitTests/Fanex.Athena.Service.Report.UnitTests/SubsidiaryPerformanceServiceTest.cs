namespace Fanex.Athena.Service.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Web.Script.Serialization;
    using DataAccess.Report.Interfaces;
    using Model.Report.DTO.SubsidiaryPerformance;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SubsidiaryPerformanceServiceTest
    {
        private ISubsidiaryPerformanceDataAccess _dataAccess;
        private SubsidiaryPerformanceService _service;
        private JavaScriptSerializer _serializer;

        [SetUp]
        public void SetUp()
        {
            _dataAccess = Substitute.For<ISubsidiaryPerformanceDataAccess>();
            _service = new SubsidiaryPerformanceService(_dataAccess);
            _serializer = new JavaScriptSerializer();
        }

        [TearDown]
        public void TearDown()
        {
            _dataAccess = null;
            _service = null;
        }

        [Test]
        public void GetData_WhenCall_CallDataAccess()
        {
            // Arrange
            var metaModel = new SubsidiaryPerformanceMetaModel
            {
                FromDate = new DateTime(2017, 8, 30),
                ToDate = new DateTime(2017, 8, 30),
                ProductCodeList = "SB,BA",
                SubsidiaryList = "123,143",
                BaseCurrency = "EUR"
            };
            IEnumerable<SubsidiaryPerformanceEntity> totalDataList = null;
            _dataAccess.GetData(metaModel, out totalDataList).Returns(x =>
            {
                x[1] = new List<SubsidiaryPerformanceEntity>();
                return new List<SubsidiaryPerformanceEntity>();
            });

            // Action
            _service.GetData(metaModel, out totalDataList);

            // Assert
            _dataAccess.ReceivedWithAnyArgs().GetData(metaModel, out totalDataList);
        }

        [Test]
        public void GetData_MetaModelIsValid_GetCorrectTotalDataList()
        {
            // Arrange
            var metaModel = new SubsidiaryPerformanceMetaModel
            {
                FromDate = new DateTime(2017, 8, 30),
                ToDate = new DateTime(2017, 8, 30),
                ProductCodeList = "SB,BA",
                SubsidiaryList = "123,143",
                BaseCurrency = "EUR"
            };
            var fakeEntityDataList = GetFakeEntityDataList();
            IEnumerable<SubsidiaryPerformanceEntity> totalDataList = null;
            var expectedResult = _serializer.Serialize(GetExpectedTotalDataList());

            _dataAccess.GetData(metaModel, out totalDataList).Returns(x =>
            {
                x[1] = GetFakeEntityDataList();
                return fakeEntityDataList;
            });

            // Action
            _service.GetData(metaModel, out totalDataList);
            var result = _serializer.Serialize(totalDataList);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        private static IEnumerable<SubsidiaryPerformanceEntity> GetFakeEntityDataList()
        {
            return new List<SubsidiaryPerformanceEntity>
            {
                new SubsidiaryPerformanceEntity
                { SubsidiaryId = 1234, Subsidiary = "New York", CurrencyId = 1, CurrencyName = "USD", Product = "SB", BetCount = 50, Turnover = 100000m, CustomerWinLoss = 10000m, SubsidiaryWinLoss = -10000m},
                new SubsidiaryPerformanceEntity
                { SubsidiaryId = 1234, Subsidiary = "New York", CurrencyId = 1, CurrencyName = "USD", Product = "BA", BetCount = 50, Turnover = 100000m, CustomerWinLoss = 10000m, SubsidiaryWinLoss = -10000m},
                new SubsidiaryPerformanceEntity
                { SubsidiaryId = 1234, Subsidiary = "New York", CurrencyId = 1, CurrencyName = "USD", Product = "RC", BetCount = 50, Turnover = 100000m, CustomerWinLoss = 10000m, SubsidiaryWinLoss = -10000m},
                new SubsidiaryPerformanceEntity
                { SubsidiaryId = 1235, Subsidiary = "India", CurrencyId = 6, CurrencyName = "EUR", Product = "SB", BetCount = 50, Turnover = 100000m, CustomerWinLoss = 10000m, SubsidiaryWinLoss = -10000m},
                new SubsidiaryPerformanceEntity
                { SubsidiaryId = 1235, Subsidiary = "India", CurrencyId = 1, CurrencyName = "USD", Product = "SB", BetCount = 50, Turnover = 100000m, CustomerWinLoss = 10000m, SubsidiaryWinLoss = -10000m}
            };
        }

        private static IEnumerable<SubsidiaryPerformanceEntity> GetExpectedTotalDataList()
        {
            return new List<SubsidiaryPerformanceEntity>
            {
                new SubsidiaryPerformanceEntity
                { SubsidiaryId = 1234, Subsidiary = "1234", CurrencyName = "EUR", BetCount = 150, Turnover = 300000m, CustomerWinLoss = 30000m, SubsidiaryWinLoss = -30000m},
                new SubsidiaryPerformanceEntity
                { SubsidiaryId = 1235, Subsidiary = "1235", CurrencyName = "EUR", BetCount = 100, Turnover = 200000m, CustomerWinLoss = 20000m, SubsidiaryWinLoss = -20000m}
            };
        }
    }
}