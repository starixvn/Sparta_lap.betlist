namespace Fanex.Athena.Api.Report.UnitTests
{
    using System.Collections.Generic;
    using Fanex.Athena.Service.Report.Interfaces;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [TestFixture]
    public class SubLicenseePerformanceReportControllerTest
    {
        private ISessionService _sessionService;

        private IProductService _productService;

        private ISubsidiaryPerformanceService _service;

        private SubsidiaryPerformanceReportController _controller;

        [OneTimeSetUp]
        public void SetUp()
        {
            _sessionService = Substitute.For<ISessionService>();
            _productService = Substitute.For<IProductService>();
            _service = Substitute.For<ISubsidiaryPerformanceService>();
            _controller = Substitute.ForPartsOf<SubsidiaryPerformanceReportController>(_sessionService, _productService, _service);
            CreateSession();
        }

        [OneTimeTearDown]
        public void FixtureTearDown()
        {
            _service = null;
            _controller = null;
            _sessionService = null;
        }

        private void CreateSession()
        {
            var userSession = new UserSession
            {
                CurrencyList = new Dictionary<int, string>
                {
                    { -1, "EUR" }, { -2, "USD" }
                }
            };

            var licenseeSession = new LicenseeSession
            {
                LicenseeId = 1
            };

            _sessionService.UserSession.Returns(userSession);
            _sessionService.LicenseeSession.Returns(licenseeSession);
        }
    }
}