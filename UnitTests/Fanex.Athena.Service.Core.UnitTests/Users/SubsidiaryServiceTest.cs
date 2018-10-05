namespace Fanex.Athena.Service.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Users.Interfaces;
    using DataAccess.Core.Users.Interfaces;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.Model.Report.DTO;
    using Interfaces;
    using NSubstitute;
    using NUnit.Framework;
    using Session.Model;

    [TestFixture]
    public class SubsidiaryServiceTest
    {
        private const string LicenseeName = "12Bet";
        private ISessionService _sessionService;
        private ISubsidiaryDataAccess _dataAccess;
        private IMemoryCacheService _cache;
        private ISubsidiaryService _service;

        [SetUp]
        public void SetUp()
        {
            _dataAccess = Substitute.For<ISubsidiaryDataAccess>();
            _cache = Substitute.For<IMemoryCacheService>();
            _sessionService = Substitute.For<ISessionService>();
            _sessionService.LicenseeSession.Returns(new LicenseeSession
            {
                LicenseeName = LicenseeName
            });
            _sessionService.UserSession.Returns(new UserSession
            {
                CurrencyList = new Dictionary<int, string>() { { 20, "UUS" }, { 51, "IDR" }, { 13, "CNY" } },
                CurrencyAgentList = GetAgentByCurrencyData()
            });

            _dataAccess.GetSubsidiaryList(Arg.Any<string>()).Returns(GetSubsidiaryData());
            _cache.Get(Arg.Any<string>(), Arg.Any<Func<IEnumerable<Subsidiary>>>(), Arg.Any<int>())
                .Returns(GetSubsidiaryData());

            _service = new SubsidiaryService(_sessionService, _dataAccess, _cache);
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

            // Action
            _service.GetSubsidiaryList();

            // Assert
            _dataAccess.DidNotReceiveWithAnyArgs().GetSubsidiaryList(LicenseeName);
        }

        [Test]
        public void GetOperatorList_FilterOperatorByUserAgents_ReturnCorrectlyOperatorList()
        {
            // Arrange
            var cacheKey = $"SubsidiaryList_{LicenseeName}";
            var expectedResult = GetAgentByCurrencyData().SelectMany(i => i.Value);

            // Action
            var result = _service.GetSubsidiaryList().Select(i => i.AgentId);

            // Assert
            Assert.AreEqual(true, result.Except(expectedResult).Count() == 0);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" , ")]
        public void GetCurrencyAgentList_NullOrEmptySubsidairyName_ReturnEmptyList(string subsidairyNames)
        {
            // Action
            var result = _service.GetCurrencyAgentList(subsidairyNames, false, 0);

            // Assert
            Assert.AreEqual(false, result.Any());
        }

        [TestCase("alex,qc", true, 0)]
        [TestCase("alex,qc", false, 0)]
        [TestCase("qc", true, 20)]
        [TestCase("alex", false, 51)]
        public void GetCurrencyAgentList_FilterTestData_ReturnCorrectlyAgentList(
            string subsidiaryNames, bool isExcludeTest, int currencyId = 0)
        {
            // Action
            var result = _service.GetCurrencyAgentList(subsidiaryNames, isExcludeTest, currencyId);

            // Assert
            Assert.AreEqual(isExcludeTest, isExcludeTest && !result.Keys.Contains(20));
        }

        [TestCase("alex,qc", 0)]
        [TestCase("qc", 0)]
        [TestCase("alex", 20)]
        [TestCase("alex,qc", 51)]
        public void GetCurrencyAgentList_FilterAgentBySubsidiaryName_ReturnCorrectlyAgentList(
          string subsidiaryNames, int currencyId = 0)
        {
            // Arrange
            List<int> expectedResult = GetAgentListBySubsidairyNames(subsidiaryNames, currencyId);

            // Action
            var result = _service.GetCurrencyAgentList(subsidiaryNames, false, currencyId)
            .SelectMany(i => i.Value);

            // Assert
            Assert.AreEqual(expectedResult.Count(), result.Count());

            Assert.AreEqual(0, result.Except(expectedResult).Count());
        }

        [TestCase("(others)", 0)]
        [TestCase("(others),alex,qc", 0)]
        [TestCase("(others)", 20)]
        [TestCase("(others),qc", 51)]
        public void GetCurrencyAgentList_FilterAgentByOtherOption_ReturnCorrectlyAgentList(
         string subsidiaryNames, int currencyId = 0)
        {
            // Arrange
            var expectedResult = GetAgentByCurrencyData().Where(i => i.Key == currencyId || currencyId == 0)
                .SelectMany(i => i.Value).ToList()
                .Except(GetSubsidiaryData().Select(i => i.AgentId))
                .Union(GetAgentListBySubsidairyNames(subsidiaryNames, currencyId));

            // Action
            var result = _service.GetCurrencyAgentList(subsidiaryNames, false, currencyId)
            .SelectMany(i => i.Value);

            // Assert
            Assert.AreEqual(expectedResult.Count(), result.Count());

            Assert.AreEqual(0, result.Except(expectedResult).Count());
        }

        private static List<int> GetAgentListBySubsidairyNames(string subsidiaryNames, int currencyId)
        {
            var subList = subsidiaryNames.Split(new char[] { ',' });
            var agentBySub = GetSubsidiaryData().Where(i =>
              subList.Contains(i.OperatorId) && (currencyId == 0 || currencyId == i.CurrencyId)).Select(i => i.AgentId);

            return agentBySub.Intersect(GetAgentByCurrencyData().SelectMany(i => i.Value)).ToList();
        }

        private static IDictionary<int, int[]> GetAgentByCurrencyData()
        {
            return new Dictionary<int, int[]>
                {
                    { 20, new[] { 201, 202, 203, 204, 205 } },
                    { 51, new[] { 511, 512, 513, 514, 515 } },
                    { 13, new[] { 131, 132 } }
                };
        }

        private static IEnumerable<Subsidiary> GetSubsidiaryData()
        {
            yield return new Subsidiary
            {
                AgentId = 201,
                CurrencyId = 20,
                OperatorId = "alex"
            };
            yield return new Subsidiary
            {
                AgentId = 203,
                CurrencyId = 20,
                OperatorId = "alex"
            };
            yield return new Subsidiary
            {
                AgentId = 513,
                CurrencyId = 51,
                OperatorId = "qc"
            };
            yield return new Subsidiary
            {
                AgentId = 512,
                CurrencyId = 51,
                OperatorId = "qc"
            };
            yield return new Subsidiary
            {
                AgentId = 202,
                CurrencyId = 20,
                OperatorId = "qc"
            };
            yield return new Subsidiary
            {
                AgentId = 516,
                CurrencyId = 51,
                OperatorId = "qc"
            };
            yield return new Subsidiary
            {
                AgentId = 601,
                CurrencyId = 60,
                OperatorId = "alex"
            };
        }
    }
}