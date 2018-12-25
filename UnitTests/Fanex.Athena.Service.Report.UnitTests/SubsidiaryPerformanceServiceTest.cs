namespace Fanex.Athena.Service.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Constant.DataDomain;
    using Core.Users.Interfaces;
    using DataAccess.Report.Interfaces;
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

        private IList<Subsidiary> _subsidiaryList = new List<Subsidiary>
        {
            new Subsidiary { AgentId = 1234, CurrencyId = 1, OperatorId = "New York" },
            new Subsidiary { AgentId = 1235, CurrencyId = 2, OperatorId = "India" }
        };
        private readonly IDictionary<int, string> _currencyList = new Dictionary<int, string> { { 20, "UUS" } };

        private ISubsidiaryPerformanceDataAccess _dataAccess;
        private SubsidiaryPerformanceService _service;
        private ISessionService _sessionService;
        private ISubsidiaryService _subsidiaryService;

        [SetUp]
        public void SetUp()
        {
            _sessionService = Substitute.For<ISessionService>();
            _dataAccess = Substitute.For<ISubsidiaryPerformanceDataAccess>();
            _subsidiaryService = Substitute.For<ISubsidiaryService>();
            _service = new SubsidiaryPerformanceService(_sessionService, _subsidiaryService, _dataAccess);

            _sessionService.UserSession.Returns(new Session.Model.UserSession());
        }

        [TearDown]
        public void TearDown()
        {
            _dataAccess = null;
            _subsidiaryService = null;
            _sessionService = null;
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
            _dataAccess.GetData(metaModel, _currencyList).Returns(resultData);

            // Action
            _service.GetData(metaModel, _currencyList);

            // Assert
            _dataAccess.ReceivedWithAnyArgs().GetData(metaModel, _currencyList);
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
                SubsidiaryList = "New York,India",
                BaseCurrency = "EUR",
                LicenseeName = LicenseeName
            };

            _subsidiaryService.GetSubsidiaryList().Returns(_subsidiaryList);

            var fakeEntityDataList = GetFakeEntityDataList();

            var expectedResult = ObjectHelper.ToJson(GetExpectedDataModelList());

            _dataAccess.GetData(metaModel, _currencyList).Returns(fakeEntityDataList);

            // Action
            var result = ObjectHelper.ToJson(_service.GetData(metaModel, _currencyList).Data);

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
                BaseCurrency = "EUR",
                LicenseeName = LicenseeName
            };

            _sessionService.UserSession.Returns(new Session.Model.UserSession { UserLevel = UserLevelType.Subsidiary, OperatorId = "123" });
            _subsidiaryService.GetSubsidiaryList().Returns(_subsidiaryList);

            var resultData = GetFakeEntityDataList();

            var expectedData = new List<SubsidiaryPerformanceDataModel>();
            var expectedResult = ObjectHelper.ToJson(expectedData);

            _dataAccess.GetData(metaModel, _currencyList).Returns(resultData);

            // Action
            var result = ObjectHelper.ToJson(_service.GetData(metaModel, _currencyList).Data);

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
                SubsidiaryList = "New York,India",
                BaseCurrency = "EUR",
                LicenseeName = LicenseeName
            };

            _subsidiaryService.GetSubsidiaryList().Returns(_subsidiaryList);

            var resultData = GetFakeEntityDataList();

            var expectedResult = ObjectHelper.ToJson(GetExpectedTotalDataList());

            _dataAccess.GetData(metaModel, _currencyList).Returns(resultData);

            // Action
            var totalDataList = _service.GetData(metaModel, _currencyList).Total;
            var result = ObjectHelper.ToJson(totalDataList);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectGrandTotalData()
        {
            // Arrange
            var metaModel = new SubsidiaryPerformanceMetaModel
            {
                FromDate = new DateTime(2017, 8, 30),
                ToDate = new DateTime(2017, 8, 30),
                ProductCodeList = "SB,BA",
                SubsidiaryList = "New York,India",
                BaseCurrency = "EUR",
                LicenseeName = LicenseeName
            };

            _subsidiaryService.GetSubsidiaryList().Returns(_subsidiaryList);

            var resultData = GetFakeEntityDataList();

            var expectedResult = ObjectHelper.ToJson(GetExpectedGrandTotal());

            _dataAccess.GetData(metaModel, _currencyList).Returns(resultData);

            // Action
            var grandTotalDataList = _service.GetData(metaModel, _currencyList).GrandTotal;
            var result = ObjectHelper.ToJson(grandTotalDataList);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetDetailData_WhenCall_CallDataAccess()
        {
            // Arrange
            var metaModel = new SubsidiaryPerformanceMetaModel();

            _dataAccess.GetDetailData(metaModel, _currencyList).Returns(new Model.Core.ResultModel.ReportResultModel<SubsidiaryPerformanceDetail>());

            // Action
            _service.GetDetailData(metaModel, _currencyList);

            // Assert
            _dataAccess.Received().GetDetailData(metaModel, _currencyList);
        }

        private static IEnumerable<SubsidiaryPerformance> GetFakeEntityDataList()
        {
            return new List<SubsidiaryPerformance>
            {
                new SubsidiaryPerformance
                {
                    SubsidiaryId = 1234, CurrencyId = 1, CurrencyName = "USD", ProductCode = "SB",
                    BetCount = 50, Turnover = 100m, CustomerWinLoss = 10m, SubsidiaryWinLoss = -10m,
                    BaseCurrencyData = new SubsidiaryPerformanceBase { BetCount = 50, Turnover = 101m, CustomerWinLoss = 11m, SubsidiaryWinLoss = -11m }
                },
                new SubsidiaryPerformance
                {
                    SubsidiaryId = 1234, CurrencyId = 1, CurrencyName = "USD", ProductCode = "BA",
                    BetCount = 50, Turnover = 100m, CustomerWinLoss = 10m, SubsidiaryWinLoss = -10m,
                    BaseCurrencyData = new SubsidiaryPerformanceBase { BetCount = 50, Turnover = 101m, CustomerWinLoss = 11m, SubsidiaryWinLoss = -11m }
                },
                new SubsidiaryPerformance
                {
                    SubsidiaryId = 1234, CurrencyId = 1, CurrencyName = "USD", ProductCode = "PP",
                    BetCount = 50, Turnover = 100m, CustomerWinLoss = 10m, SubsidiaryWinLoss = -10m,
                    BaseCurrencyData = new SubsidiaryPerformanceBase { BetCount = 50, Turnover = 101m, CustomerWinLoss = 11m, SubsidiaryWinLoss = -11m }
                },
                new SubsidiaryPerformance
                {
                    SubsidiaryId = 1235, CurrencyId = 1, CurrencyName = "USD", ProductCode = "PP",
                    BetCount = 50, Turnover = 100m, CustomerWinLoss = 100m, SubsidiaryWinLoss = -100m,
                    BaseCurrencyData = new SubsidiaryPerformanceBase { BetCount = 50, Turnover = 101m, CustomerWinLoss = 101m, SubsidiaryWinLoss = -101m }
                },
                new SubsidiaryPerformance
                {
                    SubsidiaryId = 1235, CurrencyId = 6, CurrencyName = "EUR", ProductCode = "SB",
                    BetCount = 50, Turnover = 100m, CustomerWinLoss = 100m, SubsidiaryWinLoss = -100m,
                    BaseCurrencyData = new SubsidiaryPerformanceBase { BetCount = 50, Turnover = 101m, CustomerWinLoss = 101m, SubsidiaryWinLoss = -101m }
                },
                new SubsidiaryPerformance
                {
                    SubsidiaryId = 1235, CurrencyId = 1, CurrencyName = "USD", ProductCode = "SB",
                    BetCount = 50, Turnover = 100m, CustomerWinLoss = 100m, SubsidiaryWinLoss = -100m,
                    BaseCurrencyData = new SubsidiaryPerformanceBase { BetCount = 50, Turnover = 101m, CustomerWinLoss = 101m, SubsidiaryWinLoss = -101m }
                },
                new SubsidiaryPerformance
                {
                    SubsidiaryId = 1235, CurrencyId = 1, CurrencyName = "USD", ProductCode = "SB",
                    BetCount = 50, Turnover = 100m, CustomerWinLoss = 100m, SubsidiaryWinLoss = -100m,
                    BaseCurrencyData = new SubsidiaryPerformanceBase { BetCount = 50, Turnover = 101m, CustomerWinLoss = 101m, SubsidiaryWinLoss = -101m }
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
                new SubsidiaryPerformanceBase { Product = "PP", BetCount = 50, Turnover = 100m, CustomerWinLoss = 10m, SubsidiaryWinLoss = -10m }
            };

            var dataDetail2 = new List<SubsidiaryPerformanceBase>
            {
                new SubsidiaryPerformanceBase { Product = "All", BetCount = 150, Turnover = 300m, CustomerWinLoss = 300m, SubsidiaryWinLoss = -300m },
                new SubsidiaryPerformanceBase { Product = "PP", BetCount = 50, Turnover = 100m, CustomerWinLoss = 100m, SubsidiaryWinLoss = -100m },
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
                    Subsidiary = "New York", TotalItem = 4, Details = new Dictionary<string, IEnumerable<SubsidiaryPerformanceBase>> { { "USD", dataDetail1 } }
                },
                new SubsidiaryPerformanceDataModel
                {
                    Subsidiary = "India", TotalItem = 5, Details = new Dictionary<string, IEnumerable<SubsidiaryPerformanceBase>> { { "USD", dataDetail2 }, { "EUR", dataDetail3 } }
                }
            };
        }

        private static IDictionary<string, SubsidiaryPerformanceBase> GetExpectedTotalDataList()
        {
            return new Dictionary<string, SubsidiaryPerformanceBase>
            {
                {
                    "New York",
                    new SubsidiaryPerformanceBase
                    {
                        CurrencyName = "EUR",
                        BetCount = 150,
                        Turnover = 303m,
                        CustomerWinLoss = 33m,
                        SubsidiaryWinLoss = -33m
                    }
                },
                {
                    "India",
                    new SubsidiaryPerformanceBase
                    {
                        CurrencyName = "EUR",
                        BetCount = 200,
                        Turnover = 404m,
                        CustomerWinLoss = 404m,
                        SubsidiaryWinLoss = -404m
                    }
                }
            };
        }

        private static SubsidiaryPerformanceBase GetExpectedGrandTotal()
        {
            return new SubsidiaryPerformanceBase
            {
                CurrencyName = "EUR",
                BetCount = 350,
                Turnover = 707m,
                CustomerWinLoss = 437m,
                SubsidiaryWinLoss = -437
            };
        }
    }
}