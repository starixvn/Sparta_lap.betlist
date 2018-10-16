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
        public void UpdateGet_WhenCall_ReturnCorrectResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const int GroupId = 999;
                const decimal SampleComm = 0.0365m;
                var metaModel = new GroupCommMetaModel
                {
                    SBComm = SampleComm,
                    ParlayComm = SampleComm,
                    NGComm = SampleComm,
                    CSComm = SampleComm
                };
                _groupComService.GetById(GroupId).Returns(metaModel);
                _productService.GetProductAccess(0, string.Empty).ReturnsForAnyArgs(new ProductAccess(new List<Product>()));

                // Action
                var result = controller.Update(GroupId) as ViewResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Customer/CustomerComm/UpdateGroup.cshtml"));
                    Assert.That(metaModel.SBComm1, Is.EqualTo(3));
                    Assert.That(metaModel.SBComm2, Is.EqualTo(6));
                    Assert.That(metaModel.SBComm3, Is.EqualTo(5));

                    Assert.That(metaModel.ParlayComm1 == 3 && metaModel.ParlayComm2 == 6 && metaModel.ParlayComm3 == 5, Is.True);
                    Assert.That(metaModel.NGComm1 == 3 && metaModel.NGComm2 == 6 && metaModel.NGComm3 == 5, Is.True);
                    Assert.That(metaModel.CSComm1 == 3 && metaModel.CSComm2 == 6 && metaModel.CSComm3 == 5, Is.True);
                });
            }
        }

        [Test]
        public void UpdatePost_InvalidModelState_ReturnCorrectResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                controller.ModelState.AddModelError("abc", "xxx");

                // Action
                var result = controller.Update(new GroupCommMetaModel()) as ViewResult;
                var resultInfo = controller.ViewBag.Result as ResultInfo;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Customer/CustomerComm/UpdateGroup.cshtml"));
                    Assert.That(resultInfo.IsSuccessful, Is.False);
                    Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.InvalidParams));
                });
            }
        }

        [Test]
        public void UpdatePost_GroupNameIsNone_ReturnCorrectErrorResult()
        {
            using (var controller = CreateController())
            {
                // Action
                controller.Update(new GroupCommMetaModel
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
        public void UpdatePost_AllValid_CallUpdateGroup()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new GroupCommMetaModel
                {
                    GroupCommId = 999,
                    GroupName = "Belle"
                };
                _groupComService.GetById(metaModel.GroupCommId).Returns(new GroupCommMetaModel());
                _groupComService.UpdateGroup(metaModel, Arg.Any<Request>(), SessionService.UserSession)
                    .Returns(new ResultInfo());

                // Action
                controller.Update(metaModel);
                var resultInfo = controller.ViewBag.Result as ResultInfo;

                // Assert
                Assert.Multiple(() =>
                {
                    _groupComService.Received(1).UpdateGroup(metaModel, Arg.Any<Request>(), SessionService.UserSession);
                    Assert.That(resultInfo.Code, Is.Zero);
                    Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.MessageSuccess));
                });
            }
        }

        [Test]
        public void UpdatePost_GroupNotExists_ReturnSuccessResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new GroupCommMetaModel
                {
                    GroupCommId = 999,
                    GroupName = string.Empty
                };
                _groupComService.GetById(metaModel.GroupCommId).Returns(x => null);

                // Action
                controller.Update(metaModel);
                var resultInfo = controller.ViewBag.Result as ResultInfo;

                // Assert
                Assert.Multiple(() =>
                {
                    _groupComService.DidNotReceive().UpdateGroup(metaModel, Arg.Any<Request>(), SessionService.UserSession);
                    Assert.That(resultInfo.Code, Is.Zero);
                    Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.MessageSuccess));
                });
            }
        }

        [Test]
        public void UpdatePost_InvalidComm_ReturnErrorResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new GroupCommMetaModel
                {
                    GroupCommId = 999,
                    GroupName = "abc",
                    SBComm = 9999
                };

                // Action
                controller.Update(metaModel);
                var resultInfo = controller.ViewBag.Result as ResultInfo;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.EqualTo(2));
                    Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.MessageInvalidCommValue));
                });
            }
        }
    }
}