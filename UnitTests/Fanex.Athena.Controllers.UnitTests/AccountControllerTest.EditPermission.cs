namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
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
        public void EditPermission_WhenGetWithUserIdIsSameWithSessionUserId_RedirectToAccessDenied()
        {
            // Arrange
            SetUpUserRequestParameter(new NameValueCollection { { "UserId", DefaultUserSession.UserId.ToString() } });

            // Action
            var result = _controller.EditPermission() as RedirectResult;

            // Assert
            Assert.That(result.Url, Is.EqualTo(AccessDeniedUrl));
        }

        [TestCase("0")]
        [TestCase("Invalid")]
        public void EditPermission_WhenGetWithInvalidUserId_RedirectToAccessDenied(string userId)
        {
            // Arrange
            SetUpUserRequestParameter(new NameValueCollection { { "UserId", userId } });

            // Action
            var result = _controller.EditPermission() as RedirectResult;

            // Assert
            Assert.That(result.Url, Is.EqualTo(AccessDeniedUrl));
        }

        [Test]
        public void EditPermission_WhenGetWithDoesNotExist_RedirectToAccessDenied()
        {
            // Arrange
            SetUpUserRequestParameter();

            StubGetUserInfo(x => null);

            // Action
            var result = _controller.EditPermission() as RedirectResult;

            // Assert
            Assert.That(result.Url, Is.EqualTo(AccessDeniedUrl));
        }

        [Test]
        public void EditPermission_WhenGetWithValid_ReturnEditPermissionView()
        {
            // Arrange
            SetUpUserRequestParameter();

            StubGetUserInfo(x => new User { Username = "Superman" });

            // Action
            var viewResult = _controller.EditPermission() as ViewResult;
            var viewModel = viewResult.Model as EditPermissionMetaModel;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Account/EditPermission.cshtml"));
                Assert.That(viewModel.UserId, Is.EqualTo(111));
            });
        }

        [Test]
        public void EditPermission_WhenPost_ValidatePermissionRestrictionFailed_ReturnErrorResult()
        {
            // Action
            var result = _controller.EditPermission(new EditPermissionMetaModel
            {
                UserId = DefaultUserSession.UserId
            }) as JsonResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(resultInfo.Code, Is.EqualTo(-1));
                Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.FunctionDeniedMsg));
            });
        }

        [Test]
        public void EditPermission_WhenPostWithInValidUserId_ReturnFailResult()
        {
            // Action
            var result = _controller.EditPermission(new EditPermissionMetaModel
            {
                UserId = 0
            }) as JsonResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(resultInfo.Code, Is.EqualTo(-2));
                Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.InvalidRequestMsg));
            });
        }

        [Test]
        public void EditPermission_WhenPostWithEmptyFunctionIdList_ReturnFailResult()
        {
            // Action
            var result = _controller.EditPermission(new EditPermissionMetaModel
            {
                UserId = 111,
                FunctionIdList = string.Empty
            }) as JsonResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(resultInfo.Code, Is.EqualTo(-2));
                Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.InvalidRequestMsg));
            });
        }

        [Test]
        public void EditPermission_WhenPost_AssignUserFunctionsSuccessful_CallServiceWithCorrectFunctionIdList()
        {
            // Arrange
            LangService.GetCurrentLanguage().Returns(DefaultUserSession.Language);

            var model = new EditPermissionMetaModel
            {
                UserId = 111,
                FunctionIdList = "1,2,3,4,5"
            };

            var expectedFunctionIds = new List<int> { 1, 2, 3, 4, 5 };
            var actualFunctionIds = Enumerable.Empty<int>();

            _functionService.AssignUserFunctions(
                    DefaultLicenseeSession.LicenseeId,
                    model.UserId,
                    Arg.Do<IEnumerable<int>>(x => actualFunctionIds = x),
                    DefaultUserSession.UserId, 
                    Arg.Any<string>(),
                    Arg.Any<string>())
                .Returns(new ResultInfo());

            // Action
            var result = _controller.EditPermission(model) as JsonResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualFunctionIds.Count(), Is.EqualTo(expectedFunctionIds.Count()));
                Assert.That(actualFunctionIds.Except(expectedFunctionIds), Is.Empty);
            });
        }
    }
}