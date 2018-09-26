namespace Fanex.Athena.Service.Report.UnitTests
{
    using System.Collections.Generic;
    using Core.Users.Interfaces;
    using DataAccess.Core.Users.Interfaces;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.Model.Report.DTO;
    using Interfaces;
    using NSubstitute;
    using NUnit.Framework;
    using Session.Model;
    using System.Linq;
    using System;

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
            _sessionService = Substitute.For<ISessionService>();
            _sessionService.LicenseeSession.Returns(new LicenseeSession
            {
                LicenseeName = LicenseeName
            });
            _sessionService.UserSession.Returns(new UserSession
            {
                CurrencyList = new Dictionary<int, string>()
            });

            _dataAccess = Substitute.For<ISubsidiaryDataAccess>();
            _cache = Substitute.For<IMemoryCacheService>();
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
            var subsidiaryList = new Dictionary<int, string>();

            _cache.Get<IDictionary<int, string>>(cacheKey).Returns(subsidiaryList);

            // Action
            _service.GetSubsidiaryList();

            // Assert
            _dataAccess.DidNotReceiveWithAnyArgs().GetSubsidiaryList(LicenseeName);
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
            // Arrange
            SetUp_GetCurrencyAgentList();

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
            SetUp_GetCurrencyAgentList();
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
            SetUp_GetCurrencyAgentList();
            var expectedResult = GetAgentByCurrencyData.Where(i => i.Key == currencyId || currencyId == 0)
                .SelectMany(i => i.Value).ToList()
                .Except(GetSubsidiaryData.Select(i => i.AgentId))
                .Union(GetAgentListBySubsidairyNames(subsidiaryNames, currencyId));
            // Action
            var result = _service.GetCurrencyAgentList(subsidiaryNames, false, currencyId)
            .SelectMany(i => i.Value);

            // Assert
            Assert.AreEqual(expectedResult.Count(), result.Count());

            Assert.AreEqual(0, result.Except(expectedResult).Count());

        }
        private void SetUp_GetCurrencyAgentList()
        {
            _sessionService.UserSession.Returns(new UserSession
            {
                CurrencyList = new Dictionary<int, string>() { { 20, "UUS" }, { 51, "IDR" }, { 13, "CNY" } },
                CurrencyAgentList = GetAgentByCurrencyData
            });

            _dataAccess.GetSubsidiaryList(Arg.Any<string>()).Returns(GetSubsidiaryData);
            _cache.Get<IEnumerable<Subsidiary>>(Arg.Any<string>(), Arg.Any<Func<IEnumerable<Subsidiary>>>(), Arg.Any<int>())
                .Returns(GetSubsidiaryData);
        }
        private static List<int> GetAgentListBySubsidairyNames(string subsidiaryNames, int currencyId)
        {
            var subList = subsidiaryNames.Split(new char[] { ',' });
            var agentBySub = GetSubsidiaryData.Where(i =>
              subList.Contains(i.OperatorId) && (currencyId == 0 || currencyId == i.CurrencyId)).Select(i => i.AgentId);

            return agentBySub.Intersect(GetAgentByCurrencyData.SelectMany(i => i.Value)).ToList();

        }

        private static IDictionary<int, IEnumerable<int>> GetAgentByCurrencyData
        {
            get
            {
                var data = new Dictionary<int, IEnumerable<int>>
                {
                    { 20, new List<int>{201, 202, 203,204,205 } },
                    {
                        51,new List<int>{511,512,513,514,515}
                    },
                    {
                        13,new List<int>{131,132}
                    }
                };
                return data;
            }

        }
        private static IEnumerable<Subsidiary> GetSubsidiaryData
        {
            get
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
            }
        }
    }
}