namespace Fanex.Athena.Api.Report.UnitTests
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Hosting;
    using Fanex.Athena.Service.Report.Interfaces;
    using Model.Core.DTO.Users;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    public class PlatformWinLossReportControllerTest
    {
        private const int ExpectedCustomerComm = 999;
        private PlatformWinLossReportController _controller;
        private IProductService _productService;
        private IPlatformWinLossService _service;
        private ISessionService _sessionService;

        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            _sessionService = Substitute.For<ISessionService>();
            _productService = Substitute.For<IProductService>();
            _service = Substitute.For<IPlatformWinLossService>();
            _controller = Substitute.ForPartsOf<PlatformWinLossReportController>(_sessionService, _productService, _service);
            _controller.Request = new HttpRequestMessage();
            _controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            _sessionService.GenerateAgentListXml(Arg.Any<int>(), Arg.Any<bool>()).Returns("XmlData");

          

            // Mock data for Level = 1
            var memberData = new List<PlatformWinLossMember>
            {
                new PlatformWinLossMember(),
                new PlatformWinLossMember(),
                new PlatformWinLossMember()
            };
            PlatformWinLossMember totalMemberData;
            _service.GetMemberData(Arg.Any<PlatformWinLossMetaModel>(), out totalMemberData).Returns(x =>
            {
                x[1] = new PlatformWinLossMember
                {
                    CustomerComm = ExpectedCustomerComm
                };
                return memberData;
            });

            CreateSession();
        }

        [OneTimeTearDown]
        public void FixtureTearDown()
        {
            _service = null;
            _controller = null;
            _sessionService = null;
        }

        [TestCase(2)]
        public void Index_WhenCallWithLevel0_CallAndGetCorrectDataFromService(int numberOfRow)
        {
            // Arrange
            PlatformWinLossSummary totalData;

            // Mock data for Level = 0
            FakeSummaryData(numberOfRow);

            // Action
            var result = _controller.Index(new PlatformWinLossMetaModel
            {
                Level = 0
            });

            // Assert
            Assert.Multiple(() =>
            {
                _service.Received().GetSummaryData(Arg.Any<PlatformWinLossMetaModel>(), out totalData);
                Assert.That(result.Data.Count, Is.EqualTo(numberOfRow));
                Assert.That(result.Total.CustomerComm, Is.EqualTo(ExpectedCustomerComm));
            });
        }

        private void FakeSummaryData(int numberOfRow)
        {
            var summaryData = new List<PlatformWinLossSummary>();

            for (var i = 0; i < numberOfRow; i++)
            {
                summaryData.Add(new PlatformWinLossSummary());
            }

            PlatformWinLossSummary totalSummaryData;
            _service.GetSummaryData(Arg.Any<PlatformWinLossMetaModel>(), out totalSummaryData).Returns(x =>
            {
                x[1] = new PlatformWinLossSummary
                {
                    CustomerComm = ExpectedCustomerComm
                };
                return summaryData;
            });
        }

        [Test]
        public void Index_WhenCallWithLevel1_CallAndGetCorrectDataFromService()
        {
            // Arrange
            PlatformWinLossMember totalData;
            var metaModel = new PlatformWinLossMetaModel
            {
                Level = 1
            };

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _service.Received().GetMemberData(Arg.Any<PlatformWinLossMetaModel>(), out totalData);
                Assert.That(result.Data.Count, Is.EqualTo(3));
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
                },
                Language = "en-US"
            };

            var licenseeSession = new LicenseeSession
            {
                LicenseeId = 1,
                IsAllowedCustomerComm = true
            };

            _sessionService.UserSession.Returns(userSession);
            _sessionService.LicenseeSession.Returns(licenseeSession);
        }
    }
}