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
        public void ResetPassword_WhenCall_ReturnCorrectViewNameAndViewModel()
        {
            // Arrange
            var requestParams = new NameValueCollection
            {
                { "UserId", "111" }
            };
            _controller.ControllerContext.HttpContext.Request.Params.Returns(requestParams);

            // Action
            var viewResult = _controller.ResetPassword() as ViewResult;
            var viewModel = viewResult.Model as ResetPasswordMetaModel;

            // Assert
            Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Account/ResetPassword.cshtml"));
            Assert.That(viewModel.UserId, Is.EqualTo(111));
        }

        [Test]
        public void ResetPasswordPost_UserNotExists_ReturnFailResult()
        {
            // Arrange
            _userService.GetUserInfo(Arg.Any<int>(), Arg.Any<int>()).Returns(x => null);

            // Action
            var result = _controller.ResetPassword(new ResetPasswordMetaModel()) as JsonResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.That(resultInfo.Code, Is.EqualTo(-4));
            Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.InvalidRequestMsg));
        }

        [Test]
        public void ResetPasswordPost_InValidPassword_ReturnFailResult()
        {
            // Arrange
            _userService.GetUserInfo(Arg.Any<int>(), Arg.Any<int>()).Returns(new User());

            // Action
            var result = _controller.ResetPassword(new ResetPasswordMetaModel()) as JsonResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.That(resultInfo.Code, Is.EqualTo(-2));
            Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.PasswordNoteMsg));
        }

        [Test]
        public void ResetPasswordPost_ValidPassword_CallServiceAndReturnSuccess()
        {
            // Arrange
            _userService.GetUserInfo(Arg.Any<int>(), Arg.Any<int>()).Returns(new User());
            _userService.ResetPassword(Arg.Any<User>(), Arg.Any<int>()).Returns(new ResultInfo
            {
                Code = 0
            });

            // Action
            var result = _controller.ResetPassword(new ResetPasswordMetaModel
            {
                Password = "1234aa"
            }) as JsonResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            _userService.Received().ResetPassword(Arg.Any<User>(), SessionUserId);
            Assert.That(resultInfo.Code, Is.EqualTo(0));
            Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.ResetPasswordSuccessMsg));
        }

        [Test]
        public void ResetPasswordPost_CallServiceFail_ReturnFail()
        {
            // Arrange
            const string ResultMessage = "Boss doesn't like this";
            _userService.GetUserInfo(Arg.Any<int>(), Arg.Any<int>()).Returns(new User());
            _userService.ResetPassword(Arg.Any<User>(), Arg.Any<int>()).Returns(new ResultInfo
            {
                Code = 1,
                Message = ResultMessage
            });

            // Action
            var result = _controller.ResetPassword(new ResetPasswordMetaModel
            {
                Password = "1234aa"
            }) as JsonResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.That(resultInfo.Code, Is.EqualTo(-3));
            Assert.That(resultInfo.Message, Is.EqualTo(ResultMessage));
        }
    }
}