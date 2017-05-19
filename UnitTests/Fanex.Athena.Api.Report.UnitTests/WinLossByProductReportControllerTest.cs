namespace Fanex.Athena.Api.Report.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Http.Results;
    using System.Web.Script.Serialization;
    using Fanex.Athena.Service.Report.Interfaces;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report;
    using Resources;
    using Service.Core.Users.Interfaces;

    public class WinLossByProductReportControllerTest
    {
        private ISessionService _sessionService;
        private IWinLossByProductService _service;
        private WinLossByProductReportController _controller;
        private JavaScriptSerializer _serializer;

        [SetUp]
        public void SetUp()
        {
            _sessionService = Substitute.For<ISessionService>();
            _service = Substitute.For<IWinLossByProductService>();
            _controller = Substitute.ForPartsOf<WinLossByProductReportController>(_sessionService, _service);
            _serializer = new JavaScriptSerializer();
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
            var metaModel = new WinLossByProductMetaModel { CurrencyId = 1 };

            // Action
            var result = _controller.Index(metaModel) as BadRequestErrorMessageResult;

            // Assert
            Assert.That(result.Message, Is.EqualTo(ViewResources.InvalidParams));
        }

        [Test]
        public void Index_WhenCallWithLevel0_CallAndGetCorrectDataFromService()
        {
            // Arrange
            const int CurrencyId = -2, Level = 0, CustId = 1;
            _sessionService.GenerateAgentListXml(CurrencyId, Arg.Any<bool>()).Returns("XmlData");
            IDictionary<string, IList<WinLossByProduct>> summaryData;
            IResultInfo resultInfo;
            var dataListResult = new List<WinLossByProduct>
            {
                new WinLossByProduct()
            };

            var summaryTotalResult = new Dictionary<string, IList<WinLossByProduct>> { { "EUR", new List<WinLossByProduct>() } };
            _service.GetWinlossByProducts(Arg.Any<WinLossByProductMetaModel>(), out summaryData, out resultInfo).Returns(x =>
            {
                x[1] = summaryTotalResult;
                x[2] = new ResultInfo
                {
                    Code = 100
                };
                return dataListResult;
            });

            // Action
            var result = _controller.Index(new WinLossByProductMetaModel(), null, null, Level, CurrencyId, CustId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Data.Count, Is.EqualTo(1));
                Assert.That(result.SummaryTotal.Count, Is.EqualTo(1));
                Assert.That(result.ErrorMessage, Is.EqualTo(ViewResources.NoCustomerFoundMsg));
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