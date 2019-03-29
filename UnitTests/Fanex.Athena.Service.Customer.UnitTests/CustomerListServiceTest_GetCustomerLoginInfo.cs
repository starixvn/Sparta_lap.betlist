namespace Fanex.Athena.Service.Customer.UnitTests
{
    using System.Threading.Tasks;
    using Casino.Interfaces;
    using Core.Common.Interfaces;
    using DataAccess.Customer.Interfaces;
    using Extension.Utility;
    using Model.Customer.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using StatelessCore.Interfaces;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public partial class CustomerListServiceTest
    {
        private ICustomerDataAccess _customerDataAccess;

        private ICustomerListDataAccess _customerListDataAccess;

        private IDormantCustomerListDataAccess _dormantCustomerListDataAccess;

        private ICasinoCustomerService _casinoCustomerService;

        private IAuthorizationService _authorizationService;

        private IProductService _productService;

        private IAppSettings _appSettings;

        private CustomerListService _customerListService;

        [SetUp]
        public void SetUp()
        {
            _customerDataAccess = Substitute.For<ICustomerDataAccess>();
            _customerListDataAccess = Substitute.For<ICustomerListDataAccess>();
            _dormantCustomerListDataAccess = Substitute.For<IDormantCustomerListDataAccess>();
            _casinoCustomerService = Substitute.For<ICasinoCustomerService>();
            _authorizationService = Substitute.For<IAuthorizationService>();
            _productService = Substitute.For<IProductService>();
            _appSettings = Substitute.For<IAppSettings>();

            _customerListService = new CustomerListService(
                _customerDataAccess,
                _customerListDataAccess,
                _dormantCustomerListDataAccess,
                _casinoCustomerService,
                _authorizationService,
                _productService,
                _appSettings);
        }

        [TearDown]
        public void TearDown()
        {
            _customerListDataAccess = null;
            _casinoCustomerService = null;
            _authorizationService = null;
            _productService = null;
            _appSettings = null;
            _customerListService = null;
        }

        [Test]
        public async Task GetCustomerLoginInfo_WhenCallWithCorrectPermission_ReturnCorrectData()
        {
            // Arrange
            const string LicenseeName = "Name";
            var licenseeId = FakeHelper.Create<int>();
            var custId = FakeHelper.Create<int>();
            var expectedResult = FakeHelper.Create<LogInfo>();

            _authorizationService.HasPermissionOnData(licenseeId, custId, 0).Returns(true);
            _customerListDataAccess.GetCustomerLoginInfo(custId, licenseeId, LicenseeName).Returns(expectedResult);

            // Action
            var result = await _customerListService.GetCustomerLoginInfo(custId, licenseeId, LicenseeName);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task GetCustomerLoginInfo_HaveNoPermission_DataAccessIsNotCalled()
        {
            // Arrange
            const string LicenseeName = "Name";
            var licenseeId = FakeHelper.Create<int>();
            var custId = FakeHelper.Create<int>();

            _authorizationService.HasPermissionOnData(licenseeId, custId, 0).Returns(false);

            // Action
            var result = await _customerListService.GetCustomerLoginInfo(custId, licenseeId, LicenseeName);

            // Assert
            Assert.Multiple(() =>
            {
                _customerListDataAccess.DidNotReceive().GetCustomerLoginInfo(custId, licenseeId, LicenseeName);
                Assert.That(result, Is.Not.Null);
            });
        }
    }
}