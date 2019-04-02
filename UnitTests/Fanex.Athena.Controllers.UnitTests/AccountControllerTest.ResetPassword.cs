namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Web.Mvc;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Model.Core.MetaModel.Account;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;

    public partial class AccountControllerTest
    {
        private const string ValidPassword = "1234aa";

        [Test]
        public void ResetPassword_WhenGet_ReturnResetPasswordView()
        {
            // Arrange
            SetUpUserRequestParameter();
            using (var controller = CreateController())
            {
                // Action
                var viewResult = controller.ResetPassword() as ViewResult;
                var viewModel = viewResult.Model as ResetPasswordMetaModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Account/ResetPassword.cshtml"));
                    Assert.That(viewModel.UserId, Is.EqualTo(111));
                });
            }
        }

        [Test]
        public void ResetPassword_WhenPostWithInvalidModel_ReturnFailResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                controller.ModelState.AddModelError("Error", "Invalid model");

                // Action
                var result = controller.ResetPassword(new ResetPasswordMetaModel()) as JsonResult;
                var resultInfo = result.Data as ResultInfo;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.Not.Zero);
                    Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.InvalidRequestMsg));
                });
            }
        }

        [Test]
        public void ResetPassword_WhenPostWithUserDoesNotExist_ReturnFailResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                StubGetUserInfo(x => null);

                // Action
                var result = controller.ResetPassword(new ResetPasswordMetaModel()) as JsonResult;
                var resultInfo = result.Data as ResultInfo;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.Not.Zero);
                    Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.InvalidRequestMsg));
                });
            }
        }

        [Test]
        public void ResetPassword_WhenPostWithRequestUserIsNotTheSameWithSessionUser_ReturnFailResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                StubGetUserInfo(x => new User { IsInternal = !DefaultUserSession.IsInternal });

                // Action
                var result = controller.ResetPassword(new ResetPasswordMetaModel()) as JsonResult;
                var resultInfo = result.Data as ResultInfo;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.Not.Zero);
                    Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.InvalidRequestMsg));
                });
            }
        }

        [Test]
        public void ResetPassword_WhenPostWithInValidPassword_ReturnFailResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                StubGetUserInfo(x => new User());

                // Action
                var result = controller.ResetPassword(new ResetPasswordMetaModel
                {
                    Password = string.Empty
                }) as JsonResult;
                var resultInfo = result.Data as ResultInfo;

                // Assert
                Assert.That(resultInfo.Code, Is.Not.Zero);
                Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.PasswordNoteMsg));
            }
        }

        [Test]
        public void ResetPassword_WhenPost_ResetPasswordSuccessful_ReturnSuccessResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                StubGetUserInfo(x => new User());

                StubResetPassword(new ResultInfo
                {
                    Code = 0
                });

                // Action
                var result = controller.ResetPassword(new ResetPasswordMetaModel
                {
                    Password = ValidPassword
                }) as JsonResult;
                var resultInfo = result.Data as ResultInfo;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.EqualTo(0));
                    Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.ResetPasswordSuccessMsg));
                });
            }
        }

        [Test]
        public void ResetPassword_WhenPost_ResetPasswordFailed_ReturnFailResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string ResultMessage = "Boss doesn't like this";
                StubGetUserInfo(x => new User());
                StubResetPassword(new ResultInfo
                {
                    Code = 1,
                    Message = ResultMessage
                });

                // Action
                var result = controller.ResetPassword(new ResetPasswordMetaModel
                {
                    Password = ValidPassword
                }) as JsonResult;
                var resultInfo = result.Data as ResultInfo;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.EqualTo(-3));
                    Assert.That(resultInfo.Message, Is.EqualTo(ResultMessage));
                });
            }
        }

        private void StubResetPassword(ResultInfo fakeResult)
        {
            _userService.ResetPassword(Arg.Any<User>(), Arg.Any<int>()).Returns(fakeResult);
        }
    }
}