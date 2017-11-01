namespace Fanex.Athena.Service.Customer.UnitTests
{
    using Casino;
    using Casino.Wrappers;
    using Model.Customer.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CasinoSettingServiceTest
    {
        private ICustomerApiWrapper _customerApiWrapper;

        private IBasicInfoApiWrapper _basicInfoApiWrapper;

        private CasinoSettingService _casinoSettingService;

        [SetUp]
        public void SetUp()
        {
            _customerApiWrapper = Substitute.For<ICustomerApiWrapper>();

            _basicInfoApiWrapper = Substitute.For<IBasicInfoApiWrapper>();
            _casinoSettingService = new CasinoSettingService(_customerApiWrapper, _basicInfoApiWrapper);
        }

        [TearDown]
        public void TearDown()
        {
            _customerApiWrapper = null;
            _basicInfoApiWrapper = null;
        }

        [Test]
        public void UpdateCustSuspend_WhenCall_CallApiCorrectly()
        {
            // Arrange
            var metaModel = CreateStatusMetaModel();

            // Action
            var result = _casinoSettingService.UpdateCustSuspend(metaModel);

            // Assert
            _customerApiWrapper.Received().UpdateCustSuspend(
                            metaModel.CustId,
                            metaModel.IsOn,
                            metaModel.Ip,
                            metaModel.ModifyById,
                            metaModel.ModifyByUsername);
        }

        [Test]
        public void UpdateCustClosed_WhenCall_CallApiCorrectly()
        {
            // Arrange
            var metaModel = CreateStatusMetaModel();

            // Action
            var result = _casinoSettingService.UpdateCustClosed(metaModel);

            // Assert
            _customerApiWrapper.Received().UpdateCustClosed(
                            metaModel.CustId,
                            metaModel.IsOn,
                            metaModel.Ip,
                            metaModel.ModifyById,
                            metaModel.ModifyByUsername);
        }

        private static CustomerStatusMetaModel CreateStatusMetaModel()
        {
            return new CustomerStatusMetaModel
            {
                CustId = 1,
                IsOn = true,
                Ip = "123",
                ModifyById = 2,
                ModifyByUsername = "donald trumpt"
            };
        }
    }
}