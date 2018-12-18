namespace Fanex.Athena.Controllers.BetSettings.UnitTests
{
    using System.Collections.Generic;
    using Controllers.Base.UnitTests;
    using Model.BetSettings.DTO;
    using Model.BetSettings.MetaModel;
    using Model.BetSettings.ViewModel;
    using Model.Customer.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Service.BetSettings.Interfaces;
    using Service.Core.Common.Interfaces;

    public class BetSettingBaseControllerHelperTest : BaseControllerHelperTest
    {
        protected ICoreBetSettingService CoreBetSettingService { get; private set; }

        protected IProductService ProductService { get; private set; }

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            CoreBetSettingService = Substitute.For<ICoreBetSettingService>();
            ProductService = Substitute.For<IProductService>();
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();
            CoreBetSettingService = null;
            ProductService = null;
        }

        protected void SetUpDefaultFakeBetSettingData()
        {
            StubGetValueAppSettings("FakeLicenseeList", DefaultLicenseeSession.LicenseeId.ToString());
            CoreBetSettingService.GetCustomerInfo(Arg.Any<int>()).Returns(new CustomerInfo { SyncCasino = true });
            CoreBetSettingService.GetBetSettingProducts(Arg.Any<int>()).Returns(new List<BetSettingProduct>());
        }

        protected void AssertRequiredProperties(BetSettingBaseViewModel viewModel, BetSettingMetaModel metaModel, bool isFakeLicensee = false, bool isEditable = false)
        {
            Assert.That(metaModel.IsFakeLicensee, Is.EqualTo(isFakeLicensee));
            Assert.That(viewModel.Param, Is.EqualTo(metaModel));
            Assert.That(viewModel.Editable, Is.EqualTo(isEditable));
            Assert.That(viewModel.BetSettingProducts, Is.Empty);
        }
    }
}