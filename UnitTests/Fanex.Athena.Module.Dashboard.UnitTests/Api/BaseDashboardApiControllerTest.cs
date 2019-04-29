namespace Fanex.Athena.Module.Dashboard.UnitTests.Api
{
    using System.Web.Http.Controllers;
    using Fanex.Athena.Module.Dashboard.MetaModel;
    using NUnit.Framework;

    public class BaseDashboardApiControllerTest
    {
        private const int ActionLicenseeId = 1;
        private const int ActionUserId = 2;
        private const string ActionCurrencyIdList = "1";

        [Test]
        public void LicenseeId_WhenCall_ReturnCorrectValue()
        {
            using (var controller = CreateController())
            {
                // Action
                var actual = controller.LicenseeId;

                // Assert
                Assert.That(actual, Is.EqualTo(ActionLicenseeId));
            }
        }

        [Test]
        public void UserId_WhenCall_ReturnCorrectValue()
        {
            using (var controller = CreateController())
            {
                // Action
                var actual = controller.UserId;

                // Assert
                Assert.That(actual, Is.EqualTo(ActionUserId));
            }
        }

        [Test]
        public void CurrencyIDList_WhenCall_ReturnCorrectValue()
        {
            using (var controller = CreateController())
            {
                // Action
                var actual = controller.CurrencyIDList;

                // Assert
                Assert.That(actual, Is.EqualTo(ActionCurrencyIdList));
            }
        }

        [TestCase(0, ActionCurrencyIdList)]
        [TestCase(3, "3")]
        public void AssignAuthProperties_WhenCall_AssignCurrencyIdListForModel(int currencyId, string expected)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var model = new DashboardBaseModel { CurrencyId = currencyId };

                // Action
                controller.AssignAuthProperties(model);

                // Assert
                Assert.That(model.CurrencyIDList, Is.EqualTo(expected));
            }
        }

        private static FakeBaseDashboardApiController CreateController()
        {
            var mockActionContext = new HttpActionContext
            {
                ControllerContext = new HttpControllerContext(),
                ActionArguments =
                {
                    { "licenseeid", ActionLicenseeId },
                    { "userid", ActionUserId },
                    { "currencyIDList", ActionCurrencyIdList }
                }
            };

            return new FakeBaseDashboardApiController
            {
                ActionContext = mockActionContext
            };
        }
    }
}