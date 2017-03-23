namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Web.Mvc;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Model.Core.MetaModel.Account;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;

    public partial class AccountControllerTest
    {
        [Test]
        public void EditPermission_UserIdIsSameWithSessionUserId_RedirectToAccessDenied()
        {
            // Arrange
            var requestParams = new NameValueCollection { { "UserId", SessionUserId.ToString() } };
            _controller.ControllerContext.HttpContext.Request.Params.Returns(requestParams);

            // Action
            var result = _controller.EditPermission() as RedirectResult;

            // Assert
            Assert.That(result.Url, Is.EqualTo("~/Utility/AccessDenied"));
        }

        [Test]
        public void EditPermission_WhenCall_ReturnCorrectViewAndModel()
        {
            // Arrange
            var requestParams = new NameValueCollection { { "UserId", "111" } };
            _controller.ControllerContext.HttpContext.Request.Params.Returns(requestParams);

            _userService.GetUserInfo(Arg.Any<int>(), Arg.Any<int>()).Returns(new User
            {
                Username = "Superman"
            });

            // Action
            var viewResult = _controller.EditPermission() as ViewResult;
            var viewModel = viewResult.Model as EditPermissionMetaModel;

            // Assert
            Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Account/EditPermission.cshtml"));
            Assert.That(viewModel.UserId, Is.EqualTo(111));
        }

        [Test]
        public void EditPermissionPost_UserIdIsSameWithSessionUserId_ReturnErrorResult()
        {
            // Action
            var result = _controller.EditPermission(new EditPermissionMetaModel
            {
                UserId = SessionUserId
            }) as JsonResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.That(resultInfo.Code, Is.EqualTo(-1));
            Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.FunctionDeniedMsg));
        }

        [Test]
        public void EditPermissionPost_InValidUserId_ReturnFailResult()
        {
            // Action
            var result = _controller.EditPermission(new EditPermissionMetaModel
            {
                UserId = 0
            }) as JsonResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.That(resultInfo.Code, Is.EqualTo(-2));
            Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.InvalidRequestMsg));
        }

        [Test]
        public void EditPermissionPost_ValidUserInfo_CallServiceAndReturnSuccesResult()
        {
            // Arrange
            _functionService.AssignUserFunctions(SessionLicenseeId, 111, Arg.Any<IList<int>>(), SessionUserId, Arg.Any<string>(), Arg.Any<string>())
                .Returns(new ResultInfo
                {
                    Code = 0
                });

            // Action
            var result = _controller.EditPermission(new EditPermissionMetaModel
            {
                UserId = 111,
                FunctionIdList = "1,2,3,4,5"
            }) as JsonResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            _functionService.Received().AssignUserFunctions(SessionLicenseeId, 111, Arg.Any<IList<int>>(), SessionUserId, Arg.Any<string>(), Arg.Any<string>());
            Assert.That(resultInfo.Code, Is.EqualTo(0));
        }
    }
}