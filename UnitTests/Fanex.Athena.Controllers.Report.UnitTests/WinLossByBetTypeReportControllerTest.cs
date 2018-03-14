namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Base.UnitTests;
    using DataAccess.Core.Configuration.Interfaces;
    using DataAccess.Report.Interfaces;
    using Excel.Base.Entities;
    using Excel.Report;
    using Extension.Utility;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Configuration;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Model.Report.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report;
    using Service.Core.Common.Interfaces;
    using Web.ResultExtension;

    [TestFixture]
    public class WinLossByBetTypeReportControllerTest : BaseControllerHelperTest
    {
        private const int CurrencyId = 999;

        private const string AgentListXml = "<abc>";

        private static readonly DateTime BalanceUpDate = new DateTime(2017, 3, 4);

        private IBalanceUpDataAccess _balanceUpDataAccess;

        private IWinLossByBetTypeDataAccess _winLossByBetTypeDataAccess;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            _balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _winLossByBetTypeDataAccess = Substitute.For<IWinLossByBetTypeDataAccess>();

            _balanceUpDataAccess.GetBalanceUp().Returns(new BalanceUp
            {
                BalanceUpDay = BalanceUpDate
            });

            base.FixtureSetUp();

            SetUpDefaultFakeData();

            SessionService.GenerateAgentListXml(CurrencyId).Returns(AgentListXml);
        }

        [Test]
        public void Index_WhenGet_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Action
                var viewResult = controller.Index() as ViewResult;
                var viewModel = viewResult.Model as WinLossByBetTypeViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Report/WinLossByBetType/Main.cshtml"));
                    Assert.That(viewModel.Param.LicenseeId, Is.EqualTo(SessionService.LicenseeSession.LicenseeId));
                    Assert.That(viewModel.Param.UserId, Is.EqualTo(SessionService.UserSession.UserId));
                    Assert.That(viewModel.BalanceUpDay, Is.EqualTo(BalanceUpDate));
                    Assert.That(viewModel.CurrencyList, Is.EqualTo(SessionService.UserSession.CurrencyList));
                });
            }
        }

        [TestCase(null, false, 6)]
        [TestCase("BA", false, 8)]
        [TestCase("RB", false, 8)]
        [TestCase(null, true, 8)]
        [TestCase("BA", true, 10)]
        [TestCase("RB", true, 10)]
        [TestCase("BA,RB", true, 12)]
        public void IndexPost_NotExportExcel_ReturnCorrectViewResult(
            string productCodeList,
            bool isAllowedCustomerComm,
            int expectedColumnCount)
        {
            using (var controller = CreateController(productCodeList))
            {
                // Arrange
                SessionService.LicenseeSession.IsAllowedCustomerComm = isAllowedCustomerComm;
                var metaModel = new WinLossByBetTypeMetaModel
                {
                    CurrencyId = CurrencyId,
                    IsExportExcel = false
                };

                var expectedData = new SortableList<WinLossByBetType>();
                _winLossByBetTypeDataAccess.GetDataList(metaModel).Returns(expectedData);

                // Action
                var viewResult = controller.Index(metaModel) as ViewResult;
                var viewModel = viewResult.Model as WinLossByBetTypeViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Report/WinLossByBetType/Main.cshtml"));
                    Assert.That(viewModel.Param, Is.EqualTo(metaModel));
                    Assert.That(viewModel.BalanceUpDay, Is.EqualTo(BalanceUpDate));
                    Assert.That(viewModel.Param.AgentListXml, Is.EqualTo(AgentListXml));
                    Assert.That(viewModel.CurrencyList, Is.EqualTo(SessionService.UserSession.CurrencyList));
                    Assert.That(viewModel.MainResult, Is.EqualTo(expectedData));
                    Assert.That(viewModel.ColumnCount, Is.EqualTo(expectedColumnCount));
                });
            }
        }

        [TestCase(null, false, false)]
        [TestCase("BA,RB,NG,SB,VS", true, true)]
        public void IndexPost_ExportExcel_ReturnCorrectResult(
            string productCodeList, bool expectedProductVisible, bool isAllowCustomerComm)
        {
            using (var controller = CreateController(productCodeList))
            {
                // Arrange
                SessionService.LicenseeSession.IsAllowedCustomerComm = isAllowCustomerComm;
                MockCreatingExcel(controller);
                var metaModel = new WinLossByBetTypeMetaModel
                {
                    BaseCurrency = "UUS",
                    CurrencyName = "SSU",
                    CurrencyId = CurrencyId,
                    IsExportExcel = true
                };
                var expectedData = new SortableList<WinLossByBetType>();
                _winLossByBetTypeDataAccess.GetDataList(metaModel).Returns(expectedData);

                // Action
                var result = controller.Index(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    controller.Received(1).CreateExcel(
                        Arg.Any<string>(),
                        Arg.Is<WinLossByBetTypeViewModel>(x => x.MainResult == expectedData),
                        Arg.Is<ConvertingOption>(x =>
                            x.IsConvert == metaModel.IsConvert &&
                            x.BaseCurrency == metaModel.BaseCurrency &&
                            x.CurrencyName == metaModel.CurrencyName),
                        Arg.Is<ShowingOption>(x =>
                            x.RBVisible == expectedProductVisible &&
                            x.BAVisible == expectedProductVisible &&
                            x.NGVisible == expectedProductVisible &&
                            x.SBVisible == expectedProductVisible &&
                            x.VSVisible == expectedProductVisible &&
                            x.CustomerCommVisible == isAllowCustomerComm));
                    Assert.That(result, Is.InstanceOf<ExcelResult>());
                });
            }
        }

        private static void MockCreatingExcel(WinLossByBetTypeReportController controller)
        {
            var excel = Substitute.ForPartsOf<WinLossByBetTypeExcel>();
            excel.When(x => x.Export()).DoNotCallBase();
            controller.CreateExcel(string.Empty, null, null, null).ReturnsForAnyArgs(excel);
        }

        private WinLossByBetTypeReportController CreateController(string productCodeList = null)
        {
            var productList = string.IsNullOrEmpty(productCodeList) ?
                new List<Product>() :
                productCodeList.Split(',').Select(x => new Product
                {
                    ProductCode = x
                });
            var productService = Substitute.For<IProductService>();
            productService.GetProductAccess(
                SessionService.LicenseeSession.LicenseeId,
                SessionService.UserSession.Language).Returns(new ProductAccess(productList.ToList()));
            var controller = Substitute.ForPartsOf<WinLossByBetTypeReportController>(
                LangService,
                SessionService,
                AppSettings,
                ClientViewHelper,
                productService,
                _balanceUpDataAccess,
                _winLossByBetTypeDataAccess);
            controller.When(x => x.CreateExcel(string.Empty, null, null, null)).DoNotCallBase();
            return controller;
        }
    }
}