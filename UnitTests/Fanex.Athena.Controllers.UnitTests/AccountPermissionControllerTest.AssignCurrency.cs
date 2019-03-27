namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Model.Core.MetaModel.Account;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using Session.Model;

    public partial class AccountPermissionControllerTest
    {
        [Test]
        public async Task AssignCurrency_WhenGetWithoutEnableAssignCurrency_ReturnEmptyResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                AppSettings.EnableAssignCurrency.Returns(false);

                // Action
                var result = await controller.AssignCurrency(0) as EmptyResult;

                // Assert
                Assert.That(result, Is.Not.Null);
            }
        }

        [Test]
        public async Task AssignCurrency_WhenGetWithInvalidUserId_AccessDenied()
        {
            using (var controller = CreateController())
            {
                // Arrange
                AppSettings.EnableAssignCurrency.Returns(true);

                // Action
                var result = await controller.AssignCurrency(0) as RedirectResult;

                // Assert
                Assert.That(result.Url, Is.EqualTo(AccessDeniedUrl));
            }
        }

        [Test]
        public async Task AssignCurrency_WhenGet_ReturnAssignCurrencyView()
        {
            using (var controller = CreateController())
            {
                // Arrange
                AppSettings.EnableAssignCurrency.Returns(true);

                _currencyDataAccess.GetCurrenciesByUser(DefaultLicenseeSession.LicenseeId, DefaultUserSession.UserId).Returns(new List<CurrencyUser>
                {
                    new CurrencyUser()
                });

                // Action
                var viewResult = await controller.AssignCurrency(DefaultUserSession.UserId) as ViewResult;

                // Assert
                Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Account/AssignCurrency.cshtml"));
            }
        }

        [Test]
        public async Task AssignCurrency_WhenGetWithKeenOceanLicensee_SetCurrenyNameIsHKD()
        {
            using (var controller = CreateController())
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
                var viewResult = await controller.AssignCurrency(DefaultUserSession.UserId) as ViewResult;

                // Assert
                Assert.That(viewResult.ViewBag.CurrencyList[0].CurrencyName, Is.EqualTo("HKD"));
            }
        }

        [Test]
        public async Task AssignCurrency_WhenPostWithInValidUserId_ReturnFailResult()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = await controller.AssignCurrency(new AssignCurrencyMetaModel
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
        }

        [Test]
        public async Task AssignCurrency_WhenPostWithInCurrencyList_ReturnFailResult()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = await controller.AssignCurrency(new AssignCurrencyMetaModel
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
        }

        [Test]
        public async Task AssignCurrency_WhenPostWithoutEnableAssignCurrency_ReturnFailResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                AppSettings.EnableAssignCurrency.Returns(false);

                // Action
                var result = await controller.AssignCurrency(new AssignCurrencyMetaModel
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
        }

        [Test]
        public async Task AssignCurrency_WhenPost_ValidatePermissionRestrictionFailed_ReturnFailResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                AppSettings.EnableAssignCurrency.Returns(true);

                // Action
                var result = await controller.AssignCurrency(new AssignCurrencyMetaModel
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
        }

        [Test]
        public async Task AssignCurrency_ValidUser_BuildCorrectCurrencyList()
        {
            using (var controller = CreateController())
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
                var result = await controller.AssignCurrency(model) as JsonResult;
                var resultInfo = result.Data as ResultInfo;

                // Assert
                Assert.That(
                    model.CurrencyList,
                    Is.EqualTo("<currencies><currency currencyid=\"1\" status=\"1\"></currency><currency currencyid=\"6\" status=\"0\"></currency></currencies>"));
            }
        }

        [Test]
        public async Task AssignCurrency_UserCurrencyListGetFromDataAccessIsEmpty_CurrencyListHasEmptyElement()
        {
            using (var controller = CreateController())
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
                var result = await controller.AssignCurrency(model) as JsonResult;
                var resultInfo = result.Data as ResultInfo;

                // Assert
                Assert.That(model.CurrencyList, Is.EqualTo("<currencies></currencies>"));
            }
        }

        [Test]
        public async Task AssignCurrency_ValidUserInfo_ReturnSuccessfulData()
        {
            using (var controller = CreateController())
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
                var result = await controller.AssignCurrency(model) as JsonResult;
                var resultInfo = result.Data as ResultInfo;

                // Assert
                Assert.That(resultInfo.IsSuccessful, Is.True);
            }
        }

        [Test]
        public async Task AssignCurrency_ValidUserButCallSeviceFailed_ReturnUnSuccessfulData()
        {
            using (var controller = CreateController())
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
                var result = await controller.AssignCurrency(model) as JsonResult;
                var resultInfo = result.Data as ResultInfo;

                // Assert
                Assert.That(resultInfo.IsSuccessful, Is.False);
            }
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