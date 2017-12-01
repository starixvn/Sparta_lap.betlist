namespace Fanex.Athena.Controllers.Customer.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Model.Customer.ViewModel.CustomerLog;
    using NSubstitute;
    using NUnit.Framework;

    public partial class CustomerLogControllerTest
    {
        [Test]
        public void CustomerSetting_WhenCall_ReturnCorrectView()
        {
            using (var controller = CreateController())
            {
                // Arrange
                _productService.GetProductAccess(Arg.Any<int>(), Arg.Any<string>()).Returns(new ProductAccess(new List<Product>()));
                var expectedViewName = "~/Views/Customer/CustomerLog/CustomerSettingLog/Main.cshtml";

                // Action
                var result = controller.CustomerSetting() as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo(expectedViewName));
            }
        }

        [Test]
        public void CustomerSetting_WhenCall_ReturnCorrectViewData()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var fakeLicensee = new LicenseeSession
                {
                    IsAllowedCustomerComm = true
                };
                StubLicenseeSessionData(fakeLicensee);

                var isAllowBA = true;
                _productService.GetProductAccess(Arg.Any<int>(), Arg.Any<string>()).Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.BA } }));

                // Action
                var result = (CustomerLogViewModel)(controller.CustomerSetting() as ViewResult).Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.Param.IsAllowBA, Is.EqualTo(isAllowBA));
                    Assert.That(result.Param.IsAllowCustComm, Is.EqualTo(fakeLicensee.IsAllowedCustomerComm));
                });
            }
        }
    }
}