namespace Fanex.Athena.Controllers.BetSettings.UnitTests
{
    using System.Collections.Generic;
    using Controllers.Base.UnitTests;
    using DataAccess.BetSettings.Interfaces;
    using Model.BetSettings.DTO;
    using Model.BetSettings.MetaModel;
    using Model.BetSettings.ViewModel;
    using Model.Core.DataDomain;
    using Model.Customer.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Customer.Interfaces;

    public class BetSettingBaseControllerHelperTest : BaseControllerHelperTest
    {
        protected IAuthorizationService AuthorizationService { get; private set; }

        protected ICoreBetSettingDataAccess CoreBetSettingDataAccess { get; private set; }

        protected IProductService ProductService { get; private set; }

        protected ICustomerService CustomerService { get; private set; }

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            AuthorizationService = Substitute.For<IAuthorizationService>();
            CoreBetSettingDataAccess = Substitute.For<ICoreBetSettingDataAccess>();
            ProductService = Substitute.For<IProductService>();
            CustomerService = Substitute.For<ICustomerService>();
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            AuthorizationService = null;
            CoreBetSettingDataAccess = null;
            ProductService = null;
            CustomerService = null;
        }

        protected void SetUpDefaultFakeBetSettingData()
        {
            StubGetValueAppSettings("FakeLicenseeList", DefaultLicenseeSession.LicenseeId.ToString());
            CustomerService.GetCustomerInfo(DefaultLicenseeSession.LicenseeId, Arg.Any<int>()).Returns(new CustomerInfo { SyncCasino = true });
            CoreBetSettingDataAccess.GetBetSettingProducts(DefaultLicenseeSession.LicenseeId).Returns(new List<BetSettingProduct>());
            AuthorizationService.HasPermissions(DefaultUserSession.FunctionIdList, PermissionKeys.CustomerInfoEditBetSetting).Returns(false);
        }

        protected void SetUpForBetSettingEditable(bool isEditable)
        {
            AuthorizationService.HasPermissions(DefaultUserSession.FunctionIdList, PermissionKeys.CustomerInfoEditBetSetting).Returns(isEditable);
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