namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Model.Core.MetaModel.Account;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;

    public partial class AccountControllerTest
    {
        [Test]
        public void AssignCurrency_WhenGetWithoutEnableAssignCurrency_ReturnEmptyResult()
        {
            // Arrange
            AppSettings.EnableAssignCurrency.Returns(false);

            // Action
            var result = _controller.AssignCurrency(0) as EmptyResult;

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void AssignCurrency_WhenGetWithInvalidUserId_AccessDenied()
        {
            // Arrange
            AppSettings.EnableAssignCurrency.Returns(true);

            // Action
            var result = _controller.AssignCurrency(0) as RedirectResult;

            // Assert
            Assert.That(result.Url, Is.EqualTo(AccessDeniedUrl));
        }

        [Test]
        public void AssignCurrency_WhenGet_ReturnAssignCurrencyView()
        {
            // Arrange
            AppSettings.EnableAssignCurrency.Returns(true);

            _currencyDataAccess.GetCurrenciesByUser(DefaultLicenseeSession.LicenseeId, DefaultUserSession.UserId).Returns(new List<CurrencyUser>
            {
                new CurrencyUser()
            });

            // Action
            var viewResult = _controller.AssignCurrency(DefaultUserSession.UserId) as ViewResult;

            // Assert
            Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Account/AssignCurrency.cshtml"));
        }

        [Test]
        public void AssignCurrency_WhenGetWithKeenOceanLicensee_SetCurrenyNameIsHKD()
        {
            // Arrange
            AppSettings.EnableAssignCurrency.Returns(true);

            StubLicenseeSessionData(new LicenseeSession
            {
                LicenseeId = LicenseeIdentity.KeenOcean
            });

            _currencyDataAccess.GetCurrenciesByUser(LicenseeIdentity.KeenOcean, DefaultUserSession.UserId).Returns(new List<CurrencyUser>
            {
                new CurrencyUser { CurrencyId = CurrencyType.Hkd }
            });

            // Action
            var viewResult = _controller.AssignCurrency(DefaultUserSession.UserId) as ViewResult;

            // Assert
            Assert.That(viewResult.ViewBag.CurrencyList[0].CurrencyName, Is.EqualTo("HKD"));
        }

        [Test]
        public void AssignCurrency_WhenPostWithInValidUserId_ReturnFailResult()
        {
            // Action
            var result = _controller.AssignCurrency(new AssignCurrencyMetaModel
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
        public void AssignCurrency_WhenPostWithInCurrencyList_ReturnFailResult()
        {
            // Action
            var result = _controller.AssignCurrency(new AssignCurrencyMetaModel
            {
                UserId = DefaultUserSession.UserId,
                CurrencyList = null
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
        public void AssignCurrency_WhenPostWithoutEnableAssignCurrency_ReturnFailResult()
        {
            // Arrange
            AppSettings.EnableAssignCurrency.Returns(false);

            // Action
            var result = _controller.AssignCurrency(new AssignCurrencyMetaModel
            {
                UserId = DefaultUserSession.UserId,
                CurrencyList = "1,2,3"
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
        public void AssignCurrency_WhenPost_ValidatePermissionRestrictionFailed_ReturnFailResult()
        {
            // Arrange
            AppSettings.EnableAssignCurrency.Returns(true);

            // Action
            var result = _controller.AssignCurrency(new AssignCurrencyMetaModel
            {
                UserId = DefaultUserSession.UserId,
                CurrencyList = "1,2,3"
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
        public void AssignCurrency_ValidUser_BuildCorrectCurrencyList()
        {
            // Arrange
            AppSettings.EnableAssignCurrency.Returns(true);

            StubGetCurrenciesByUser(
                DefaultLicenseeSession.LicenseeId,
                DefaultUserSession.UserId, 
                new List<CurrencyUser>
                {
                    new CurrencyUser { CurrencyId = 1 },
                    new CurrencyUser { CurrencyId = 6 }
                });

            StubAssignCurrenciesByUser(new ResultInfo());

            var model = new AssignCurrencyMetaModel
            {
                UserId = 111,
                CurrencyList = "1,2,3,4,5"
            };

            // Action
            var result = _controller.AssignCurrency(model) as JsonResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.That(
                model.CurrencyList,
                Is.EqualTo("<currencies><currency currencyid=\"1\" status=\"1\"></currency><currency currencyid=\"6\" status=\"0\"></currency></currencies>"));
        }

        [Test]
        public void AssignCurrency_UserCurrencyListGetFromDataAccessIsEmpty_CurrencyListHasEmptyElement()
        {
            // Arrange
            AppSettings.EnableAssignCurrency.Returns(true);

            StubGetCurrenciesByUser(DefaultLicenseeSession.LicenseeId, DefaultUserSession.UserId, new List<CurrencyUser>());

            StubAssignCurrenciesByUser(new ResultInfo());

            var model = new AssignCurrencyMetaModel
            {
                UserId = 111,
                CurrencyList = "1,2,3,4,5"
            };

            // Action
            var result = _controller.AssignCurrency(model) as JsonResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.That(model.CurrencyList, Is.EqualTo("<currencies></currencies>"));
        }

        [Test]
        public void AssignCurrency_ValidUserInfo_ReturnSuccessfulData()
        {
            // Arrange
            AppSettings.EnableAssignCurrency.Returns(true);

            StubGetCurrenciesByUser(DefaultLicenseeSession.LicenseeId, DefaultUserSession.UserId, new List<CurrencyUser> { new CurrencyUser { CurrencyId = 1 } });

            StubAssignCurrenciesByUser(new ResultInfo { Code = 0 });

            var model = new AssignCurrencyMetaModel
            {
                UserId = 111,
                CurrencyList = "1,2,3,4,5"
            };

            // Action
            var result = _controller.AssignCurrency(model) as JsonResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.That(resultInfo.IsSuccessful, Is.True);
        }

        [Test]
        public void AssignCurrency_ValidUserButCallSeviceFailed_ReturnUnSuccessfulData()
        {
            // Arrange
            AppSettings.EnableAssignCurrency.Returns(true);

            StubGetCurrenciesByUser(DefaultLicenseeSession.LicenseeId, DefaultUserSession.UserId, new List<CurrencyUser> { new CurrencyUser { CurrencyId = 1 } });

            StubAssignCurrenciesByUser(new ResultInfo { Code = 1, Message = "Error" });

            var model = new AssignCurrencyMetaModel
            {
                UserId = 111,
                CurrencyList = "1,2,3,4,5"
            };

            // Action
            var result = _controller.AssignCurrency(model) as JsonResult;
            var resultInfo = result.Data as ResultInfo;

            // Assert
            Assert.That(resultInfo.IsSuccessful, Is.False);
        }

        private void StubGetCurrenciesByUser(int licenseeId, int userId, List<CurrencyUser> fakeResult)
        {
            LangService.GetCurrentLanguage().Returns(DefaultUserSession.Language);
            _currencyDataAccess.GetCurrenciesByUser(DefaultLicenseeSession.LicenseeId, DefaultUserSession.UserId).Returns(x => fakeResult);
        }

        private void StubAssignCurrenciesByUser(ResultInfo fakeResult)
        {
            _currencyDataAccess.AssignCurrenciesByUser(Arg.Any<AssignCurrencyMetaModel>()).Returns(x => fakeResult);
        }
    }
}