namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Linq;
    using System.Web.Mvc;
    using Base.UnitTests;
    using DataAccess.Core.Configuration.Interfaces;
    using Excel.Base.Entities;
    using Excel.Report;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Configuration;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Model.Report.ViewModel;
    using Model.Report.ViewModel.LiveReports;
    using NSubstitute;
    using NUnit.Framework;
    using Report;
    using Service.Core.Common.Interfaces;
    using Service.Report.Interfaces;
    using Web.ResultExtension;

    [TestFixture]
    public class MatchWinLossReportControllerTest : BaseControllerHelperTest
    {
        private const int CurrencyId = 999;

        private const string AgentListXml = "melody";

        private static readonly DateTime BalanceUpDate = new DateTime(2017, 3, 4);

        private IBalanceUpDataAccess _balanceUpDataAccess;

        private IMatchWinLossService _matchWinLossService;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            _balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _matchWinLossService = Substitute.For<IMatchWinLossService>();

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
                var viewModel = viewResult.Model as MatchWinLossViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Report/MatchWinLoss/Main.cshtml"));
                    Assert.That(viewModel.CurrencyList, Is.EqualTo(SessionService.UserSession.CurrencyList));
                    Assert.That(viewModel.Param.BalanceUpDay, Is.EqualTo(BalanceUpDate.ToShortDateString()));
                    Assert.That(viewModel.Param.LicenseeId, Is.EqualTo(SessionService.LicenseeSession.LicenseeId));
                });
            }
        }

        [Test]
        public void IndexPost_NotExportExcel_ReturnCorrectViewResult()
        {
            using (var controller = CreateController("AB,BC,CD"))
            {
                // Arrange
                var metaModel = new MatchWinLossMetaModel
                {
                    CurrencyId = CurrencyId,
                    IsExportExcel = false
                };

                var expectedData = new List<MatchWinLoss>
                {
                    new MatchWinLoss()
                };
                _matchWinLossService.GetDataList(metaModel).Returns(expectedData);

                // Action
                var viewResult = controller.Index(metaModel) as ViewResult;
                var viewModel = viewResult.Model as MatchWinLossViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Report/MatchWinLoss/Main.cshtml"));
                    Assert.That(viewModel.Param, Is.EqualTo(metaModel));
                    Assert.That(viewModel.Param.AgentListXml, Is.EqualTo(AgentListXml));
                    Assert.That(viewModel.MainResult, Is.EqualTo(expectedData));
                });
            }
        }

        [TestCase("BA,RB,NG,LC,SB,VS,KN", true)]
        [TestCase("hihi", false)]
        public void IndexPost_ExportExcel_PassParamsToCreateExcelCorrectly(string productCodeList, bool expectedProductVisible)
        {
            using (var controller = CreateController(productCodeList))
            {
                // Arrange
                controller.ControllerContext = ControllerContext;
                controller.Request.Form.Returns(new NameValueCollection());
                MockCreatingExcel(controller);
                var metaModel = new MatchWinLossMetaModel
                {
                    BaseCurrency = "UUS",
                    CurrencyName = "All",
                    CurrencyId = 0,
                    IsExportExcel = true
                };
                var expectedData = new DataSet();
                expectedData.Tables.Add(new DataTable("MainData"));
                expectedData.Tables.Add(new DataTable("TotalData"));

                _matchWinLossService.GetDetailDataSet(metaModel).Returns(expectedData);

                // Action
                var result = controller.Index(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    controller.Received(1).CreateExcel(
                        Arg.Any<string>(),
                        metaModel,
                        Arg.Is<ShowingOption>(x => x.BAVisible == expectedProductVisible &&
                                            x.RBVisible == expectedProductVisible &&
                                            x.NGVisible == expectedProductVisible &&
                                            x.LCVisible == expectedProductVisible &&
                                            x.SBVisible == expectedProductVisible &&
                                            x.VSVisible == expectedProductVisible &&
                                            x.KNVisible == expectedProductVisible),
                        Arg.Is<ConvertingOption>(x =>
                            x.IsConvert == metaModel.IsConvert &&
                            x.BaseCurrency == metaModel.BaseCurrency &&
                            x.CurrencyName == metaModel.CurrencyName),
                        Arg.Is<DataTable>(x => x.TableName == "MainData"),
                        Arg.Is<DataTable>(x => x.TableName == "TotalData"));
                    Assert.That(result, Is.InstanceOf<ExcelResult>());
                });
            }
        }

        [Test]
        public void Detail_WhenGet_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new MatchWinLossMetaModel
                {
                    CurrencyId = CurrencyId
                };
                var dataList = new List<MatchWinLoss>
                {
                    new MatchWinLoss()
                };
                _matchWinLossService.GetDetailDataList(metaModel).Returns(dataList);

                // Action
                var viewResult = controller.Detail(metaModel) as ViewResult;
                var viewModel = viewResult.Model as MatchWinLossViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Report/MatchWinLoss/Content.data.Detail.cshtml"));
                    Assert.That(viewModel.MainResult, Is.EqualTo(dataList));
                    Assert.That(metaModel.AgentListXml, Is.EqualTo(AgentListXml));
                });
            }
        }

        private static void MockCreatingExcel(MatchWinLossReportController controller)
        {
            var excel = Substitute.ForPartsOf<MatchWinLossExcel>();
            excel.When(x => x.Export()).DoNotCallBase();
            controller.CreateExcel(string.Empty, null, null, null, null, null).ReturnsForAnyArgs(excel);
        }

        private MatchWinLossReportController CreateController(string productCodeList = null)
        {
            var productList = string.IsNullOrEmpty(productCodeList) ?
                new List<Product>() :
                productCodeList.Split(',').Select(x => new Product
                {
                    ProductCode = x
                });
            var productService = Substitute.For<IProductService>();
            productService.GetSingleWalletProductAccess(
                SessionService.LicenseeSession.LicenseeId,
                SessionService.UserSession.Language,
                FunctionNames.MatchWinLoss).Returns(new ProductAccess(productList.ToList()));
            var controller = Substitute.ForPartsOf<MatchWinLossReportController>(
                LangService,
                SessionService,
                AppSettings,
                productService,
                _balanceUpDataAccess,
                _matchWinLossService);
            controller.When(x => x.CreateExcel(string.Empty, null, null, null, null, null)).DoNotCallBase();
            return controller;
        }
    }
}