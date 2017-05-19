namespace Fanex.Athena.Api.Report.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Http.Results;
    using Fanex.Athena.Service.Report.Interfaces;
    using Model.Core.DTO.Configuration;
    using Model.Core.DTO.Users;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Configuration.Interfaces;
    using Service.Core.Users.Interfaces;

    public class WinLossByLeagueReportControllerTest
    {
        private ISessionService _sessionService;
        private IProductService _productService;
        private IBalanceUpService _balanceUpService;
        private IWinLossByLeagueService _service;
        private WinLossByLeagueReportController _controller;

        [SetUp]
        public void SetUp()
        {
            _sessionService = Substitute.For<ISessionService>();
            _productService = Substitute.For<IProductService>();
            _balanceUpService = Substitute.For<IBalanceUpService>();
            _service = Substitute.For<IWinLossByLeagueService>();
            _controller = Substitute.ForPartsOf<WinLossByLeagueReportController>(_sessionService, _productService, _balanceUpService, _service);

            _balanceUpService.BalanceUpInfo.Returns(new BalanceUp());
            CreateSession();
        }

        [TearDown]
        public void TearDown()
        {
            _service = null;
            _controller = null;
            _sessionService = null;
        }

        [Test]
        public void Index_WhenCallWithInvalidModel_ReturnBadRequestWithCorrectMessage()
        {
            // Arrange
            var metaModel = new WinLossByLeagueMetaModel { CurrencyId = 1 };

            // Action
            var result = _controller.Index(metaModel) as BadRequestErrorMessageResult;

            // Assert
            Assert.That(result.Message, Is.EqualTo(ViewResources.InvalidParams));
        }

        [Test]
        public void Index_WhenCallWithLevel0_CallAndGetCorrectDataFromService()
        {
            // Arrange
            var dataListResult = new List<WinLossByLeague>
            {
                new WinLossByLeague
                {
                    BetCount = 100
                },
                new WinLossByLeague
                {
                    BetCount = 200
                }
            };

            _service.GetWinlossByLeagues(Arg.Any<WinLossByLeagueMetaModel>()).Returns(dataListResult);

            // Action
            var result = _controller.Index(new WinLossByLeagueMetaModel());

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Data.Count, Is.EqualTo(2));
                Assert.That(result.Total.BetCount, Is.EqualTo(300));
            });
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