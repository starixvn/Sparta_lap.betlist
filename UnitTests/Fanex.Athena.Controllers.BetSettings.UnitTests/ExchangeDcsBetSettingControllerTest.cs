namespace Fanex.Athena.Controllers.BetSettings.UnitTests
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Model.BetSettings.MetaModel;
    using Model.BetSettings.ViewModel;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using Service.BetSettings.Interfaces;

    public class ExchangeDcsBetSettingControllerTest : BetSettingBaseControllerHelperTest
    {
        private IDcsBetSettingService _betSettingService;
        private IExchangeCommApiService _exchangeCommApiService;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _betSettingService = Substitute.For<IDcsBetSettingService>();
            _exchangeCommApiService = Substitute.For<IExchangeCommApiService>();

            SetUpDefaultFakeData();
        }

        [SetUp]
        public void SetUp()
        {
            SetUpDefaultFakeBetSettingData();
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task Index_CallApiSuccessfully_ReturnCorrectViewResult(bool? isEnabled)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetSettingMetaModel { CustId = 100 };

                _betSettingService.GetStatusAsync(DefaultLicenseeSession.LicenseeId, metaModel.CustId, metaModel.ProductId)
                    .Returns(Task.FromResult(isEnabled));

                const double ExtendedData = 10d;
                _exchangeCommApiService.GetCommission(DefaultLicenseeSession.LicenseeId, metaModel.CustId)
                    .Returns(new ResultInfo<double> { ExtendedData = ExtendedData });

                // Action
                var result = await controller.Index(metaModel) as ViewResult;
                var viewModel = (ExchangeBetSettingViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    AssertRequiredProperties(viewModel, metaModel);
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/Customer/BetSetting/Exchange.cshtml"));
                    Assert.That(viewModel.IsEnabled, Is.EqualTo(isEnabled));
                    Assert.That(viewModel.Commission, Is.EqualTo(ExtendedData));
                });
            }
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task Index_CallApiFailed_ReturnCorrectViewResult(bool? isEnabled)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetSettingMetaModel { CustId = 100 };

                _betSettingService.GetStatusAsync(DefaultLicenseeSession.LicenseeId, metaModel.CustId, metaModel.ProductId)
                    .Returns(Task.FromResult(isEnabled));

                _exchangeCommApiService.GetCommission(DefaultLicenseeSession.LicenseeId, metaModel.CustId)
                    .Returns(new ResultInfo<double> { Code = 1, Message = "Error" });

                // Action
                var result = await controller.Index(metaModel) as ViewResult;
                var viewModel = (ExchangeBetSettingViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    AssertRequiredProperties(viewModel, metaModel);
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/Customer/BetSetting/Exchange.cshtml"));
                    Assert.That(viewModel.IsEnabled, Is.EqualTo(isEnabled));
                    Assert.That(viewModel.IsNoInfo, Is.True);
                });
            }
        }

        [TestCase(1)]
        [TestCase(1.5)]
        [TestCase(7.5)]
        public async Task UpdateComm_InvalidNewCommissionValue_ReturnFailedResult(double newComm)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetSettingMetaModel { CustId = 100 };

                // Action
                var result = await controller.UpdateComm(0, 0, newComm) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.EqualTo(100));
                    Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.InvalidChargeCommission));
                });
            }
        }

        [TestCase(2)]
        [TestCase(3.5)]
        [TestCase(5.5)]
        public async Task UpdateComm_ValidNewCommissionValue_ReturnCorrectResult(double newComm)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetSettingMetaModel { CustId = 100 };
                var expectedResultInfo = new ResultInfo<double> { Code = 1, Message = "Test" };

                _exchangeCommApiService.UpdateCommissionAsync(DefaultLicenseeSession.LicenseeId, DefaultUserSession.UserId, 0, 0, newComm)
                    .Returns(Task.FromResult<IResultInfo<double>>(expectedResultInfo));

                // Action
                var result = await controller.UpdateComm(0, 0, newComm) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.EqualTo(expectedResultInfo.Code));
                    Assert.That(resultInfo.Message, Is.EqualTo(expectedResultInfo.Message));
                });
            }
        }

        private ExchangeDcsBetSettingController CreateController()
        {
            var controller = new ExchangeDcsBetSettingController(
                LangService,
                SessionService,
                AppSettings,
                CoreBetSettingService,
                ProductService,
                _betSettingService,
                _exchangeCommApiService);

            return controller;
        }
    }
}