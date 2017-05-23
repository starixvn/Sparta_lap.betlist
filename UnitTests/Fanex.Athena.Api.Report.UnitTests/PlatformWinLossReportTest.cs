namespace Fanex.Athena.Api.Report.UnitTests
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Hosting;
    using System.Web.Http.Results;
    using Excel.Base.Entities;
    using Excel.Report;
    using Fanex.Athena.Service.Report.Interfaces;
    using Model.Core.DTO.Users;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    public class PlatformWinLossReportTest
    {
        private const int ExpectedCustomerComm = 999;

        private ISessionService _sessionService;
        private IProductService _productService;
        private IPlatformWinLossService _service;
        private PlatformWinLossReportController _controller;

        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            _sessionService = Substitute.For<ISessionService>();
            _productService = Substitute.For<IProductService>();
            _service = Substitute.For<IPlatformWinLossService>();
            _controller = Substitute.ForPartsOf<PlatformWinLossReportController>(_sessionService, _productService, _service);
            _controller.Request = new HttpRequestMessage();
            _controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            _controller.When(x => x.ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>())).DoNotCallBase();
            _sessionService.GenerateAgentListXml(Arg.Any<int>(), Arg.Any<bool>()).Returns("XmlData");

            // Mock data for Level = 0
            var summaryData = new List<PlatformWinLossSummary>
            {
                new PlatformWinLossSummary(),
                new PlatformWinLossSummary()
            };
            PlatformWinLossSummary totalSummaryData;
            _service.GetSummaryData(Arg.Any<PlatformWinLossMetaModel>(), out totalSummaryData).Returns(x =>
            {
                x[1] = new PlatformWinLossSummary
                {
                    CustomerComm = ExpectedCustomerComm
                };
                return summaryData;
            });

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
            PlatformWinLossSummary totalData;

            // Action
            var result = _controller.Index(new PlatformWinLossMetaModel
            {
                Level = 0
            });

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

        [Test]
        public void Index_ExportExcelLevel0_PassCorrectParamsToCreateExcel()
        {
            // Arrange
            const string ExpectedBaseCurrency = "ABC";
            const string ExpectedCurrencyName = "123";
            var metaModel = new PlatformWinLossMetaModel
            {
                Level = 0,
                IsExportExcel = true,
                BaseCurrency = ExpectedBaseCurrency,
                CurrencyName = ExpectedCurrencyName
            };

            var excel = Substitute.ForPartsOf<PlatformWinLossSummaryExcel>(
                string.Empty,
                metaModel,
                Arg.Any<IList<PlatformWinLossSummary>>(),
                Arg.Any<PlatformWinLossSummary>(),
                Arg.Any<ConvertingOption>());
            excel.When(x => x.Export()).DoNotCallBase();

            _controller.CreateExcelSummary(
                null,
                metaModel,
                Arg.Any<IList<PlatformWinLossSummary>>(),
                Arg.Any<PlatformWinLossSummary>(),
                Arg.Any<ConvertingOption>())
                .Returns(excel);

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _controller.Received().CreateExcelSummary(
                    Arg.Any<string>(),
                    metaModel,
                    Arg.Is<IList<PlatformWinLossSummary>>(x => x.Count == 2),
                    Arg.Is<PlatformWinLossSummary>(x => x.CustomerComm == ExpectedCustomerComm),
                    Arg.Is<ConvertingOption>(x => x.BaseCurrency == ExpectedBaseCurrency.ToLower() && x.CurrencyName == ExpectedCurrencyName));
                Assert.That(metaModel.AgentListXml, Is.EqualTo("XmlData"));
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