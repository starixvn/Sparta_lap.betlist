namespace Fanex.Athena.Service.Customer.UnitTests
{
    using Casino;
    using Casino.Wrappers;
    using Logging;
    using Model.Customer.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CasinoCustomerServiceTest
    {
        private ICustomerApiWrapper _customerApiWrapper;

        private CasinoCustomerService _casinoCustomerService;

        [SetUp]
        public void SetUp()
        {
            var logger = Substitute.For<ILogger>();
            _customerApiWrapper = Substitute.For<ICustomerApiWrapper>();

            _casinoCustomerService = new CasinoCustomerService(logger, _customerApiWrapper);
        }

        [TearDown]
        public void TearDown()
        {
            _customerApiWrapper = null;
        }

        [Test]
        public void UpdateSuspend_WhenCall_CallApiCorrectly()
        {
            // Arrange
            var metaModel = CreateStatusMetaModel();

            // Action
            var result = _casinoCustomerService.UpdateSuspend(metaModel);

            // Assert
            _customerApiWrapper.Received().UpdateCustSuspend(
                            metaModel.CustId,
                            metaModel.IsOn,
                            metaModel.Ip,
                            metaModel.ModifyById,
                            metaModel.ModifyByUsername);
        }

        [Test]
        public void UpdateClosed_WhenCall_CallApiCorrectly()
        {
            // Arrange
            var metaModel = CreateStatusMetaModel();

            // Action
            var result = _casinoCustomerService.UpdateClosed(metaModel);

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