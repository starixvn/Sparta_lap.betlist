namespace Fanex.Athena.Service.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Web.Script.Serialization;
    using DataAccess.Report.Interfaces;
    using Extension.Utility.Interfaces;
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
        private IMemoryCacheService _cache;

        [SetUp]
        public void SetUp()
        {
            _dataAccess = Substitute.For<ISubsidiaryPerformanceDataAccess>();
            _cache = Substitute.For<IMemoryCacheService>();
            _service = new SubsidiaryPerformanceService(_dataAccess, _cache);
            _serializer = new JavaScriptSerializer();
        }

        [TearDown]
        public void TearDown()
        {
            _cache = null;
            _dataAccess = null;
            _service = null;
        }

        [Test]
        public void GetSubsidiaryList_DataIsInCache_DoNotCallDataAccess()
        {
            // Arrange
            const string LicenseeName = "12Bet";
            const string CurrencyIdList = "1,2";
            var cacheKey = $"SubsidiaryList_{LicenseeName}";
            var subsidiaryList = new Dictionary<int, string>();

            _cache.Get<IDictionary<int, string>>(cacheKey).Returns(subsidiaryList);

            // Action
            _service.GetSubsidiaryList(LicenseeName, CurrencyIdList);

            // Assert
            _dataAccess.DidNotReceiveWithAnyArgs().GetSubsidiaryList(LicenseeName, CurrencyIdList);
        }

        [Test]
        public void GetSubsidiaryList_DataIsNotInCache_ReturnCorrectDataAfterCallingDataAccess()
        {
            // Arrange
            const string LicenseeName = "12Bet";
            const string CurrencyIdList = "1,2";
            var cacheKey = $"SubsidiaryList_{LicenseeName}";
            Dictionary<int, string> subsidiaryList = null;

            _cache.Get<IDictionary<int, string>>(cacheKey).Returns(subsidiaryList);

            var expectedResult = new Dictionary<int, string>
            {
                [1] = "Data1"
            };
            _dataAccess.GetSubsidiaryList(LicenseeName, CurrencyIdList).Returns(expectedResult);

            // Action
            var result = _service.GetSubsidiaryList(LicenseeName, CurrencyIdList);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetSubsidiaryList_DataIsNotInCache_SetMethodOfCacheShouldBeCalled()
        {
            // Arrange
            const string LicenseeName = "12Bet";
            const string CurrencyIdList = "1,2";
            var cacheKey = $"SubsidiaryList_{LicenseeName}";
            Dictionary<int, string> subsidiaryList = null;

            _cache.Get<IDictionary<int, string>>(cacheKey).Returns(subsidiaryList);

            var expectedResult = new Dictionary<int, string>
            {
                [1] = "Data1"
            };
            _dataAccess.GetSubsidiaryList(LicenseeName, CurrencyIdList).Returns(expectedResult);

            // Action
            _service.GetSubsidiaryList(LicenseeName, CurrencyIdList);

            // Assert
            _cache.Received().Set(cacheKey, expectedResult, 300);
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
        public void GetData_WhenCall_GetCorrectReturnedData()
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
            var expectedResult = _serializer.Serialize(GetExpectedDataModelList());

            _dataAccess.GetData(metaModel, out totalDataList).Returns(x =>
            {
                x[1] = new List<SubsidiaryPerformanceEntity>();
                return fakeEntityDataList;
            });

            // Action
            var result = _serializer.Serialize(_service.GetData(metaModel, out totalDataList));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetData_WhenCall_GetCorrectTotalDataList()
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
                x[1] = fakeEntityDataList;
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
                {
                    SubsidiaryId = 1234, Subsidiary = "New York", CurrencyId = 1, CurrencyName = "USD",
                    Product = "SB", BetCount = 50, Turnover = 100m, CustomerWinLoss = 10m, SubsidiaryWinLoss = -10m
                },
                new SubsidiaryPerformanceEntity
                {
                    SubsidiaryId = 1234, Subsidiary = "New York", CurrencyId = 1, CurrencyName = "USD",
                    Product = "BA", BetCount = 50, Turnover = 100m, CustomerWinLoss = 10m, SubsidiaryWinLoss = -10m
                },
                new SubsidiaryPerformanceEntity
                {
                    SubsidiaryId = 1234, Subsidiary = "New York", CurrencyId = 1, CurrencyName = "USD",
                    Product = "RB", BetCount = 50, Turnover = 100m, CustomerWinLoss = 10m, SubsidiaryWinLoss = -10m
                },
                new SubsidiaryPerformanceEntity
                {
                    SubsidiaryId = 1235, Subsidiary = "India", CurrencyId = 1, CurrencyName = "USD",
                    Product = "RB", BetCount = 50, Turnover = 100m, CustomerWinLoss = 100m, SubsidiaryWinLoss = -100m
                },
                new SubsidiaryPerformanceEntity
                {
                    SubsidiaryId = 1235, Subsidiary = "India", CurrencyId = 6, CurrencyName = "EUR",
                    Product = "SB", BetCount = 50, Turnover = 100m, CustomerWinLoss = 100m, SubsidiaryWinLoss = -100m
                },
                new SubsidiaryPerformanceEntity
                {
                    SubsidiaryId = 1235, Subsidiary = "India", CurrencyId = 1, CurrencyName = "USD",
                    Product = "SB", BetCount = 50, Turnover = 100m, CustomerWinLoss = 100m, SubsidiaryWinLoss = -100m
                }
            };
        }

        private static IEnumerable<SubsidiaryPerformanceDataModel> GetExpectedDataModelList()
        {
            var dataDetail1 = new List<SubsidiaryPerformanceDetail>
            {
                new SubsidiaryPerformanceDetail { Product = "All", BetCount = 150, Turnover = 300m, CustomerWinLoss = 30m, SubsidiaryWinLoss = -30m },
                new SubsidiaryPerformanceDetail { Product = "Sportsbook", BetCount = 50, Turnover = 100m, CustomerWinLoss = 10m, SubsidiaryWinLoss = -10m },
                new SubsidiaryPerformanceDetail { Product = "BA", BetCount = 50, Turnover = 100m, CustomerWinLoss = 10m, SubsidiaryWinLoss = -10m },
                new SubsidiaryPerformanceDetail { Product = "Racing", BetCount = 50, Turnover = 100m, CustomerWinLoss = 10m, SubsidiaryWinLoss = -10m }
            };

            var dataDetail2 = new List<SubsidiaryPerformanceDetail>
            {
                new SubsidiaryPerformanceDetail { Product = "All", BetCount = 100, Turnover = 200m, CustomerWinLoss = 200m, SubsidiaryWinLoss = -200m },
                new SubsidiaryPerformanceDetail { Product = "Racing", BetCount = 50, Turnover = 100m, CustomerWinLoss = 100m, SubsidiaryWinLoss = -100m },
                new SubsidiaryPerformanceDetail { Product = "Sportsbook", BetCount = 50, Turnover = 100m, CustomerWinLoss = 100m, SubsidiaryWinLoss = -100m }
            };

            var dataDetail3 = new List<SubsidiaryPerformanceDetail>
            {
                new SubsidiaryPerformanceDetail { Product = "All", BetCount = 50, Turnover = 100m, CustomerWinLoss = 100m, SubsidiaryWinLoss = -100m },
                new SubsidiaryPerformanceDetail { Product = "Sportsbook", BetCount = 50, Turnover = 100m, CustomerWinLoss = 100m, SubsidiaryWinLoss = -100m }
            };

            return new List<SubsidiaryPerformanceDataModel>
            {
                new SubsidiaryPerformanceDataModel { Subsidiary = "1234", CurrencyName = "USD", Details = dataDetail1 },
                new SubsidiaryPerformanceDataModel { Subsidiary = "1235", CurrencyName = "USD", Details = dataDetail2 },
                new SubsidiaryPerformanceDataModel { Subsidiary = "1235", CurrencyName = "EUR", Details = dataDetail3 }
            };
        }

        private static IEnumerable<SubsidiaryPerformanceEntity> GetExpectedTotalDataList()
        {
            return new List<SubsidiaryPerformanceEntity>
            {
                new SubsidiaryPerformanceEntity
                {
                    SubsidiaryId = 1234,
                    Subsidiary = "Total 1234",
                    CurrencyName = "Based EUR",
                    BetCount = 150,
                    Turnover = 300m,
                    CustomerWinLoss = 30m,
                    SubsidiaryWinLoss = -30m
                },
                new SubsidiaryPerformanceEntity
                {
                    SubsidiaryId = 1235,
                    Subsidiary = "Total 1235",
                    CurrencyName = "Based EUR",
                    BetCount = 150,
                    Turnover = 300m,
                    CustomerWinLoss = 300m,
                    SubsidiaryWinLoss = -300m
                }
            };
        }
    }
}