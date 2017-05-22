namespace Fanex.Athena.Api.Report.UnitTests
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Hosting;
    using System.Web.Http.Results;
    using Fanex.Athena.Service.Report.Interfaces;
    using Model.Core.DTO.Users;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    public class PlatformWinLossReportTest
    {
        private ISessionService _sessionService;
        private IProductService _productService;
        private IPlatformWinLossService _service;
        private PlatformWinLossReportController _controller;

        [SetUp]
        public void SetUp()
        {
            _sessionService = Substitute.For<ISessionService>();
            _productService = Substitute.For<IProductService>();
            _service = Substitute.For<IPlatformWinLossService>();
            _controller = Substitute.ForPartsOf<PlatformWinLossReportController>(_sessionService, _productService, _service);
            _controller.Request = new HttpRequestMessage();
            _controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
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
            var metaModel = new PlatformWinLossMetaModel { CurrencyId = 99 };

            // Action
            var result = _controller.Index(metaModel) as BadRequestErrorMessageResult;

            // Assert
            Assert.That(result.Message, Is.EqualTo(ViewResources.InvalidParams));
        }

        [Test]
        public void Index_WhenCallWithLevel0_CallAndGetCorrectDataFromService()
        {
            // Arrange
            const int ExpectedCustomerComm = 999;
            var dataListResult = new List<PlatformWinLossSummary>
            {
                new PlatformWinLossSummary(),
                new PlatformWinLossSummary()
            };
            PlatformWinLossSummary totalData;
            _service.GetSummaryData(Arg.Any<PlatformWinLossMetaModel>(), out totalData).Returns(x =>
            {
                x[1] = new PlatformWinLossSummary
                {
                    CustomerComm = ExpectedCustomerComm
                };
                return dataListResult;
            });

            // Action
            var result = _controller.Index(new PlatformWinLossMetaModel());

            // Assert
            Assert.Multiple(() =>
            {
                _service.Received().GetSummaryData(Arg.Any<PlatformWinLossMetaModel>(), out totalData);
                Assert.That(result.Data.Count, Is.EqualTo(2));
                Assert.That(result.Total.CustomerComm, Is.EqualTo(ExpectedCustomerComm));
            });
        }

        [Test]
        public void Index_WhenCallWithLevel1_CallAndGetCorrectDataFromService()
        {
            // Arrange
            const int ExpectedCustomerComm = 999;
            var dataListResult = new List<PlatformWinLossSummary>
            {
                new PlatformWinLossSummary(),
                new PlatformWinLossSummary()
            };
            PlatformWinLossSummary totalData;
            _service.GetSummaryData(Arg.Any<PlatformWinLossMetaModel>(), out totalData).Returns(x =>
            {
                x[1] = new PlatformWinLossSummary
                {
                    CustomerComm = ExpectedCustomerComm
                };
                return dataListResult;
            });

            // Action
            var result = _controller.Index(new PlatformWinLossMetaModel());

            // Assert
            Assert.Multiple(() =>
            {
                _service.Received().GetSummaryData(Arg.Any<PlatformWinLossMetaModel>(), out totalData);
                Assert.That(result.Data.Count, Is.EqualTo(2));
                Assert.That(result.Total.CustomerComm, Is.EqualTo(ExpectedCustomerComm));
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