namespace Fanex.Athena.Controllers.UnitTests
{
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
        public void EditAccount_UserIdIsSameWithSessionUserId_ReturnNullModel()
        {
            // Arrange
            var requestParams = new NameValueCollection { { "UserId", SessionUserId.ToString() } };
            _controller.ControllerContext.HttpContext.Request.Params.Returns(requestParams);

            // Action
            var viewResult = _controller.EditAccount() as ViewResult;

            // Assert
            Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Account/EditAccount.cshtml"));
            Assert.That(viewResult.Model, Is.Null);
        }

        [TestCase("0")]
        [TestCase("Invalid")]
        public void EditAccount_InvalidUserId_ReturnNullModel(string userId)
        {
            // Arrange
            var requestParams = new NameValueCollection { { "UserId", userId } };
            _controller.ControllerContext.HttpContext.Request.Params.Returns(requestParams);

            // Action
            var viewResult = _controller.EditAccount() as ViewResult;

            // Assert
            Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Account/EditAccount.cshtml"));
            Assert.That(viewResult.Model, Is.Null);
        }

        [Test]
        public void EditAccount_WhenCall_ReturnCorrectViewAndModel()
        {
            // Arrange
            var requestParams = new NameValueCollection { { "UserId", "111" } };
            _controller.ControllerContext.HttpContext.Request.Params.Returns(requestParams);

            _userService.GetUserInfo(Arg.Any<int>(), Arg.Any<int>()).Returns(new User
            {
                Username = "Superman"
            });

            // Action
            var viewResult = _controller.EditAccount() as ViewResult;
            var viewModel = viewResult.Model as EditAccountMetaModel;

            // Assert
            Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Account/EditAccount.cshtml"));
            Assert.That(viewModel.UserName, Is.EqualTo("Superman"));
        }

        [Test]
        public void EditAccount_UserInfoIsNull_ReturnNullModel()
        {
            // Arrange
            var requestParams = new NameValueCollection { { "UserId", "111" } };
            _controller.ControllerContext.HttpContext.Request.Params.Returns(requestParams);

            _userService.GetUserInfo(Arg.Any<int>(), Arg.Any<int>()).Returns(x => null);

            // Action
            var viewResult = _controller.EditAccount() as ViewResult;

            // Assert
            Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Account/EditAccount.cshtml"));
            Assert.That(viewResult.Model, Is.Null);
        }

        [Test]
        public void EditAccount_UserInfoIsDifferentFromSession_ReturnNullModel()
        {
            // Arrange
            var requestParams = new NameValueCollection { { "UserId", "111" } };
            _controller.ControllerContext.HttpContext.Request.Params.Returns(requestParams);

            _userService.GetUserInfo(Arg.Any<int>(), Arg.Any<int>()).Returns(new User
            {
                Username = "Superman",
                IsInternal = true
            });

            // Action
            var viewResult = _controller.EditAccount() as ViewResult;

            // Assert
            Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Account/EditAccount.cshtml"));
            Assert.That(viewResult.Model, Is.Null);
        }

        [Test]
        public void EditAccountPost_InValidModel_ReturnFailResult()
        {
            // Arrange
            _controller.ModelState.AddModelError("err", "errmsg");

            // Action
            var result = _controller.EditAccount(new EditAccountMetaModel()) as JsonResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.That(resultInfo.Code, Is.EqualTo(-1));
            Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.InvalidRequestMsg));
        }

        [Test]
        public void EditAccountPost_InValidUserInfo_ReturnFailResult()
        {
            // Arrange
            _userService.GetUserInfo(Arg.Any<int>(), Arg.Any<int>()).Returns(x => null);

            // Action
            var result = _controller.EditAccount(new EditAccountMetaModel()) as JsonResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.That(resultInfo.Code, Is.EqualTo(-1));
            Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.InvalidRequestMsg));
        }

        [Test]
        public void EditAccountPost_ValidUserInfo_ReturnSuccesResult()
        {
            // Arrange
            _userService.GetUserInfo(Arg.Any<int>(), Arg.Any<int>()).Returns(new User());
            _userService.UpdateInfo(Arg.Any<User>(), Arg.Any<int>()).Returns(new ResultInfo
            {
                Code = 0
            });

            // Action
            var result = _controller.EditAccount(new EditAccountMetaModel()) as JsonResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            _userService.Received().UpdateInfo(Arg.Any<User>(), SessionUserId);
            Assert.That(resultInfo.Code, Is.EqualTo(0));
            Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.UpdateAccountSuccessMsg));
        }

        [Test]
        public void EditAccountPost_EditFailed_ReturnFailedResult()
        {
            // Arrange
            _userService.GetUserInfo(Arg.Any<int>(), Arg.Any<int>()).Returns(new User());
            _userService.UpdateInfo(Arg.Any<User>(), Arg.Any<int>()).Returns(new ResultInfo
            {
                Code = 1
            });

            // Action
            var result = _controller.EditAccount(new EditAccountMetaModel()) as JsonResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            _userService.Received().UpdateInfo(Arg.Any<User>(), SessionUserId);
            Assert.That(resultInfo.IsSuccessful, Is.False);
        }
    }
}