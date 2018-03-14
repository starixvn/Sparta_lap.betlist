namespace Fanex.Athena.Controllers.Customer.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Model.Customer.ViewModel;
    using NSubstitute;
    using NUnit.Framework;

    public partial class CustomerControllerTest
    {
        [Test]
        public void GetCustomerList_UserNameIsNotEqualPlaceholderUsername_ReturnUserName()
        {
            using (var controller = CreateContoller())
            {
                // Arrange
                const string UserName = "UserName";

                AppSettings.FakeLicenseeList.Returns(Enumerable.Empty<int>());
                _productService.GetProductAccess(Arg.Any<int>(), Arg.Any<string>()).Returns(new ProductAccess(new List<Product>()));

                // Action
                var result = (CustomerListViewModel)(controller.CustomerList(UserName) as ViewResult).Model;

                // Assert
                Assert.That(result.Param.Username, Is.EqualTo(UserName));
            }
        }

        [Test]
        public void GetCustomerList_IsFakeLicensee_PlaceholderUsernameIsExternalId()
        {
            using (var controller = CreateContoller())
            {
                // Arrange
                var username = Resources.ViewResources.ExternalId;

                StubLicenseeSessionData(DefaultLicenseeSession);
                AppSettings.FakeLicenseeList.Returns(new[] { DefaultLicenseeSession.LicenseeId });
                _productService.GetProductAccess(Arg.Any<int>(), Arg.Any<string>()).Returns(new ProductAccess(new List<Product>()));

                // Action
                var result = (CustomerListViewModel)(controller.CustomerList(username) as ViewResult).Model;

                // Assert
                Assert.That(result.Param.Username, Is.Empty);
            }
        }

        [Test]
        public void GetCustomerList_IsFakeLicensee_PlaceholderUsernameIsExternalIdOrSystemId()
        {
            using (var controller = CreateContoller())
            {
                // Arrange
                var username = Resources.ViewResources.ExternalIdOrSystemId;

                AppSettings.FakeLicenseeList.Returns(Enumerable.Empty<int>());
                _productService.GetProductAccess(Arg.Any<int>(), Arg.Any<string>()).Returns(new ProductAccess(new List<Product>()));

                // Action
                var result = (CustomerListViewModel)(controller.CustomerList(username) as ViewResult).Model;

                // Assert
                Assert.That(result.Param.Username, Is.Empty);
            }
        }

        [Test]
        public void GetCustomerList_InitGridColumns_IsAllowBA_AddBAStatusColumn()
        {
            using (var controller = CreateContoller())
            {
                // Arrange
                const string BaStatus = "BAStatus";

                AppSettings.FakeLicenseeList.Returns(new[] { DefaultLicenseeSession.LicenseeId });
                _productService.GetProductAccess(Arg.Any<int>(), Arg.Any<string>()).Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.BA } }));

                // Action
                var result = (CustomerListViewModel)(controller.CustomerList() as ViewResult).Model;

                // Assert
                Assert.That(result.Param.GridColumns.Any(x => x.Name == BaStatus && x.Checked));
            }
        }

        [Test]
        public void GetCustomerList_InitGridColumns_AllowLicensees_AddStatusColumn()
        {
            using (var controller = CreateContoller())
            {
                // Arrange
                const string Status = "Status";

                StubLicenseeSessionData(DefaultLicenseeSession);
                AppSettings.FakeLicenseeList.Returns(new[] { DefaultLicenseeSession.LicenseeId });
                _productService.GetProductAccess(Arg.Any<int>(), Arg.Any<string>()).Returns(new ProductAccess(new List<Product>()));
                AppSettings.LicenseeOfferStatusFunction.Returns(new[] { DefaultLicenseeSession.LicenseeId });

                // Action
                var result = (CustomerListViewModel)(controller.CustomerList() as ViewResult).Model;

                // Assert
                Assert.That(result.Param.GridColumns.Any(x => x.Name == Status && x.Checked));
            }
        }

        [Test]
        public void GetCustomerList_InitGridColumns_AllowCustomerCommAndRealLicensee_AddCommissionColumn()
        {
            using (var controller = CreateContoller())
            {
                // Arrange
                const string Commission = "Commission";
                const string Group = "Group";

                AppSettings.FakeLicenseeList.Returns(Enumerable.Empty<int>());
                _productService.GetProductAccess(Arg.Any<int>(), Arg.Any<string>()).Returns(new ProductAccess(new List<Product>()));
                StubLicenseeSessionData(new LicenseeSession { IsAllowedCustomerComm = true });

                // Action
                var result = (CustomerListViewModel)(controller.CustomerList() as ViewResult).Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.Param.GridColumns.Any(x => x.Name == Commission && x.Checked));
                    Assert.That(result.Param.GridColumns.ElementAt(0).Name, Is.EqualTo(Group));
                });
            }
        }

        [Test]
        public void GetCustomerList_CreateCustomerViewModel_IsAllowedCustomerComm_AddDataForGroupComm()
        {
            using (var controller = CreateContoller())
            {
                // Arrange
                const int GroupCommId = 1;
                const int LicenseeId = 2;

                AppSettings.FakeLicenseeList.Returns(Enumerable.Empty<int>());
                _productService.GetProductAccess(Arg.Any<int>(), Arg.Any<string>()).Returns(new ProductAccess(new List<Product>()));
                StubLicenseeSessionData(new LicenseeSession { IsAllowedCustomerComm = true, LicenseeId = 2 });

                // Action
                var result = (CustomerListViewModel)(controller.CustomerList(groupId: GroupCommId) as ViewResult).Model;

                // Assert
                Assert.Multiple(() =>
                {
                    _groupComService.Received().GetAll(LicenseeId);
                    _groupComService.Received().GetById(GroupCommId);
                });
            }
        }

        [Test]
        public void GetCustomerList_SetValueForBaStatus_IsAllowedSpecialBA_DonotAddBAMessage()
        {
            using (var controller = CreateContoller())
            {
                // Arrange
                AppSettings.FakeLicenseeList.Returns(Enumerable.Empty<int>());
                _productService.GetProductAccess(Arg.Any<int>(), Arg.Any<string>()).Returns(new ProductAccess(new List<Product>()));
                StubLicenseeSessionData(DefaultLicenseeSession);
                SessionService.IsAllowedSpecialBA(DefaultUserSession).Returns(true);

                // Action
                var result = (CustomerListViewModel)(controller.CustomerList() as ViewResult).Model;

                // Assert
                Assert.That(result.CanUpdateBAMessage, Is.Empty);
                _licenseeDataAccess.DidNotReceive().IsAllowUpdateMemberBAStatus(Arg.Any<Request>(), Arg.Any<int>());
            }
        }

        [Test]
        public void GetCustomerList_SetValueForBaStatus_IsNotAllowedSpecialBA_AddBAMessage()
        {
            using (var controller = CreateContoller())
            {
                // Arrange
                const string BaMessage = "Message";
                AppSettings.FakeLicenseeList.Returns(Enumerable.Empty<int>());
                _productService.GetProductAccess(Arg.Any<int>(), Arg.Any<string>()).Returns(new ProductAccess(new List<Product>()));
                StubLicenseeSessionData(DefaultLicenseeSession);
                SessionService.IsAllowedSpecialBA(DefaultUserSession).Returns(false);
                _licenseeDataAccess.IsAllowUpdateMemberBAStatus(Arg.Any<Request>(), Arg.Any<int>()).Returns(new ResultInfo<bool>
                {
                    Message = BaMessage
                });

                // Action
                var result = (CustomerListViewModel)(controller.CustomerList() as ViewResult).Model;

                // Assert
                Assert.That(result.CanUpdateBAMessage, Is.EqualTo(BaMessage));
            }
        }

        [Test]
        public void GetCustomerList_SetValueForBaStatus_IsAllowedBAAndNotSpecialBA_AddBATimeCheck()
        {
            using (var controller = CreateContoller())
            {
                // Arrange
                const string BaMessage = "Message";
                AppSettings.FakeLicenseeList.Returns(Enumerable.Empty<int>());
                _productService.GetProductAccess(Arg.Any<int>(), Arg.Any<string>()).Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.BA } }));
                StubLicenseeSessionData(DefaultLicenseeSession);
                SessionService.IsAllowedSpecialBA(DefaultUserSession).Returns(false);
                _licenseeDataAccess.IsAllowUpdateMemberBAStatus(Arg.Any<Request>(), Arg.Any<int>()).Returns(new ResultInfo<bool>
                {
                    Message = BaMessage
                });

                // Action
                var result = (CustomerListViewModel)(controller.CustomerList() as ViewResult).Model;

                // Assert
                Assert.That(result.BATimeCheck, Is.EqualTo(BaMessage));
            }
        }
    }
}