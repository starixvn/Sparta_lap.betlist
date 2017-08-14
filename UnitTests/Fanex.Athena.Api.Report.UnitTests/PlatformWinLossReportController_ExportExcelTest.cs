namespace Fanex.Athena.Api.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Hosting;
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
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    public class PlatformWinLossReportController_ExportExcelTest
    {
        private const bool IsAllowedCustomerComm = true;
        private const int ExpectedCustomerComm = 999;
        private const string Language = "en-US";
        private const int LicenseeId = 1;
        private const string FunctionName = "PlatformWinloss";
        private PlatformWinLossReportController _controller;
        private IProductService _productService;
        private IPlatformWinLossService _service;
        private ISessionService _sessionService;
        private PlatformWinLossMetaModel _metalModel;

        [SetUp]
        public void SetUp()
        {
            _metalModel = new PlatformWinLossMetaModel
            {
                IsExportExcel = true
            };
        }

        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            FakeSession();
            _productService = Substitute.For<IProductService>();
            _service = Substitute.For<IPlatformWinLossService>();
            _controller = Substitute.ForPartsOf<PlatformWinLossReportController>(_sessionService, _productService, _service);
            _controller.Request = new HttpRequestMessage();
            _controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            _controller.When(x => x.ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>())).DoNotCallBase();
            _sessionService.GenerateAgentListXml(Arg.Any<int>(), Arg.Any<bool>()).Returns("XmlData");
        }

        [OneTimeTearDown]
        public void FixtureTearDown()
        {
            _service = null;
            _controller = null;
            _sessionService = null;
        }

        [TearDown]
        public void TearDown()
        {
            _metalModel = null;
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Index_ExportExcelMemberWithLevel1_PassCustomerCommVisibleInShowingOptionToExcelCorrectly(bool hasLicenseeComm)
        {
            // Arrange
            FakeLicenseeSession(hasLicenseeComm);
            _metalModel.Level = 1;
            _metalModel.ProductOptions = RandomProductOption();
            FakeMemberData();
            FakeProductAccess();
            ShowingOption expetedShowingOption = null;

            var excelModel = new PlatformWinLossViewModel();
            var excel = Substitute.ForPartsOf<PlatformWinLossMemberExcel>(
               Arg.Any<string>(),
               excelModel,
               Arg.Do<ShowingOption>(x => expetedShowingOption = x),
               Arg.Any<ConvertingOption>(),
               Arg.Any<IEnumerable<ProductOption>>());
            excel.When(x => x.Export()).DoNotCallBase();

            _controller.CreateExcelMember(
                Arg.Any<string>(),
                _metalModel,
                excelModel,
                Arg.Any<ShowingOption>(),
                Arg.Any<ConvertingOption>())
                .ReturnsForAnyArgs(excel);

            // Action
            _controller.Index(_metalModel);

            // Assert
            Assert.IsTrue(expetedShowingOption.CustomerCommVisible == hasLicenseeComm);
        }

        [Test]
        public void Index_ExportExcelMemberWithLevel1_PassProductionOptionToExcelCorrectly()
        {
            // Arrange
            _metalModel.Level = 1;
            _metalModel.ProductOptions = RandomProductOption();
            FakeMemberData();
            FakeProductAccess();

            ShowingOption expetedShowingOption = null;

            var excelModel = new PlatformWinLossViewModel();
            var excel = Substitute.ForPartsOf<PlatformWinLossMemberExcel>(
               Arg.Any<string>(),
               excelModel,
               Arg.Do<ShowingOption>(x => expetedShowingOption = x),
               Arg.Any<ConvertingOption>(),
               Arg.Any<IEnumerable<ProductOption>>());
            excel.When(x => x.Export()).DoNotCallBase();

            _controller.CreateExcelMember(
                Arg.Any<string>(),
                _metalModel,
                excelModel,
                Arg.Any<ShowingOption>(),
                Arg.Any<ConvertingOption>())
                .ReturnsForAnyArgs(excel);

            // Action
            _controller.Index(_metalModel);

            // Assert
            VerifyShowingOption(_metalModel.ProductOptions, expetedShowingOption);
        }

        [Test]
        public void Index_ExportExcelSummaryWithLevel0_PassCorrectParamsToCreateExcel()
        {
            // Arrange
            const string ExpectedBaseCurrency = "ABC";
            const string ExpectedCurrencyName = "123";
            _metalModel.Level = 0;
            _metalModel.BaseCurrency = ExpectedBaseCurrency;
            _metalModel.CurrencyName = ExpectedCurrencyName;

            FakeGetSummaryData();

            var excel = Substitute.ForPartsOf<PlatformWinLossSummaryExcel>(
                string.Empty,
                _metalModel,
                Arg.Any<IList<PlatformWinLossSummary>>(),
                Arg.Any<PlatformWinLossSummary>(),
                Arg.Any<ConvertingOption>());

            excel.When(x => x.Export()).DoNotCallBase();

            _controller.CreateExcelSummary(
                null,
                _metalModel,
                Arg.Any<IList<PlatformWinLossSummary>>(),
                Arg.Any<PlatformWinLossSummary>(),
                Arg.Any<ConvertingOption>())
                .Returns(excel);

            // Action
            _controller.Index(_metalModel);

            // Assert
            Assert.Multiple(() =>
            {
                _controller.Received().CreateExcelSummary(
                    Arg.Any<string>(),
                    _metalModel,
                    Arg.Is<IList<PlatformWinLossSummary>>(x => x.Count == 2),
                    Arg.Is<PlatformWinLossSummary>(x => x.CustomerComm == ExpectedCustomerComm),
                    Arg.Is<ConvertingOption>(x => x.BaseCurrency == ExpectedBaseCurrency.ToLower() && x.CurrencyName == ExpectedCurrencyName));
                Assert.That(_metalModel.AgentListXml, Is.EqualTo("XmlData"));
            });
        }

        private static void VerifyShowingOption(IEnumerable<ProductOption> productOptions, ShowingOption expetedShowingOption)
        {
            Assert.Multiple(() =>
            {
                Assert.IsTrue(expetedShowingOption.SBVisible == productOptions.Any(i => i.Code == Product.SB && i.Status));
                Assert.IsTrue(expetedShowingOption.BAVisible == productOptions.Any(i => i.Code == Product.BA && i.Status));
                Assert.IsTrue(expetedShowingOption.BGVisible == productOptions.Any(i => i.Code == Product.BG && i.Status));
                Assert.IsTrue(expetedShowingOption.BTVisible == productOptions.Any(i => i.Code == Product.BT && i.Status));
                Assert.IsTrue(expetedShowingOption.COVisible == productOptions.Any(i => i.Code == Product.CO && i.Status));
                Assert.IsTrue(expetedShowingOption.CSVisible == productOptions.Any(i => i.Code == Product.CS && i.Status));
                Assert.IsTrue(expetedShowingOption.KNVisible == productOptions.Any(i => i.Code == Product.KN && i.Status));
                Assert.IsTrue(expetedShowingOption.LCVisible == productOptions.Any(i => i.Code == Product.LC && i.Status));
                Assert.IsTrue(expetedShowingOption.NGVisible == productOptions.Any(i => i.Code == Product.NG && i.Status));
                Assert.IsTrue(expetedShowingOption.RBVisible == productOptions.Any(i => i.Code == Product.RB && i.Status));
                Assert.IsTrue(expetedShowingOption.VSVisible == productOptions.Any(i => i.Code == Product.VS && i.Status));
            });
        }

        private static IList<ProductOption> RandomProductOption()
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

            var rand = new Random();

            return productOptions.Select(p => new ProductOption { Code = p, Status = rand.Next(1, 13) % 2 == 0 }).ToList();
        }

        private void FakeGetSummaryData()
        {
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
        }

        private void FakeProductAccess()
        {
            var productAccess = new ProductAccess(new List<Product> { new Product { ProductCode = Product.BA }, new Product { ProductCode = Product.RB } });
            _productService.GetProductAccess(LicenseeId, Language, FunctionName).Returns(productAccess);
        }

        private void FakeMemberData()
        {
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
        }

        private void FakeSession()
        {
            _sessionService = Substitute.For<ISessionService>();
            var userSession = new UserSession
            {
                CurrencyList = new Dictionary<int, string>
                {
                    { -1, "EUR" }, { -2, "USD" }
                },
                Language = Language
            };

            FakeLicenseeSession(IsAllowedCustomerComm);

            _sessionService.UserSession.Returns(userSession);
        }

        private void FakeLicenseeSession(bool isAllowCustomerComm)
        {
            var licenseeSession = new LicenseeSession
            {
                LicenseeId = LicenseeId,
                IsAllowedCustomerComm = isAllowCustomerComm
            };
            _sessionService.LicenseeSession.Returns(licenseeSession);
        }
    }
}