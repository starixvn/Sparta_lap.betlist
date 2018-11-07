namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Model.Core.DTO.Common;
    using Model.Customer.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;

    [TestFixture]
    public partial class CustomerComGroupControllerTest : BaseControllerHelperTest
    {
        [Test]
        public void CreateGet_Whencall_ReturnCorrectResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const int DefaultGroupId = 999;
                var licenseeId = SessionService.LicenseeSession.LicenseeId;
                var productList = new List<Product>
                {
                    new Product { ProductCode = Product.NG },
                    new Product { ProductCode = Product.CS },
                    new Product { ProductCode = Product.AB },
                };
                _productService.GetProductAccess(
                    licenseeId,
                    SessionService.UserSession.Language).Returns(new ProductAccess(productList));
                _groupComService.GetAll(licenseeId).Returns(new List<GroupCommMetaModel>
                    {
                        new GroupCommMetaModel
                        {
                            IsDefaultGroup = true,
                            GroupCommId = DefaultGroupId
                        }
                    });

                // Action
                var result = controller.Create() as ViewResult;
                var model = result.Model as GroupCommMetaModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/Customer/CustomerComm/CreateNewGroup.cshtml"));

                    Assert.That(model.LicenseeId, Is.EqualTo(licenseeId));
                    Assert.That(model.IsUpdateNG, Is.True);
                    Assert.That(model.IsUpdateCS, Is.True);
                    Assert.That(model.DefaultGroupId, Is.EqualTo(DefaultGroupId));
                    Assert.That(model.OriginalGroupName, Is.Null);
                });
            }
        }

        [Test]
        public void CreatePost_InvalidModelState_ReturnCorrectErrorResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                controller.ModelState.AddModelError("abc", "xxx");

                // Action
                var result = controller.Create(new GroupCommMetaModel()) as ViewResult;
                var resultInfo = controller.ViewBag.Result as ResultInfo;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/Customer/CustomerComm/CreateNewGroup.cshtml"));
                    Assert.That(resultInfo.Code, Is.Not.Zero);
                    Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.InvalidParams));
                });
            }
        }

        [Test]
        public void CreatePost_GroupNameIsNone_ReturnCorrectErrorResult()
        {
            using (var controller = CreateController())
            {
                // Action
                controller.Create(new GroupCommMetaModel
                {
                    GroupName = "None  "
                });
                var resultInfo = controller.ViewBag.Result as ResultInfo;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.Not.Zero);
                    Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.ChooseGroupNameMsg));
                });
            }
        }

        [Test]
        public void CreatePost_ValidCommissions_CallInsertGroup()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new GroupCommMetaModel
                {
                    IsDefaultGroup = true,
                    GroupCommId = 999,
                    GroupName = "Belle"
                };
                _groupComService.InsertGroup(metaModel, Arg.Any<Request>(), SessionService.UserSession.UserId)
                    .Returns(new ResultInfo());

                // Action
                controller.Create(metaModel);
                var resultInfo = controller.ViewBag.Result as ResultInfo;

                // Assert
                Assert.Multiple(() =>
                {
                    _groupComService.Received(1).InsertGroup(metaModel, Arg.Any<Request>(), SessionService.UserSession.UserId);
                    Assert.That(resultInfo.Code, Is.Zero);
                    Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.MessageSuccess));
                    Assert.That(metaModel.IsResetDefault, Is.False);
                    Assert.That(metaModel.DefaultGroupId, Is.EqualTo(metaModel.GroupCommId));
                    Assert.That(metaModel.OriginalGroupName, Is.EqualTo(metaModel.GroupName));
                });
            }
        }

        [Test]
        public void CreatePost_InvalidCommissions_ReturnCorrectError()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new GroupCommMetaModel
                {
                    SBComm = 100,
                    GroupName = "foo"
                };

                // Action
                controller.Create(metaModel);
                var resultInfo = controller.ViewBag.Result as ResultInfo;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.Not.Zero);
                    Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.MessageInvalidCommValue));
                });
            }
        }
    }
}