namespace Fanex.Athena.Service.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Web.Script.Serialization;
    using DataAccess.Report.Interfaces;
    using Extension.Utility.Interfaces;
    using Interfaces;
    using Model.Report.DTO;
    using Model.Report.DTO.SubsidiaryPerformance;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class SubsidiaryPerformanceServiceTest
    {
        private const string LicenseeName = "12Bet";
        private ISubsidiaryPerformanceDataAccess _dataAccess;
        private SubsidiaryPerformanceService _service;
        private IMemoryCacheService _cache;
        private ISubsidiaryService _subsidiaryService;

        [SetUp]
        public void SetUp()
        {
            _dataAccess = Substitute.For<ISubsidiaryPerformanceDataAccess>();
            _cache = Substitute.For<IMemoryCacheService>();
            _subsidiaryService = Substitute.For<ISubsidiaryService>();
            _service = new SubsidiaryPerformanceService(_subsidiaryService, _dataAccess, _cache);
        }

        [TearDown]
        public void TearDown()
        {
            _cache = null;
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
            var resultData = new List<SubsidiaryPerformance>();
            _dataAccess.GetData(metaModel).Returns(resultData);

            // Action
            _service.GetData(metaModel);

            // Assert
            _dataAccess.ReceivedWithAnyArgs().GetData(metaModel);
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
                BaseCurrency = "EUR",
                LicenseeName = LicenseeName
            };

            var operatorList = new List<Subsidiary>
            {
                new Subsidiary { AgentId = 1234, CurrencyId = 1, SubsidiaryName = "New York" },
                new Subsidiary { AgentId = 1235, CurrencyId = 2, SubsidiaryName = "India" }
            };
            var cacheKey = $"MemoryCache_SubsidiaryList_{LicenseeName}";
            _cache.Get(cacheKey, Arg.Any<Func<IEnumerable<Subsidiary>>>(), Arg.Any<int>()).Returns(operatorList);

            var fakeEntityDataList = GetFakeEntityDataList();

            var expectedResult = ObjectHelper.ToJson(GetExpectedDataModelList());

            _dataAccess.GetData(metaModel).Returns(fakeEntityDataList);

            // Action
            var result = ObjectHelper.ToJson(_service.GetData(metaModel).Data);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetData_AgentIdIsNotExistInOpertorList_ReturnEmptyResult()
        {
            // Arrange
            var metaModel = new SubsidiaryPerformanceMetaModel
            {
                FromDate = new DateTime(2017, 8, 30),
                ToDate = new DateTime(2017, 8, 30),
                ProductCodeList = "SB,BA",
                SubsidiaryList = "123,143",
                BaseCurrency = "EUR",
                LicenseeName = LicenseeName
            };

            var operatorList = new List<Subsidiary>
            {
                new Subsidiary { AgentId = 1230, CurrencyId = 1, SubsidiaryName = "New York" }
            };
            var cacheKey = $"MemoryCache_SubsidiaryList_{LicenseeName}";
            _cache.Get<IEnumerable<Subsidiary>>(cacheKey).Returns(operatorList);

            var resultData = GetFakeEntityDataList();

            var expectedData = new List<SubsidiaryPerformanceDataModel>();
            var expectedResult = ObjectHelper.ToJson(expectedData);

            _dataAccess.GetData(metaModel).Returns(resultData);

            // Action
            var result = ObjectHelper.ToJson(_service.GetData(metaModel).Data);

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
                BaseCurrency = "EUR",
                LicenseeName = LicenseeName
            };

            var operatorList = new List<Subsidiary>
            {
                new Subsidiary { AgentId = 1234, CurrencyId = 1, SubsidiaryName = "New York" },
                new Subsidiary { AgentId = 1235, CurrencyId = 2, SubsidiaryName = "India" }
            };
            var cacheKey = $"MemoryCache_SubsidiaryList_{LicenseeName}";
            _cache.Get(cacheKey, Arg.Any<Func<IEnumerable<Subsidiary>>>(), Arg.Any<int>()).Returns(operatorList);

            var resultData = GetFakeEntityDataList();

            var expectedResult = ObjectHelper.ToJson(GetExpectedTotalDataList());

            _dataAccess.GetData(metaModel).Returns(resultData);

            // Action
            var totalDataList = _service.GetData(metaModel).Total;
            var result = ObjectHelper.ToJson(totalDataList);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetData_TotalDataListIsNotEmpty_ReturnCorrectGrandTotalData()
        {
            // Arrange
            var metaModel = new SubsidiaryPerformanceMetaModel
            {
                FromDate = new DateTime(2017, 8, 30),
                ToDate = new DateTime(2017, 8, 30),
                ProductCodeList = "SB,BA",
                SubsidiaryList = "123,143",
                BaseCurrency = "EUR",
                LicenseeName = LicenseeName
            };

            var operatorList = new List<Subsidiary>
            {
                new Subsidiary { AgentId = 1234, CurrencyId = 1, SubsidiaryName = "New York" },
                new Subsidiary { AgentId = 1235, CurrencyId = 2, SubsidiaryName = "India" }
            };
            var cacheKey = $"MemoryCache_SubsidiaryList_{LicenseeName}";
            _cache.Get(cacheKey, Arg.Any<Func<IEnumerable<Subsidiary>>>(), Arg.Any<int>()).Returns(operatorList);

            var resultData = GetFakeEntityDataList();

            var expectedResult = ObjectHelper.ToJson(GetExpectedGrandTotal());

            _dataAccess.GetData(metaModel).Returns(resultData);

            // Action
            var grandTotalDataList = _service.GetData(metaModel).GrandTotal;
            var result = ObjectHelper.ToJson(grandTotalDataList);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetData_TotalDataListIsEmpty_GrandTotalIsNull()
        {
            // Arrange
            var metaModel = new SubsidiaryPerformanceMetaModel
            {
                FromDate = new DateTime(2017, 8, 30),
                ToDate = new DateTime(2017, 8, 30),
                ProductCodeList = "SB,BA",
                SubsidiaryList = "123,143",
                BaseCurrency = "EUR",
                LicenseeName = LicenseeName
            };

            var operatorList = new List<Subsidiary>
            {
                new Subsidiary { AgentId = 1234, CurrencyId = 1, SubsidiaryName = "New York" },
                new Subsidiary { AgentId = 1235, CurrencyId = 2, SubsidiaryName = "India" }
            };
            var cacheKey = $"MemoryCache_SubsidiaryList_{LicenseeName}";
            _cache.Get(cacheKey, Arg.Any<Func<IEnumerable<Subsidiary>>>()).Returns(operatorList);

            var resultData = new List<SubsidiaryPerformance>();

            _dataAccess.GetData(metaModel).Returns(resultData);

            // Action
            var result = _service.GetData(metaModel).GrandTotal;

            // Assert
            Assert.That(result, Is.Null);
        }

        private static IEnumerable<SubsidiaryPerformance> GetFakeEntityDataList()
        {
            return new List<SubsidiaryPerformance>
            {
                new SubsidiaryPerformance
                {
                    SubsidiaryId = 1234, Subsidiary = "New York", CurrencyId = 1, CurrencyName = "USD",
                    Product = "SB", BetCount = 50, Turnover = 100m, CustomerWinLoss = 10m, SubsidiaryWinLoss = -10m
                },
                new SubsidiaryPerformance
                {
                    SubsidiaryId = 1234, Subsidiary = "New York", CurrencyId = 1, CurrencyName = "USD",
                    Product = "BA", BetCount = 50, Turnover = 100m, CustomerWinLoss = 10m, SubsidiaryWinLoss = -10m
                },
                new SubsidiaryPerformance
                {
                    SubsidiaryId = 1234, Subsidiary = "New York", CurrencyId = 1, CurrencyName = "USD",
                    Product = "RB", BetCount = 50, Turnover = 100m, CustomerWinLoss = 10m, SubsidiaryWinLoss = -10m
                },
                new SubsidiaryPerformance
                {
                    SubsidiaryId = 1235, Subsidiary = "India", CurrencyId = 1, CurrencyName = "USD",
                    Product = "RB", BetCount = 50, Turnover = 100m, CustomerWinLoss = 100m, SubsidiaryWinLoss = -100m
                },
                new SubsidiaryPerformance
                {
                    SubsidiaryId = 1235, Subsidiary = "India", CurrencyId = 6, CurrencyName = "EUR",
                    Product = "SB", BetCount = 50, Turnover = 100m, CustomerWinLoss = 100m, SubsidiaryWinLoss = -100m
                },
                new SubsidiaryPerformance
                {
                    SubsidiaryId = 1235, Subsidiary = "India", CurrencyId = 1, CurrencyName = "USD",
                    Product = "SB", BetCount = 50, Turnover = 100m, CustomerWinLoss = 100m, SubsidiaryWinLoss = -100m
                },
                new SubsidiaryPerformance
                {
                    SubsidiaryId = 1235, Subsidiary = "India", CurrencyId = 1, CurrencyName = "USD",
                    Product = "SB", BetCount = 50, Turnover = 100m, CustomerWinLoss = 100m, SubsidiaryWinLoss = -100m
                }
            };
        }

        private static IEnumerable<SubsidiaryPerformanceDataModel> GetExpectedDataModelList()
        {
            var dataDetail1 = new List<SubsidiaryPerformanceBase>
            {
                new SubsidiaryPerformanceBase { Product = "All", BetCount = 150, Turnover = 300m, CustomerWinLoss = 30m, SubsidiaryWinLoss = -30m },
                new SubsidiaryPerformanceBase { Product = "Sportsbook", BetCount = 50, Turnover = 100m, CustomerWinLoss = 10m, SubsidiaryWinLoss = -10m },
                new SubsidiaryPerformanceBase { Product = "BA", BetCount = 50, Turnover = 100m, CustomerWinLoss = 10m, SubsidiaryWinLoss = -10m },
                new SubsidiaryPerformanceBase { Product = "Racing", BetCount = 50, Turnover = 100m, CustomerWinLoss = 10m, SubsidiaryWinLoss = -10m }
            };

            var dataDetail2 = new List<SubsidiaryPerformanceBase>
            {
                new SubsidiaryPerformanceBase { Product = "All", BetCount = 150, Turnover = 300m, CustomerWinLoss = 300m, SubsidiaryWinLoss = -300m },
                new SubsidiaryPerformanceBase { Product = "Racing", BetCount = 50, Turnover = 100m, CustomerWinLoss = 100m, SubsidiaryWinLoss = -100m },
                new SubsidiaryPerformanceBase { Product = "Sportsbook", BetCount = 100, Turnover = 200m, CustomerWinLoss = 200m, SubsidiaryWinLoss = -200m }
            };

            var dataDetail3 = new List<SubsidiaryPerformanceBase>
            {
                new SubsidiaryPerformanceBase { Product = "All", BetCount = 50, Turnover = 100m, CustomerWinLoss = 100m, SubsidiaryWinLoss = -100m },
                new SubsidiaryPerformanceBase { Product = "Sportsbook", BetCount = 50, Turnover = 100m, CustomerWinLoss = 100m, SubsidiaryWinLoss = -100m }
            };

            return new List<SubsidiaryPerformanceDataModel>
            {
                new SubsidiaryPerformanceDataModel
                {
                    Subsidiary = "New York", Details = new Dictionary<string, IEnumerable<SubsidiaryPerformanceBase>> { { "USD", dataDetail1 } }
                },
                new SubsidiaryPerformanceDataModel
                {
                    Subsidiary = "India", Details = new Dictionary<string, IEnumerable<SubsidiaryPerformanceBase>> { { "USD", dataDetail2 } }
                },
                new SubsidiaryPerformanceDataModel
                {
                    Subsidiary = "India", Details = new Dictionary<string, IEnumerable<SubsidiaryPerformanceBase>> { { "EUR", dataDetail3 } }
                }
            };
        }

        private static IEnumerable<SubsidiaryPerformance> GetExpectedTotalDataList()
        {
            return new List<SubsidiaryPerformance>
            {
                new SubsidiaryPerformance
                {
                    Subsidiary = "Total New York",
                    CurrencyName = "Based EUR",
                    BetCount = 150,
                    Turnover = 300m,
                    CustomerWinLoss = 30m,
                    SubsidiaryWinLoss = -30m
                },
                new SubsidiaryPerformance
                {
                    Subsidiary = "Total India",
                    CurrencyName = "Based EUR",
                    BetCount = 200,
                    Turnover = 400m,
                    CustomerWinLoss = 400m,
                    SubsidiaryWinLoss = -400m
                }
            };
        }

        private static SubsidiaryPerformance GetExpectedGrandTotal()
        {
            return new SubsidiaryPerformance
            {
                Subsidiary = "Total",
                CurrencyName = "Based EUR",
                BetCount = 350,
                Turnover = 700m,
                CustomerWinLoss = 430m,
                SubsidiaryWinLoss = -430
            };
        }
    }
}