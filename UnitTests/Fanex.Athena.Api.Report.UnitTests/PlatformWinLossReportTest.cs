namespace Fanex.Athena.Api.Report.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Hosting;
    using System.Web.Http.Results;
    using Excel.Base.Entities;
    using Excel.Report;
    using Fanex.Athena.Service.Report.Interfaces;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Model.Core.MetaModel.Product;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Model.Report.ViewModel;
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

        [Test]
        public void Index_ExportExcelLevel1AndShowAllProducts_PassCorrectParamsToCreateExcel()
        {
            // Arrange
            const string Language = "en-US";
            const int LicenseeId = 1;
            const string FunctionName = "PlatformWinloss";
            var productAccess = new ProductAccess(new List<Product>());
            _productService.GetProductAccess(LicenseeId, Language, FunctionName).Returns(productAccess);

            const string ExpectedBaseCurrency = "ABC";
            const string ExpectedCurrencyName = "123";
            const bool IsVisibleAllProduct = true;
            var metaModel = new PlatformWinLossMetaModel
            {
                Level = 1,
                IsExportExcel = true,
                BaseCurrency = ExpectedBaseCurrency,
                CurrencyName = ExpectedCurrencyName,
                ProductOptions = GetProductOption(IsVisibleAllProduct)
            };

            var excelModel = new PlatformWinLossViewModel();
            var excel = Substitute.ForPartsOf<PlatformWinLossMemberExcel>(
                Arg.Any<string>(),
                excelModel,
                Arg.Any<ShowingOption>(),
                Arg.Any<ConvertingOption>());
            excel.When(x => x.Export()).DoNotCallBase();
            _controller.CreateExcelMember(
                Arg.Any<string>(),
                metaModel,
                excelModel,
                Arg.Any<ShowingOption>(),
                Arg.Any<ConvertingOption>())
                .ReturnsForAnyArgs(excel);

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _controller.Received().CreateExcelMember(
                    Arg.Any<string>(),
                    metaModel,
                    Arg.Is<PlatformWinLossViewModel>(x => x.ProductAccess == productAccess && x.IsAllowedCustomerCommission == true && x.Colspan == 7),
                    Arg.Is<ShowingOption>(x => x.CustomerCommVisible == true && CheckShowingOption(x, IsVisibleAllProduct)),
                    Arg.Is<ConvertingOption>(x => x.BaseCurrency == ExpectedBaseCurrency.ToLower() && x.CurrencyName == ExpectedCurrencyName));
                Assert.That(metaModel.AgentListXml, Is.EqualTo("XmlData"));
            });
        }

        [Test]
        public void Index_ExportExcelLevel1AndHideAllProducts_PassCorrectParamsToCreateExcel()
        {
            // Arrange
            const string Language = "en-US";
            const int LicenseeId = 1;
            const string FunctionName = "PlatformWinloss";
            var productAccess = new ProductAccess(new List<Product> { new Product { ProductCode = Product.BA }, new Product { ProductCode = Product.RB } });
            _productService.GetProductAccess(LicenseeId, Language, FunctionName).Returns(productAccess);

            const string ExpectedBaseCurrency = "ABC";
            const string ExpectedCurrencyName = "123";
            const bool IsVisibleAllProduct = false;
            var metaModel = new PlatformWinLossMetaModel
            {
                Level = 1,
                IsExportExcel = true,
                BaseCurrency = ExpectedBaseCurrency,
                CurrencyName = ExpectedCurrencyName,
                ProductOptions = GetProductOption(IsVisibleAllProduct)
            };

            var excelModel = new PlatformWinLossViewModel();
            var excel = Substitute.ForPartsOf<PlatformWinLossMemberExcel>(
                Arg.Any<string>(),
                excelModel,
                Arg.Any<ShowingOption>(),
                Arg.Any<ConvertingOption>());
            excel.When(x => x.Export()).DoNotCallBase();
            _controller.CreateExcelMember(
                Arg.Any<string>(),
                metaModel,
                excelModel,
                Arg.Any<ShowingOption>(),
                Arg.Any<ConvertingOption>())
                .ReturnsForAnyArgs(excel);

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _controller.Received().CreateExcelMember(
                    Arg.Any<string>(),
                    metaModel,
                    Arg.Is<PlatformWinLossViewModel>(x => x.ProductAccess == productAccess && x.IsAllowedCustomerCommission == true && x.Colspan == 11),
                    Arg.Is<ShowingOption>(x => x.CustomerCommVisible == true && CheckShowingOption(x, IsVisibleAllProduct)),
                    Arg.Is<ConvertingOption>(x => x.BaseCurrency == ExpectedBaseCurrency.ToLower() && x.CurrencyName == ExpectedCurrencyName));
                Assert.That(metaModel.AgentListXml, Is.EqualTo("XmlData"));
            });
        }

        private bool CheckShowingOption(ShowingOption option, bool isVisible)
        {
            return option.BAVisible == isVisible &&
                option.RBVisible == isVisible &&
                option.NGVisible == isVisible &&
                option.LCVisible == isVisible &&
                option.SBVisible == isVisible &&
                option.VSVisible == isVisible &&
                option.CSVisible == isVisible &&
                option.KNVisible == isVisible;
        }

        private IList<ProductOption> GetProductOption(bool status)
        {
            var productOptions = new List<string>
            {
                Product.BA,
                Product.RB,
                Product.NG,
                Product.LC,
                Product.SB,
                Product.VS,
                Product.CS,
                Product.KN
            };

            return productOptions.Select(p => new ProductOption { Code = p, Status = status }).ToList();
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