namespace Fanex.Athena.Service.Customer.UnitTests
{
    using Casino.Interfaces;
    using Core.Common.Interfaces;
    using Core.Users.Interfaces;
    using DataAccess.Customer.Interfaces;
    using Extension.Utility;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerServiceTest
    {
        private ICustomerDataAccess _customerDataAccess;

        private ICasinoSetting _casinoSetting;

        private IAuthorizationService _authorizationService;

        private IProductService _productService;

        private ISessionService _sessionService;

        private IRNGCasinoService _rngCasinoService;

        private IAppSettings _appSettings;

        private CustomerService _customerService;

        [SetUp]
        public void SetUp()
        {
            _customerDataAccess = Substitute.For<ICustomerDataAccess>();
            _casinoSetting = Substitute.For<ICasinoSetting>();
            _authorizationService = Substitute.For<IAuthorizationService>();
            _productService = Substitute.For<IProductService>();
            _sessionService = Substitute.For<ISessionService>();
            _rngCasinoService = Substitute.For<IRNGCasinoService>();
            _appSettings = Substitute.For<IAppSettings>();

            _customerService = new CustomerService(
                _customerDataAccess,
                _casinoSetting,
                _authorizationService,
                _productService,
                _sessionService,
                _rngCasinoService,
                _appSettings);
        }

        [TearDown]
        public void TearDown()
        {
            _customerDataAccess = null;
            _casinoSetting = null;
            _authorizationService = null;
            _productService = null;
            _sessionService = null;
            _rngCasinoService = null;
            _appSettings = null;
            _customerService = null;
        }
    }
}