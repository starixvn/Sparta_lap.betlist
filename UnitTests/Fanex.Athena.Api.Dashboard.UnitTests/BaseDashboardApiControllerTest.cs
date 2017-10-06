namespace Fanex.Athena.Api.Dashboard.UnitTests
{
    using System.Web.Http.Controllers;
    using Model.Dashboard.MetaModel;
    using NUnit.Framework;

    public class BaseDashboardApiControllerTest
    {
        [Test]
        public void LicenseeId_WhenCall_ReturnCorrectValue()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const int Expected = 1;

                // Action
                var actual = controller.LicenseeId;

                // Assert
                Assert.That(actual, Is.EqualTo(Expected));
            }
        }

        [Test]
        public void UserId_WhenCall_ReturnCorrectValue()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const int Expected = 2;

                // Action
                var actual = controller.UserId;

                // Assert
                Assert.That(actual, Is.EqualTo(Expected));
            }
        }

        [Test]
        public void CurrencyIDList_WhenCall_ReturnCorrectValue()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string Expected = "1";

                // Action
                var actual = controller.CurrencyIDList;

                // Assert
                Assert.That(actual, Is.EqualTo(Expected));
            }
        }

        [Test]
        public void AssignAuthProperties_CurrencyIsZero_AssignCurrencyIdListFromActionContext()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string Expected = "1";

                var model = new DashboardBaseModel { CurrencyId = 0 };

                // Action
                controller.AssignAuthProperties(model);

                // Assert
                Assert.That(model.CurrencyIDList, Is.EqualTo(Expected));
            }
        }

        [Test]
        public void AssignAuthProperties_CurrencyIdIsNotZero_AssignCurrencyIdListFromCurrencyId()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var model = new DashboardBaseModel { CurrencyId = 2 };

                // Action
                controller.AssignAuthProperties(model);

                // Assert
                Assert.That(model.CurrencyIDList, Is.EqualTo(model.CurrencyId.ToString()));
            }
        }

        [Test]
        public void AssignAuthProperties_WhenCall_AssignLicenseeIdFromActionContext()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string Expected = "1";

                var model = new DashboardBaseModel { CurrencyId = 0 };

                // Action
                controller.AssignAuthProperties(model);

                // Assert
                Assert.That(model.LicenseeId, Is.EqualTo(Expected));
            }
        }

        private static FakeBaseDashboardApiController CreateController()
        {
            var controller = new FakeBaseDashboardApiController();
            var mockActionContext = new HttpActionContext
            {
                ControllerContext = new HttpControllerContext(),
                ActionArguments = { { "licenseeid", 1 }, { "userid", 2 }, { "currencyIDList", "1" } }
            };

            controller.ActionContext = mockActionContext;

            return controller;
        }
    }
}