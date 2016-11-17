namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Web.Mvc;
    using Fanex.Athena.Resources;
    using NSubstitute;
    using NUnit.Framework;
    using ONEbook.UM.Models;
    using ONEbook.UM.Services;

    public class UtilityControllerTest : BaseControllerTest
    {
        private IMaintenanceService _maintenanceService;

        [SetUp]
        public void SetUp()
        {
            _maintenanceService = Substitute.For<IMaintenanceService>();
        }

        [TearDown]
        public void TearDown()
        {
            _maintenanceService = null;
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("20 0")]
        public void OneTokenRestriction_InvalidCode_ReturnDefaultMsg(string code)
        {
            // Arrange
            FakeBaseControllerDependency();
            using (var controller = new UtilityController(LangService, SessionService, AppSettings, _maintenanceService))
            {
                controller.ControllerContext = this.ControllerContext;
                var expectedMsg = ViewResources.OneTokenRestrictionDefaultMsg;

                // Action
                var result = controller.OneTokenRestriction(code) as ViewResult;

                // Assert
                Assert.AreEqual(expectedMsg, result.ViewBag.Message);
            }
        }

        [TestCase("100", "Token was expired")]
        [TestCase("200", "Licensee is not existed")]
        [TestCase("400", "Direct Access to this website is restricted. Please contact Administrator for further enquiries.")]
        [TestCase("600", "Licensee does not offered currency")]
        [TestCase("300", "Token is invalid")]
        [TestCase("700", "You are not allowed to access this page !!!")]
        public void OneTokenRestriction_ValidCode_ReturnMsgByCode(string code, string msg)
        {
            // Arrange
            FakeBaseControllerDependency();
            using (var controller = new UtilityController(LangService, SessionService, AppSettings, _maintenanceService))
            {
                ControllerContext = ControllerContext;

                // Action
                var result = controller.OneTokenRestriction(code) as ViewResult;

                // Assert
                Assert.AreEqual(msg, result.ViewBag.Message);
            }
        }

        [Test]
        public void UnderMaintenance_UnderMaintenanceState_ReturnRightUMTimeInfo()
        {
            // Arrange
            FakeBaseControllerDependency();
            using (var controller = new UtilityController(LangService, SessionService, AppSettings, _maintenanceService))
            {
                controller.ControllerContext = this.ControllerContext;
                var maintenanceInfo = new UnderMaintenanceTime(true, "11-14-2016 05:00:00", "11-14-2016 07:00:00");
                _maintenanceService.GetUnderMaintenanceTime().Returns(x => maintenanceInfo);

                // Action
                var result = controller.UnderMaintenance(Arg.Any<bool>()) as ViewResult;

                // Assert
                Assert.AreEqual(maintenanceInfo.From, result.ViewBag.UmTimeFrom);
                Assert.AreEqual(maintenanceInfo.To, result.ViewBag.UmTimeTo);
            }
        }

        [Test]
        public void UnderMaintenance_NotUnderMaintenanceState_ShowEmptyUMInfo()
        {
            // Arrange
            FakeBaseControllerDependency();
            using (var controller = new UtilityController(LangService, SessionService, AppSettings, _maintenanceService))
            {
                controller.ControllerContext = ControllerContext;
                var maintenanceInfo = new UnderMaintenanceTime(false, "11-14-2016 05:00:00", "11-14-2016 07:00:00");
                _maintenanceService.GetUnderMaintenanceTime().Returns(x => maintenanceInfo);

                // Action
                var result = controller.UnderMaintenance(true) as ViewResult;

                // Assert
                Assert.AreEqual(string.Empty, result.ViewBag.UmTimeFrom);
                Assert.AreEqual(string.Empty, result.ViewBag.UmTimeTo);
            }
        }
    }
}