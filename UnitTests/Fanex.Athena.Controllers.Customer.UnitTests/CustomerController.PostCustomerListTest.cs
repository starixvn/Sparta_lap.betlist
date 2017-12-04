namespace Fanex.Athena.Controllers.Customer.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Model.Core.ResultModel;
    using Model.Customer.DTO;
    using Model.Customer.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public partial class CustomerControllerTest
    {
        [Test]
        public void PostCustomerList_UserNameIsNotEqualPlaceholderUsername_ReturnUserName()
        {
            using (var controller = CreateContoller())
            {
                // Arrange
                const string UserName = "UserName";
                var metaModel = new CustomerListMetaModel
                {
                    Username = UserName
                };

                AppSettings.FakeLicenseeList.Returns(Enumerable.Empty<int>());
                _customerListService.Search(metaModel).Returns(new ReportResultModel<CustomerDTO>
                {
                    DataList = new List<CustomerDTO>(),
                    TotalRows = 0
                });

                // Action
                var result = controller.CustomerList(metaModel);

                // Assert
                Assert.That(metaModel.Username, Is.EqualTo(UserName));
            }
        }

        [Test]
        public void PostCustomerList_IsFakeLicensee_PlaceholderUsernameIsExternalId()
        {
            using (var controller = CreateContoller())
            {
                // Arrange
                var userName = Resources.ViewResources.ExternalId;
                var metaModel = new CustomerListMetaModel
                {
                    Username = userName
                };

                StubLicenseeSessionData(DefaultLicenseeSession);
                AppSettings.FakeLicenseeList.Returns(new int[] { DefaultLicenseeSession.LicenseeId });
                _customerListService.Search(metaModel).Returns(new ReportResultModel<CustomerDTO>
                {
                    DataList = new List<CustomerDTO>(),
                    TotalRows = 0
                });

                // Action
                var result = controller.CustomerList(metaModel);

                // Assert
                Assert.That(metaModel.Username, Is.Empty);
            }
        }

        [Test]
        public void PostCustomerList_IsFakeLicensee_PlaceholderUsernameIsExternalIdOrSystemId()
        {
            using (var controller = CreateContoller())
            {
                // Arrange
                var userName = Resources.ViewResources.ExternalIdOrSystemId;
                var metaModel = new CustomerListMetaModel
                {
                    Username = userName
                };

                AppSettings.FakeLicenseeList.Returns(Enumerable.Empty<int>());
                _customerListService.Search(metaModel).Returns(new ReportResultModel<CustomerDTO>
                {
                    DataList = new List<CustomerDTO>(),
                    TotalRows = 0
                });

                // Action
                var result = controller.CustomerList(metaModel);

                // Assert
                Assert.That(metaModel.Username, Is.Empty);
            }
        }

        [Test]
        public void PostCustomerList_WhenCall_CallServiceToGetData()
        {
            using (var controller = CreateContoller())
            {
                // Arrange
                var metaModel = new CustomerListMetaModel();

                AppSettings.FakeLicenseeList.Returns(Enumerable.Empty<int>());
                _customerListService.Search(metaModel).Returns(new ReportResultModel<CustomerDTO>
                {
                    DataList = new List<CustomerDTO>(),
                    TotalRows = 0
                });

                // Action
                var result = controller.CustomerList(metaModel);

                // Assert
                _customerListService.Received().Search(metaModel);
            }
        }
    }
}