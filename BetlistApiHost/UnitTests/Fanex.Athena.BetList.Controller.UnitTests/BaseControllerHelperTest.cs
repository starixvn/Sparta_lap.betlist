namespace Fanex.Athena.BetList.Controller.UnitTests
{
    using System.Web.Mvc;
    using Common.Utility.Services.Interfaces;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public abstract class BaseControllerHelperTest
    {
        protected UrlHelper Url { get; set; }

        protected ControllerContext ControllerContext { get; set; }

        protected IAppSettings AppSettings { get; set; }

        [OneTimeSetUp]
        public virtual void FixtureSetUp()
        {
            AppSettings = Substitute.For<IAppSettings>();
            SetUpForContexts();
        }

        [OneTimeTearDown]
        public virtual void FixtureTearDown()
        {
            Url = null;
            ControllerContext = null;
            AppSettings = null;
        }

        protected void StubGetValueAppSettings(string valueName, string fakeValue)
        {
            AppSettings.GetValue(valueName).Returns(fakeValue);
        }

        private void SetUpForContexts()
        {
            ControllerContext = MvcHelper.CreateControllerContext();
            Url = MvcHelper.CreateUrlHelper(ControllerContext.HttpContext);
        }
    }
}