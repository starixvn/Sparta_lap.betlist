namespace Fanex.Athena.Service.Report.UnitTests
{
    using Core.Configuration;
    using DataAccess.Report.Interfaces;
    using Fanex.Athena.Service.Core.Configuration.Interfaces;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SubLicenseePerformanceServiceTest
    {
        private IBalanceUpService _balanceService;

        private ISubLicenseePerformanceDataAccess _dataAccess;

        private SubLicenseePerformanceService _service;

        [SetUp]
        public void SetUp()
        {
            _balanceService = new BalanceUpService();
            _dataAccess = Substitute.For<ISubLicenseePerformanceDataAccess>();
            _service = new SubLicenseePerformanceService(_balanceService, _dataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _balanceService = null;
            _dataAccess = null;
            _service = null;
        }
    }
}