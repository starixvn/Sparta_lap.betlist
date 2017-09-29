namespace Fanex.Athena.Controllers.UnitTests
{
    using System;
    using System.Collections.Specialized;
    using System.Web.Mvc;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Model.Core.MetaModel.Account;
    using NSubstitute;
    using NSubstitute.Core;
    using NUnit.Framework;
    using Resources;

    public partial class AccountControllerTest
    {
        [TestCase("0")]
        [TestCase("111")]
        public void EditAccount_WhenGet_ReturnEditAccountView(string userId)
        {
            // Arrange
            SetUpUserRequestParameter(new NameValueCollection { { "UserId", userId } });

            StubGetUserInfo(x => new User { Username = "Superman" });

            // Action
            var viewResult = _controller.EditAccount() as ViewResult;
            var viewModel = viewResult.Model as EditAccountMetaModel;

            // Assert
            Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Account/EditAccount.cshtml"));
        }

        [Test]
        public void EditAccount_WhenGetWithUserIdIsSameWithSessionUserId_ReturnNullModel()
        {
            // Arrange
            SetUpUserRequestParameter(new NameValueCollection { { "UserId", DefaultUserSession.UserId.ToString() } });

            // Action
            var viewResult = _controller.EditAccount() as ViewResult;

            // Assert
            Assert.That(viewResult.Model, Is.Null);
        }

        [TestCase("0")]
        [TestCase("Invalid")]
        public void EditAccount_WhenGetWithInvalidUserId_ReturnNullModel(string userId)
        {
            // Arrange
            SetUpUserRequestParameter(new NameValueCollection { { "UserId", userId } });

            // Action
            var viewResult = _controller.EditAccount() as ViewResult;

            // Assert
            Assert.That(viewResult.Model, Is.Null);
        }

        [Test]
        public void EditAccount_WhenGetWithUserInfoIsNull_ReturnNullModel()
        {
            // Arrange
            SetUpUserRequestParameter();

            StubGetUserInfo(x => null);

            // Action
            var viewResult = _controller.EditAccount() as ViewResult;

            // Assert
            Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Account/EditAccount.cshtml"));
            Assert.That(viewResult.Model, Is.Null);
        }

        public void EditAccount_WhenGetWithValidUser_ReturnCorrectModel()
        {
            // Arrange
            SetUpUserRequestParameter(new NameValueCollection { { "UserId", DefaultUserSession.UserId.ToString() } });

            var user = new User { Username = "Superman" };
            StubGetUserInfo(x => user);

            // Action
            var viewResult = _controller.EditAccount() as ViewResult;
            var viewModel = viewResult.Model as EditAccountMetaModel;

            // Assert
            Assert.That(viewModel.UserName, Is.EqualTo(user.Username));
        }

        [Test]
        public void EditAccount_WhenGetWithUserInfoIsDifferentFromSession_ReturnNullModel()
        {
            // Arrange
            SetUpUserRequestParameter();

            StubGetUserInfo(x => new User { Username = "Superman", IsInternal = !SessionService.UserSession.IsInternal });

            // Action
            var viewResult = _controller.EditAccount() as ViewResult;

            // Assert
            Assert.That(viewResult.Model, Is.Null);
        }

        [Test]
        public void EditAccount_WhenPostWithInValidModel_ReturnFailResult()
        {
            // Arrange
            _controller.ModelState.AddModelError("err", "errmsg");

            // Action
            var result = _controller.EditAccount(new EditAccountMetaModel()) as JsonResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(resultInfo.Code, Is.EqualTo(-1));
                Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.InvalidRequestMsg));
            });
        }

        [Test]
        public void EditAccount_WhenPostWithUserInfoIsDifferentFromSession_ReturnSuccesResult()
        {
            // Arrange
            StubGetUserInfo(x => new User { IsInternal = !SessionService.UserSession.IsInternal });

            // Action
            var result = _controller.EditAccount(new EditAccountMetaModel()) as JsonResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(resultInfo.Code, Is.EqualTo(-1));
                Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.InvalidRequestMsg));
            });
        }

        [Test]
        public void EditAccount_WhenPost_UpdateInforSuccessful_ReturnSuccessResult()
        {
            // Arrange
            StubGetUserInfo(x => new User());
            _userService.UpdateInfo(Arg.Any<User>(), Arg.Any<int>()).Returns(new ResultInfo
            {
                Code = 0
            });

            // Action
            var result = _controller.EditAccount(new EditAccountMetaModel()) as JsonResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(resultInfo.Code, Is.EqualTo(0));
                Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.UpdateAccountSuccessMsg));
            });
        }

        [Test]
        public void EditAccount_WhenPost_UpdateInforFailed_ReturnFailResult()
        {
            // Arrange
            StubGetUserInfo(x => new User());
            _userService.UpdateInfo(Arg.Any<User>(), DefaultUserSession.UserId).Returns(new ResultInfo
            {
                Code = 1,
                Message = "Error"
            });

            // Action
            var result = _controller.EditAccount(new EditAccountMetaModel()) as JsonResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.That(resultInfo.IsSuccessful, Is.False);
        }

        private void StubGetUserInfo(Func<CallInfo, User> fakeReturn, params Func<CallInfo, User>[] fakeReturns)
        {
            _userService.GetUserInfo(Arg.Any<int>(), Arg.Any<int>()).Returns(fakeReturn, fakeReturns);
        }
    }
}