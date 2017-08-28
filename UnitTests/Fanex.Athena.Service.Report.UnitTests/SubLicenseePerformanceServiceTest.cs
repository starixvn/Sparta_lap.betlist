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

        private ISubsidiaryPerformanceDataAccess _dataAccess;

        private SubsidiaryPerformanceService _service;

        [SetUp]
        public void SetUp()
        {
            _balanceService = new BalanceUpService();
            _dataAccess = Substitute.For<ISubsidiaryPerformanceDataAccess>();
            _service = new SubsidiaryPerformanceService(_balanceService, _dataAccess);
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