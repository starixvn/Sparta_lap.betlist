namespace Fanex.Athena.Service.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Web.Script.Serialization;
    using DataAccess.Report.Interfaces;
    using Extension.Utility.Interfaces;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.DTO.SubsidiaryPerformance;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SubsidiaryPerformanceServiceTest
    {
        private const string LicenseeName = "12Bet";
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
        public void GetOperatorList_DataIsInCache_DoNotCallDataAccess()
        {
            // Arrange
            var cacheKey = $"SubsidiaryList_{LicenseeName}";
            var subsidiaryList = new Dictionary<int, string>();

            _cache.Get<IDictionary<int, string>>(cacheKey).Returns(subsidiaryList);

            // Action
            _service.GetOperatorList(LicenseeName);

            // Assert
            _dataAccess.DidNotReceiveWithAnyArgs().GetOperatorList(LicenseeName);
        }

        [Test]
        public void GetOperatorList_DataIsNotInCache_ReturnCorrectDataAfterCallingDataAccess()
        {
            // Arrange
            var cacheKey = $"MemoryCache_SubsidiaryList_{LicenseeName}";

            IEnumerable<Operator> operatorList = null;
            _cache.Get<IEnumerable<Operator>>(cacheKey).Returns(operatorList);

            const string OperatorName = "Data1";
            const int AgentId = 1;
            var expectedFromDataAccess = new List<Operator>
            {
                new Operator
                {
                    AgentId = AgentId,
                    CurrencyId = 1,
                    OperatorName = OperatorName
                }
            };
            _dataAccess.GetOperatorList(LicenseeName).Returns(expectedFromDataAccess);

            // Action
            var result = _service.GetOperatorList(LicenseeName);

            // Assert
            Assert.That(result, Is.EqualTo(expectedFromDataAccess));
        }

        [Test]
        public void GetSubsidiaryList_DataIsNotInCache_SetMethodOfCacheShouldBeCalled()
        {
            // Arrange
            var cacheKey = $"MemoryCache_SubsidiaryList_{LicenseeName}";
            IEnumerable<Operator> operatorList = null;

            _cache.Get<IEnumerable<Operator>>(cacheKey).Returns(operatorList);

            const string OperatorName = "Data1";
            const int AgentId = 1;
            var expectedFromDataAccess = new List<Operator>
            {
                new Operator
                {
                    AgentId = AgentId,
                    CurrencyId = 1,
                    OperatorName = OperatorName
                }
            };
            _dataAccess.GetOperatorList(LicenseeName).Returns(expectedFromDataAccess);

            // Action
            var result = _service.GetOperatorList(LicenseeName);

            // Assert
            _cache.Received().Set(cacheKey, expectedFromDataAccess, 300);
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
            var resultData = new ReportResultModel<SubsidiaryPerformanceEntity>
            {
                DataList = new List<SubsidiaryPerformanceEntity>(),
                TotalDataList = new List<SubsidiaryPerformanceEntity>()
            };
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

            var operatorList = new List<Operator>
            {
                new Operator { AgentId = 1234, CurrencyId = 1, OperatorName = "New York" },
                new Operator { AgentId = 1235, CurrencyId = 2, OperatorName = "India" }
            };
            var cacheKey = $"MemoryCache_SubsidiaryList_{LicenseeName}";
            _cache.Get<IEnumerable<Operator>>(cacheKey).Returns(operatorList);

            var fakeEntityDataList = GetFakeEntityDataList();
            var resultData = new ReportResultModel<SubsidiaryPerformanceEntity>
            {
                DataList = fakeEntityDataList,
                TotalDataList = new List<SubsidiaryPerformanceEntity>()
            };
            var expectedResult = _serializer.Serialize(GetExpectedDataModelList());

            _dataAccess.GetData(metaModel).Returns(resultData);

            // Action
            var result = _serializer.Serialize(_service.GetData(metaModel).Data);

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

            var operatorList = new List<Operator>
            {
                new Operator { AgentId = 1230, CurrencyId = 1, OperatorName = "New York" }
            };
            var cacheKey = $"MemoryCache_SubsidiaryList_{LicenseeName}";
            _cache.Get<IEnumerable<Operator>>(cacheKey).Returns(operatorList);

            var resultData = new ReportResultModel<SubsidiaryPerformanceEntity>
            {
                DataList = GetFakeEntityDataList(),
                TotalDataList = new List<SubsidiaryPerformanceEntity>()
            };

            var expectedData = new List<SubsidiaryPerformanceDataModel>();
            var expectedResult = _serializer.Serialize(expectedData);

            _dataAccess.GetData(metaModel).Returns(resultData);

            // Action
            var result = _serializer.Serialize(_service.GetData(metaModel).Data);

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

            var operatorList = new List<Operator>
            {
                new Operator { AgentId = 1234, CurrencyId = 1, OperatorName = "New York" },
                new Operator { AgentId = 1235, CurrencyId = 2, OperatorName = "India" }
            };
            var cacheKey = $"MemoryCache_SubsidiaryList_{LicenseeName}";
            _cache.Get<IEnumerable<Operator>>(cacheKey).Returns(operatorList);

            var resultData = new ReportResultModel<SubsidiaryPerformanceEntity>
            {
                DataList = GetFakeEntityDataList(),
                TotalDataList = GetFakeEntityDataList()
            };
            var expectedResult = _serializer.Serialize(GetExpectedTotalDataList());

            _dataAccess.GetData(metaModel).Returns(resultData);

            // Action
            var totalDataList = _service.GetData(metaModel).Total;
            var result = _serializer.Serialize(totalDataList);

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

            var operatorList = new List<Operator>
            {
                new Operator { AgentId = 1234, CurrencyId = 1, OperatorName = "New York" },
                new Operator { AgentId = 1235, CurrencyId = 2, OperatorName = "India" }
            };
            var cacheKey = $"MemoryCache_SubsidiaryList_{LicenseeName}";
            _cache.Get<IEnumerable<Operator>>(cacheKey).Returns(operatorList);

            var resultData = new ReportResultModel<SubsidiaryPerformanceEntity>
            {
                DataList = GetFakeEntityDataList(),
                TotalDataList = GetFakeEntityDataList()
            };
            var expectedResult = _serializer.Serialize(GetExpectedGrandTotal());

            _dataAccess.GetData(metaModel).Returns(resultData);

            // Action
            var grandTotalDataList = _service.GetData(metaModel).GrandTotal;
            var result = _serializer.Serialize(grandTotalDataList);

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

            var operatorList = new List<Operator>
            {
                new Operator { AgentId = 1234, CurrencyId = 1, OperatorName = "New York" },
                new Operator { AgentId = 1235, CurrencyId = 2, OperatorName = "India" }
            };
            var cacheKey = $"MemoryCache_SubsidiaryList_{LicenseeName}";
            _cache.Get<IEnumerable<Operator>>(cacheKey).Returns(operatorList);

            var resultData = new ReportResultModel<SubsidiaryPerformanceEntity>
            {
                DataList = new List<SubsidiaryPerformanceEntity>(),
                TotalDataList = new List<SubsidiaryPerformanceEntity>()
            };

            _dataAccess.GetData(metaModel).Returns(resultData);

            // Action
            var result = _service.GetData(metaModel).GrandTotal;

            // Assert
            Assert.That(result, Is.Null);
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
            var dataDetail1 = new List<SubsidiaryPerformanceModelBase>
            {
                new SubsidiaryPerformanceModelBase { Product = "All", BetCount = 150, Turnover = 300m, CustomerWinLoss = 30m, SubsidiaryWinLoss = -30m },
                new SubsidiaryPerformanceModelBase { Product = "Sportsbook", BetCount = 50, Turnover = 100m, CustomerWinLoss = 10m, SubsidiaryWinLoss = -10m },
                new SubsidiaryPerformanceModelBase { Product = "BA", BetCount = 50, Turnover = 100m, CustomerWinLoss = 10m, SubsidiaryWinLoss = -10m },
                new SubsidiaryPerformanceModelBase { Product = "Racing", BetCount = 50, Turnover = 100m, CustomerWinLoss = 10m, SubsidiaryWinLoss = -10m }
            };

            var dataDetail2 = new List<SubsidiaryPerformanceModelBase>
            {
                new SubsidiaryPerformanceModelBase { Product = "All", BetCount = 150, Turnover = 300m, CustomerWinLoss = 300m, SubsidiaryWinLoss = -300m },
                new SubsidiaryPerformanceModelBase { Product = "Racing", BetCount = 50, Turnover = 100m, CustomerWinLoss = 100m, SubsidiaryWinLoss = -100m },
                new SubsidiaryPerformanceModelBase { Product = "Sportsbook", BetCount = 100, Turnover = 200m, CustomerWinLoss = 200m, SubsidiaryWinLoss = -200m }
            };

            var dataDetail3 = new List<SubsidiaryPerformanceModelBase>
            {
                new SubsidiaryPerformanceModelBase { Product = "All", BetCount = 50, Turnover = 100m, CustomerWinLoss = 100m, SubsidiaryWinLoss = -100m },
                new SubsidiaryPerformanceModelBase { Product = "Sportsbook", BetCount = 50, Turnover = 100m, CustomerWinLoss = 100m, SubsidiaryWinLoss = -100m }
            };

            return new List<SubsidiaryPerformanceDataModel>
            {
                new SubsidiaryPerformanceDataModel { Subsidiary = "New York", CurrencyName = "USD", Details = dataDetail1 },
                new SubsidiaryPerformanceDataModel { Subsidiary = "India", CurrencyName = "USD", Details = dataDetail2 },
                new SubsidiaryPerformanceDataModel { Subsidiary = "India", CurrencyName = "EUR", Details = dataDetail3 }
            };
        }

        private static IEnumerable<SubsidiaryPerformanceEntity> GetExpectedTotalDataList()
        {
            return new List<SubsidiaryPerformanceEntity>
            {
                new SubsidiaryPerformanceEntity
                {
                    Subsidiary = "Total New York",
                    CurrencyName = "Based EUR",
                    BetCount = 150,
                    Turnover = 300m,
                    CustomerWinLoss = 30m,
                    SubsidiaryWinLoss = -30m
                },
                new SubsidiaryPerformanceEntity
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

        private static SubsidiaryPerformanceEntity GetExpectedGrandTotal()
        {
            return new SubsidiaryPerformanceEntity
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