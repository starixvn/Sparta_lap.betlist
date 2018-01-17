namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System.Web.Mvc;
    using Base.UnitTests;
    using Excel.Report.RNGCasino;
    using Model.Casino.DTO;
    using Model.Casino.MetaModel;
    using Model.Casino.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report.RNGCasino;
    using Service.Casino.Interfaces;
    using Web.ResultExtension;

    [TestFixture]
    public partial class HandHistoryControllerTest : BaseControllerHelperTest
    {
        private IRNGCasinoService _rngCasinoService;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            _rngCasinoService = Substitute.For<IRNGCasinoService>();
            base.FixtureSetUp();

            SetUpDefaultFakeData();
        }

        [TearDown]
        public void TearDown()
        {
            _rngCasinoService.ClearReceivedCalls();
        }

        [Test]
        public void RNGCasino_WhenCall_ReturnCorrectResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new RNGCasinoResultMetaModel();

                // Action
                var result = controller.RNGCasino(metaModel) as ViewResult;
                var viewModel = result.Model as RNGCasinoResultViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/RNGCasino/Main.cshtml"));
                    Assert.That(viewModel.Param, Is.EqualTo(metaModel));
                });
            }
        }

        [Test]
        public void RNGCasino_ExportExcel_ReturnCorrectResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new RNGCasinoResultMetaModel
                {
                    IsExportExcel = true
                };
                var excel = Substitute.ForPartsOf<HandHistoryExcel>();
                excel.When(x => x.Export()).DoNotCallBase();
                controller.CreateExcel(string.Empty, null, null, null).ReturnsForAnyArgs(excel);

                // Action
                var result = controller.RNGCasino(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    controller.Received(1).CreateExcel(
                        Arg.Any<string>(),
                        metaModel,
                        Arg.Any<RNGCasinoResult>(),
                        Arg.Any<RNGCasinoResultViewModel>());
                    Assert.That(result, Is.InstanceOf<ExcelResult>());
                });
            }
        }

        private HandHistoryController CreateController()
        {
            var controller = Substitute.ForPartsOf<HandHistoryController>(
                LangService,
                SessionService,
                AppSettings,
                ClientViewHelper,
                _rngCasinoService);
            controller.ControllerContext = ControllerContext;
            controller.WhenForAnyArgs(x => x.CreateExcel(string.Empty, null, null, null)).DoNotCallBase();
            return controller;
        }
    }
}