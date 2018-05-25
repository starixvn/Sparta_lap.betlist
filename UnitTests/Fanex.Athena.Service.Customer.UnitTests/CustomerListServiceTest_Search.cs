namespace Fanex.Athena.Service.Customer.UnitTests
{
    using Casino.Interfaces;
    using Core.Common.Interfaces;
    using Core.Users.Interfaces;
    using DataAccess.Customer.Interfaces;
    using Extension.Utility;
    using Model.Core.ResultModel;
    using Model.Customer.DTO;
    using Model.Customer.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public partial class CustomerListServiceTest
    {
        private ICustomerDataAccess _customerDataAccess;

        private ICustomerListDataAccess _customerListDataAccess;

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
            _casinoCustomerService = Substitute.For<ICasinoCustomerService>();
            _authorizationService = Substitute.For<IAuthorizationService>();
            _productService = Substitute.For<IProductService>();
            _appSettings = Substitute.For<IAppSettings>();

            _customerListService = new CustomerListService(
                _customerDataAccess,
                _customerListDataAccess,
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
        public void SearchCustomers_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var userId = FakeHelper.Create<int>();
            var userName = FakeHelper.Create<string>();
            var agentListXml = FakeHelper.Create<string>();
            var expectedResult = new string[1] { FakeHelper.Create<string>() };
            _customerListDataAccess.SearchCustomers(userId, userName, agentListXml).Returns(expectedResult);

            // Action
            var result = _customerListService.SearchCustomers(userId, userName, agentListXml);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Search_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var metaModel = FakeHelper.Create<CustomerListMetaModel>();

            var expectedResult = new ReportResultModel<CustomerDTO>
            {
                DataList = FakeHelper.CreateMany<CustomerDTO>(10),
                TotalRows = FakeHelper.Create<int>()
            };
            _customerListDataAccess.Search(metaModel).Returns(expectedResult);

            // Action
            var result = _customerListService.Search(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList, Is.EqualTo(expectedResult.DataList));
                Assert.That(result.TotalRows, Is.EqualTo(expectedResult.TotalRows));
            });
        }
    }
}