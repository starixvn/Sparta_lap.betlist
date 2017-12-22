namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Collections.Generic;
    using System.Data;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Customer;
    using Excel.Base.Entities;
    using Excel.Customer;
    using Model.Core.DTO.Common;
    using Model.Customer.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Customer.Interfaces;
    using UnitTest.Common.Extensions;
    using Web.ResultExtension;

    [TestFixture]
    public partial class CustomerComGroupControllerTest : BaseControllerHelperTest
    {
        private IGroupComService _groupComService;
        private IProductService _productService;

        [SetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();
            SetUpDefaultFakeData();
            _groupComService = Substitute.For<IGroupComService>();
            _productService = Substitute.For<IProductService>();
        }

        [TearDown]
        public void TearDown()
        {
            AppSettings.ClearReceivedCalls();
            _groupComService.ClearReceivedCalls();
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        public void Index_WhenCall_AssignViewBagCorrectly(bool isEmptyProductList, bool expectedResult)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var productList = isEmptyProductList ? new List<Product>() : new List<Product>
                {
                    new Product { ProductCode = Product.NG },
                    new Product { ProductCode = Product.RB },
                    new Product { ProductCode = Product.LC },
                    new Product { ProductCode = Product.CS },
                    new Product { ProductCode = Product.AB },
                };
                _productService.GetProductAccess(
                    SessionService.LicenseeSession.LicenseeId,
                    SessionService.UserSession.Language).Returns(new ProductAccess(productList));

                // Action
                var result = controller.Index() as ViewResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(controller.ViewBag.IsAllowNG, Is.EqualTo(expectedResult));
                    Assert.That(controller.ViewBag.IsAllowRC, Is.EqualTo(expectedResult));
                    Assert.That(controller.ViewBag.IsAllowLC, Is.EqualTo(expectedResult));
                    Assert.That(controller.ViewBag.IsAllowCS, Is.EqualTo(expectedResult));
                    Assert.That(controller.ViewBag.IsAllowAB, Is.EqualTo(expectedResult));

                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Customer/CustomerComm/GroupCommList.cshtml"));
                });
            }
        }

        [TestCase("", false, "NO")]
        [TestCase("", true, "NO")]
        [TestCase("abc", true, "NO")]
        [TestCase("abc", false, "OK")]
        public void Validate_WhenCall_ReturnCorrectResult(string groupName, bool groupExist, string expectedResult)
        {
            using (var controller = CreateController())
            {
                // Arrange
                _groupComService.CheckExistingGroupByName(SessionService.LicenseeSession.LicenseeId, groupName)
                    .Returns(groupExist);

                // Action
                var result = controller.Validate(groupName);

                // Assert
                Assert.That(result, Is.EqualTo(expectedResult));
            }
        }

        [Test]
        public void Delete_WhenCall_CallDeleteGroup()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const int GroupId = 999;
                const bool IsRemovePlayerComm = true;
                const bool IsResetDefault = true;
                var licenseeId = SessionService.LicenseeSession.LicenseeId;
                var userSession = SessionService.UserSession;
                var productList = new List<Product> { new Product { ProductCode = Product.CS } };
                var productAccess = new ProductAccess(productList);
                _productService.GetProductAccess(licenseeId, userSession.Language).Returns(productAccess);
                _groupComService.DeleteGroup(null).ReturnsForAnyArgs(new ResultInfo());

                // Action
                var result = controller.Delete(GroupId, IsRemovePlayerComm, IsResetDefault) as JsonResult;
                var resultInfo = result.Data as ResultInfo;

                // Assert
                Assert.Multiple(() =>
                {
                    _groupComService.Received(1).DeleteGroup(Arg.Is<DeleteGroupMetaModel>(x =>
                        x.GroupId == GroupId &&
                        x.LicenseeId == licenseeId &&
                        x.UserId == userSession.UserId &&
                        x.UserName == userSession.UserName &&
                        x.Language == userSession.Language &&
                        x.IsRemoveCommSetting == IsRemovePlayerComm &&
                        x.IsAllowedCSComm == productAccess.IsAllowCS &&
                        x.IsResetDefault == IsResetDefault));
                    Assert.That(resultInfo.Code, Is.Zero);
                    Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.CommIsClear));
                });
            }
        }

        [TestCase(false, 0)]
        [TestCase(true, 999)]
        public void GetDefaultGroup_WhenCall_ReturnCorrectResult(bool isDefaultGroup, int expectedGroupId)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var group = new GroupCommMetaModel
                {
                    IsDefaultGroup = isDefaultGroup,
                    GroupCommId = 999
                };
                _groupComService.GetAll(SessionService.LicenseeSession.LicenseeId)
                    .Returns(new List<GroupCommMetaModel> { group });

                // Action
                var result = controller.GetDefaultGroup() as JsonResult;

                // Assert
                Assert.That(result.Data.GetValue("id"), Is.EqualTo(expectedGroupId));
            }
        }

        [Test]
        public void ExportExcel_WhenCall_PassCorrectParamsToCreateExcel()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var licenseeId = SessionService.LicenseeSession.LicenseeId;
                var productList = new List<Product>
                {
                    new Product { ProductCode = Product.SB },
                    new Product { ProductCode = Product.RB },
                    new Product { ProductCode = Product.NG },
                    new Product { ProductCode = Product.CS },
                    new Product { ProductCode = Product.LC },
                    new Product { ProductCode = Product.AB },
                };
                _productService.GetProductAccess(licenseeId, SessionService.UserSession.Language)
                    .Returns(new ProductAccess(productList));
                var dataTable = new DataTable();
                _groupComService.GetDataForExcel(licenseeId).Returns(dataTable);
                var excel = Substitute.ForPartsOf<CustomerCommGroupExcel>(string.Empty, dataTable, new ShowingOption());
                excel.When(x => x.Export()).DoNotCallBase();
                controller.CreateExcel(string.Empty, dataTable, new ShowingOption()).ReturnsForAnyArgs(excel);

                // Action
                var result = controller.ExportExcel() as ExcelResult;

                // Assert
                controller.Received(1).CreateExcel(
                    Arg.Any<string>(),
                    dataTable,
                    Arg.Is<ShowingOption>(x => x.SBVisible && x.RBVisible && x.NGVisible && x.CSVisible && x.LCVisible && x.ABVisible));
            }
        }

        private CustomerComGroupController CreateController()
        {
            var controller = Substitute.ForPartsOf<CustomerComGroupController>(
               LangService,
               SessionService,
               AppSettings,
               _groupComService,
               _productService);
            controller.ControllerContext = ControllerContext;
            return controller;
        }
    }
}