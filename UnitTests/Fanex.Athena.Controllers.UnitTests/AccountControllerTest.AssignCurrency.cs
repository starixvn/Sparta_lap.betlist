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
        public void AssignCurrency_NotEnableAssignCurrency_ReturnEmptyResult()
        {
            // Arrange
            AppSettings.EnableAssignCurrency.Returns(false);

            // Action
            var result = _controller.AssignCurrency(0) as EmptyResult;

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void AssignCurrency_InvalidUserId_RedirectToAccessDenied()
        {
            // Arrange
            AppSettings.EnableAssignCurrency.Returns(true);

            // Action
            var result = _controller.AssignCurrency(0) as RedirectResult;

            // Assert
            Assert.That(result.Url, Is.EqualTo("~/Utility/AccessDenied"));
        }

        [Test]
        public void AssignCurrency_WhenCall_ReturnCorrectView()
        {
            // Arrange
            const int UserId = 111;
            AppSettings.EnableAssignCurrency.Returns(true);
            _currencyService.GetCurrenciesByUser(SessionLicenseeId, UserId).Returns(new List<CurrencyUser>
            {
                new CurrencyUser()
            });

            // Action
            var viewResult = _controller.AssignCurrency(UserId) as ViewResult;

            // Assert
            Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Account/AssignCurrency.cshtml"));
            Assert.That(viewResult.ViewBag.CurrencyList.Count, Is.EqualTo(1));
        }

        [Test]
        public void AssignCurrencyPost_InValidUserId_ReturnFailResult()
        {
            // Action
            var result = _controller.AssignCurrency(new AssignCurrencyMetaModel
            {
                UserId = 0
            }) as JsonResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.That(resultInfo.Code, Is.EqualTo(-2));
            Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.InvalidRequestMsg));
        }

        [Test]
        public void AssignCurrencyPost_NotEnableAssignCurrency_ReturnFailResult()
        {
            // Arrange
            AppSettings.EnableAssignCurrency.Returns(false);

            // Action
            var result = _controller.AssignCurrency(new AssignCurrencyMetaModel
            {
                UserId = 111,
                CurrencyList = "1,2,3"
            }) as JsonResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.That(resultInfo.Code, Is.EqualTo(-1));
            Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.FunctionDeniedMsg));
        }

        [Test]
        public void AssignCurrencyPost_ValidUserInfo_CallServiceAndBuildCorrectCurrencyList()
        {
            // Arrange
            AppSettings.EnableAssignCurrency.Returns(true);
            _currencyService.GetCurrenciesByUser(SessionLicenseeId, SessionUserId)
                .Returns(new List<CurrencyUser>
                {
                    new CurrencyUser
                    {
                        CurrencyId = 1
                    }
                });
            _currencyService.AssignCurrenciesByUser(Arg.Any<AssignCurrencyMetaModel>())
                .Returns(new ResultInfo());
            var model = new AssignCurrencyMetaModel
            {
                UserId = 111,
                CurrencyList = "1,2,3,4,5"
            };

            // Action
            var result = _controller.AssignCurrency(model) as JsonResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            _currencyService.Received().AssignCurrenciesByUser(Arg.Any<AssignCurrencyMetaModel>());
            Assert.That(model.CurrencyList, Is.EqualTo("<currencies><currency currencyid=\"1\" status=\"1\"></currency></currencies>"));
            Assert.That(resultInfo.Code, Is.EqualTo(0));
        }
    }
}