namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Web.Mvc;
    using Base.UnitTests;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public partial class UserGuideControllerTest : BaseControllerTest
    {
        [OneTimeSetUp]
        public new void FixtureSetUp()
        {
            FakeBaseControllerDependency();
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectView()
        {
            // Arrange
            const string ResourceName = "CustomerList";
            using (var controller = CreateController())
            {
                controller.Url = this.Url;
                controller.GetFileContent(Arg.Any<string>()).Returns("{rooturl}content-here");

                // Action
                var result = controller.Index(ResourceName) as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/UserGuide/Index.cshtml"));
                Assert.That(result.Model, Is.EqualTo("/content-here"));
            }
        }

        [Test]
        public void DownloadCustomerListPdf_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            using (var controller = CreateController())
            {
                // Action
                var result = controller.DownloadCustomerListPdf() as FilePathResult;

                // Assert
                Assert.That(result.FileName, Is.EqualTo("~/App_Data/UserGuide/CustomerList/pdf/en-Us/Odds Group Feature_Licensee.pdf"));
            }
        }

        private UserGuideController CreateController()
        {
            var controller = Substitute.ForPartsOf<UserGuideController>(
                LangService,
                SessionService,
                AppSettings);

            controller.ControllerContext = this.ControllerContext;

            return controller;
        }
    }
}