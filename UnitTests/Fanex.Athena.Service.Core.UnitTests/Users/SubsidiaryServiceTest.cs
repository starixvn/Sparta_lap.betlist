namespace Fanex.Athena.Service.Report.UnitTests
{
    using System.Collections.Generic;
    using Core.Users.Interfaces;
    using DataAccess.Core.Users.Interfaces;
    using Extension.Utility.Interfaces;
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
    }
}