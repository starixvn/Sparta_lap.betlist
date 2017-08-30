namespace Fanex.Athena.Service.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        public void GetData_MetaModelIsValid_CallDataAccess()
        {
            // Arrange
            var metaModel = new SubsidiaryPerformanceMetaModel
            {
                FromDate = new DateTime(2017, 8, 30),
                ToDate = new DateTime(2017, 8, 30)
            };
            IEnumerable<SubsidiaryPerformanceEntity> totalDataList = null;

            // Action
            _service.GetData(metaModel, out totalDataList);

            // Assert
            _dataAccess.Received().GetData(metaModel, out totalDataList);
        }

        [Test]
        public void GetData_MetaModelIsValid_ReturnCorrectDataModel()
        {
            // Arrange
            var metaModel = new SubsidiaryPerformanceMetaModel
            {
                FromDate = new DateTime(2017, 8, 30),
                ToDate = new DateTime(2017, 8, 30)
            };
            var fakeEntityDataList = GetFakeEntityDataList();
            IEnumerable<SubsidiaryPerformanceEntity> totalDataList = null;
            _dataAccess.GetData(metaModel, out totalDataList).Returns(fakeEntityDataList);

            var expectedResult = _serializer.Serialize(GetExpectedSummaryData());

            // Action
            var result = _serializer.Serialize(_service.GetData(metaModel, out totalDataList));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetData_MetaModelIsValid_GetCorrectTotalDataList()
        {
            // Arrange
            var metaModel = new SubsidiaryPerformanceMetaModel
            {
                FromDate = new DateTime(2017, 8, 30),
                ToDate = new DateTime(2017, 8, 30)
            };
            var fakeEntityDataList = GetFakeEntityDataList();
            IEnumerable<SubsidiaryPerformanceEntity> totalDataList = null;
            IEnumerable<SubsidiaryPerformanceEntity> expectedTotalDataList = new List<SubsidiaryPerformanceEntity> { new SubsidiaryPerformanceEntity() };

            _dataAccess.GetData(metaModel, out totalDataList).Returns(x =>
            {
                x[1] = expectedTotalDataList;
                return fakeEntityDataList;
            });

            // Action
            _service.GetData(metaModel, out totalDataList);

            // Assert
            Assert.That(totalDataList, Is.EqualTo(expectedTotalDataList));
        }

        [Test]
        public void GetData_MetaModelIsNotValid_DoNotCallDataAccess()
        {
            // Arrange
            var metaModel = new SubsidiaryPerformanceMetaModel
            {
                FromDate = new DateTime(2017, 8, 30),
                ToDate = new DateTime(2017, 8, 1)
            };
            IEnumerable<SubsidiaryPerformanceEntity> totalDataList = null;

            // Action
            _service.GetData(metaModel, out totalDataList);

            // Assert
            _dataAccess.DidNotReceiveWithAnyArgs().GetData(Arg.Any<SubsidiaryPerformanceMetaModel>(), out totalDataList);
        }

        [Test]
        public void GetData_MetaModelIsNotValid_ReturnEmptyList()
        {
            // Arrange
            var metaModel = new SubsidiaryPerformanceMetaModel
            {
                FromDate = new DateTime(2017, 8, 30),
                ToDate = new DateTime(2017, 8, 1)
            };
            IEnumerable<SubsidiaryPerformanceEntity> totalDataList = null;

            // Action
            var result = _service.GetData(metaModel, out totalDataList);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public void GetData_MetaModelIsNotValid_TotalDataListIsEmpty()
        {
            // Arrange
            var metaModel = new SubsidiaryPerformanceMetaModel
            {
                FromDate = new DateTime(2017, 8, 30),
                ToDate = new DateTime(2017, 8, 1)
            };
            IEnumerable<SubsidiaryPerformanceEntity> totalDataList = null;

            // Action
            _service.GetData(metaModel, out totalDataList);

            // Assert
            Assert.That(totalDataList, Is.Null);
        }

        private static IEnumerable<SubsidiaryPerformanceEntity> GetFakeEntityDataList()
        {
            return new List<SubsidiaryPerformanceEntity>
            {
                new SubsidiaryPerformanceEntity
                { Subsidiary = "New York", Currency = "USD", Product = "Sportsbook", BetCount = 50, Turnover = 100000m, CustomerWinLoss = 10000m, SubsidiaryWinLoss = -10000m},
                new SubsidiaryPerformanceEntity
                { Subsidiary = "New York", Currency = "USD", Product = "BA", BetCount = 50, Turnover = 100000m, CustomerWinLoss = 10000m, SubsidiaryWinLoss = -10000m},
                new SubsidiaryPerformanceEntity
                { Subsidiary = "New York", Currency = "USD", Product = "Racing", BetCount = 50, Turnover = 100000m, CustomerWinLoss = 10000m, SubsidiaryWinLoss = -10000m},
                new SubsidiaryPerformanceEntity
                { Subsidiary = "India", Currency = "EUR", Product = "Sportsbook", BetCount = 50, Turnover = 100000m, CustomerWinLoss = 10000m, SubsidiaryWinLoss = -10000m},
                new SubsidiaryPerformanceEntity
                { Subsidiary = "India", Currency = "USD", Product = "Sportsbook", BetCount = 50, Turnover = 100000m, CustomerWinLoss = 10000m, SubsidiaryWinLoss = -10000m}
            };
        }

        private static IEnumerable<SubsidiaryPerformanceDataModel> GetExpectedSummaryData()
        {
            var detailData1 = new List<SubsidiaryPerformanceDetail>
            {
                new SubsidiaryPerformanceDetail { Product = "Sportsbook", BetCount = 50, Turnover = 100000m, CustomerWinLoss = 10000m, SubsidiaryWinLoss = -10000m},
                new SubsidiaryPerformanceDetail { Product = "BA", BetCount = 50, Turnover = 100000m, CustomerWinLoss = 10000m, SubsidiaryWinLoss = -10000m},
                new SubsidiaryPerformanceDetail { Product = "Racing", BetCount = 50, Turnover = 100000m, CustomerWinLoss = 10000m, SubsidiaryWinLoss = -10000m}
            };

            var detailData2 = new List<SubsidiaryPerformanceDetail>
            {
                new SubsidiaryPerformanceDetail { Product = "Sportsbook", BetCount = 50, Turnover = 100000m, CustomerWinLoss = 10000m, SubsidiaryWinLoss = -10000m}
            };

            return new List<SubsidiaryPerformanceDataModel>
            {
                new SubsidiaryPerformanceDataModel { Subsidiary = "New York", Currency = "USD", Details = detailData1},
                new SubsidiaryPerformanceDataModel { Subsidiary = "India", Currency = "EUR", Details = detailData2},
                new SubsidiaryPerformanceDataModel { Subsidiary = "India", Currency = "USD", Details = detailData2}
            };
        }
    }
}