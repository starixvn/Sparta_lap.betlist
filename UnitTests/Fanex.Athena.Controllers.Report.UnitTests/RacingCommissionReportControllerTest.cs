namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System;
    using System.Web.Mvc;
    using Base.UnitTests;
    using DataAccess.Core.Configuration.Interfaces;
    using DataAccess.Report.Interfaces;
    using Excel.Base.Entities;
    using Excel.Report;
    using Extension.Utility;
    using Model.Core.DTO.Configuration;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Model.Report.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report;
    using Service.Core.Common.Interfaces;
    using Web.ResultExtension;

    [TestFixture]
    public class RacingCommissionReportControllerTest : BaseControllerHelperTest
    {
        private const int CurrencyId = 999;

        private const string AgentListXml = "<abc>";

        private static readonly DateTime BalanceUpDate = new DateTime(2017, 3, 4);

        private IBalanceUpDataAccess _balanceUpDataAccess;

        private IRacingCommissionDataAccess _racingCommissionDataAccess;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            _balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _racingCommissionDataAccess = Substitute.For<IRacingCommissionDataAccess>();

            _balanceUpDataAccess.GetBalanceUp().Returns(new BalanceUp
            {
                BalanceUpDay = BalanceUpDate
            });

            base.FixtureSetUp();

            SetUpDefaultFakeData();

            SessionService.GenerateAgentListXml(CurrencyId).Returns(AgentListXml);
        }

        [TearDown]
        public void TearDown()
        {
            _racingCommissionDataAccess.ClearReceivedCalls();
        }

        [Test]
        public void Index_WhenGet_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Action
                var viewResult = controller.Index() as ViewResult;
                var viewModel = viewResult.Model as RacingCommissionViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Report/RacingCommission/Main.cshtml"));
                    Assert.That(viewModel.Param.LicenseeId, Is.EqualTo(SessionService.LicenseeSession.LicenseeId));
                    Assert.That(viewModel.Param.UserId, Is.EqualTo(SessionService.UserSession.UserId));
                    Assert.That(viewModel.Param.BalanceUpDay, Is.EqualTo(BalanceUpDate));
                    Assert.That(viewModel.CurrencyList, Is.EqualTo(SessionService.UserSession.CurrencyList));
                });
            }
        }

        [Test]
        public void IndexPost_NotValidModel_NotCallDataAccess()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new RacingCommissionMetaModel
                {
                    CurrencyId = CurrencyId,
                    FromDate = new DateTime(2017, 2, 2),
                    ToDate = new DateTime(2017, 1, 1),
                    IsExportExcel = false
                };

                // Action
                controller.Index(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(metaModel.AgentListXml, Is.EqualTo(AgentListXml));
                    _racingCommissionDataAccess.DidNotReceiveWithAnyArgs().GetData(metaModel);
                });
            }
        }

        [Test]
        public void IndexPost_NotExportExcel_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new RacingCommissionMetaModel
                {
                    CurrencyId = CurrencyId,
                    FromDate = new DateTime(2017, 1, 1),
                    ToDate = new DateTime(2017, 2, 2),
                    IsExportExcel = false
                };

                var expectedData = new SortableReportResultModel<RacingCommission>
                {
                    DataList = new SortableList<RacingCommission>
                    {
                        new RacingCommission()
                    },
                    TotalData = new RacingCommission()
                };
                _racingCommissionDataAccess.GetData(metaModel).Returns(expectedData);

                // Action
                var viewResult = controller.Index(metaModel) as ViewResult;
                var viewModel = viewResult.Model as RacingCommissionViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Report/RacingCommission/Main.cshtml"));
                    Assert.That(viewModel.Param, Is.EqualTo(metaModel));
                    Assert.That(viewModel.Param.AgentListXml, Is.EqualTo(AgentListXml));
                    Assert.That(viewModel.CurrencyList, Is.EqualTo(SessionService.UserSession.CurrencyList));
                    Assert.That(viewModel.RacingCommissionList, Is.EqualTo(expectedData.DataList));
                    Assert.That(viewModel.Total, Is.EqualTo(expectedData.TotalData));
                });
            }
        }

        [Test]
        public void IndexPost_ExportExcel_ReturnCorrectResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                MockCreatingExcel(controller);
                var metaModel = new RacingCommissionMetaModel
                {
                    BaseCurrency = "UUS",
                    CurrencyName = "SSU",
                    CurrencyId = CurrencyId,
                    IsExportExcel = true
                };
                var expectedData = new SortableReportResultModel<RacingCommission>
                {
                    DataList = new SortableList<RacingCommission>
                    {
                        new RacingCommission()
                    },
                    TotalData = new RacingCommission()
                };
                _racingCommissionDataAccess.GetData(metaModel).Returns(expectedData);

                // Action
                var result = controller.Index(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    controller.Received(1).CreateExcel(
                        Arg.Any<string>(),
                        Arg.Is<RacingCommissionViewModel>(x =>
                            x.RacingCommissionList == expectedData.DataList &&
                            x.Total == expectedData.TotalData),
                        Arg.Is<ConvertingOption>(x =>
                            x.IsConvert == metaModel.IsConvert &&
                            x.BaseCurrency == metaModel.BaseCurrency &&
                            x.CurrencyName == metaModel.CurrencyName),
                        Arg.Any<ShowingOption>());
                    Assert.That(result, Is.InstanceOf<ExcelResult>());
                });
            }
        }

        private static void MockCreatingExcel(RacingCommissionReportController controller)
        {
            var excel = Substitute.ForPartsOf<RacingCommissionExcel>();
            excel.When(x => x.Export()).DoNotCallBase();
            controller.CreateExcel(string.Empty, null, null, null).ReturnsForAnyArgs(excel);
        }

        private RacingCommissionReportController CreateController()
        {
            var controller = Substitute.ForPartsOf<RacingCommissionReportController>(
                LangService,
                SessionService,
                AppSettings,
                ClientViewHelper,
                Substitute.For<IProductService>(),
                _balanceUpDataAccess,
                _racingCommissionDataAccess);
            controller.When(x => x.CreateExcel(string.Empty, null, null, null)).DoNotCallBase();
            return controller;
        }
    }
}