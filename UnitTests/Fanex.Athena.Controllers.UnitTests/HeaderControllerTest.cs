namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Web.Mvc;
    using Controllers.Base.UnitTests;
    using DataAccess.Core.Users.Interfaces;
    using Model.Core.ViewModel;
    using NSubstitute;
    using NUnit.Framework;

    public class HeaderControllerTest : BaseControllerHelperTest
    {
        private ILicenseeDataAccess _licenseeDataAccess;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            SetUpDefaultFakeData();

            _licenseeDataAccess = Substitute.For<ILicenseeDataAccess>();
            LangService.When(x => x.ChangeLanguage(Arg.Any<string>())).DoNotCallBase();
            SessionService.When(x => x.UpdateLiceseeSession(Arg.Any<int>())).DoNotCallBase();
        }

        [TearDown]
        public void TearDown()
        {
            _licenseeDataAccess.ClearReceivedCalls();
            SessionService.ClearReceivedCalls();
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectViewName()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = controller.Index() as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/Header/Index.cshtml"));
            }
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectViewModel()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const bool HasRecheckIn = true;
                AppSettings.LocalTest.Returns(!HasRecheckIn);

                const bool IsFakeLicensee = true;
                AppSettings.FakeLicenseeList.Returns(new[] { DefaultLicenseeSession.LicenseeId });

                // Action
                var result = controller.Index() as ViewResult;
                var model = (HeaderPageViewModel)result.Model;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/Header/Index.cshtml"));
                Assert.Multiple(() =>
                {
                    Assert.That(model.Language, Is.EqualTo(DefaultUserSession.Language));
                    Assert.That(model.OnUserId, Is.EqualTo(DefaultUserSession.OnUserId));
                    Assert.That(model.Language, Is.EqualTo(DefaultUserSession.Language));
                    Assert.That(model.IsInternalLogin, Is.EqualTo(DefaultUserSession.IsInternal));
                    Assert.That(model.HasRecheckIn, Is.EqualTo(HasRecheckIn));
                    Assert.That(model.UserName, Is.EqualTo(DefaultUserSession.UserName));
                    Assert.That(model.CurrentLicenseeId, Is.EqualTo(DefaultLicenseeSession.LicenseeId));
                    Assert.That(model.IsFakeLicensee, Is.EqualTo(IsFakeLicensee));
                });
            }
        }

        [TestCase(0)]
        [TestCase(null)]
        public void ChangeLicensee_LicenseeIsLessThanZero_DoNotUpdateLicenseeSession(int? licenseeId)
        {
            using (var controller = CreateController())
            {
                // Action
                var result = controller.ChangeLicensee(licenseeId);

                // Assert
                SessionService.DidNotReceive().UpdateLiceseeSession(Arg.Any<int>());
            }
        }

        [Test]
        public void ChangeLicensee_NewLicenseeIsEqualToCurrent_DoNotUpdateLicenseeSession()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = controller.ChangeLicensee(DefaultLicenseeSession.LicenseeId);

                // Assert
                SessionService.DidNotReceive().UpdateLiceseeSession(Arg.Any<int>());
            }
        }

        [Test]
        public void ChangeLicensee_NewLicenseeIsNotBelongToUser_DoNotUpdateLicenseeSession()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const int LicenseeId = 10;
                _licenseeDataAccess.IsLicenseeBelongToUser(Arg.Any<int>(), Arg.Any<int>()).Returns(false);

                // Action
                var result = controller.ChangeLicensee(LicenseeId);

                // Assert
                SessionService.DidNotReceive().UpdateLiceseeSession(Arg.Any<int>());
            }
        }

        [Test]
        public void ChangeLicensee_InvalidNewLicenseeId_UpdateLicenseeSession()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const int LicenseeId = 10;
                _licenseeDataAccess.IsLicenseeBelongToUser(Arg.Any<int>(), Arg.Any<int>()).Returns(true);

                // Action
                var result = controller.ChangeLicensee(LicenseeId);

                // Assert
                SessionService.Received().UpdateLiceseeSession(LicenseeId);
            }
        }

        [TestCase("")]
        [TestCase("Invalid")]
        [TestCase("en-US")]
        public void ChangeLanguage_InvalidLanguageCode_ReturnZero(string languagesite)
        {
            using (var controller = CreateController())
            {
                // Arrange
                LangService.GetCurrentLanguage().Returns("en-US");
                LangService.ChangeLanguage(Arg.Any<string>()).Returns(false);

                // Action
                var result = controller.ChangeLanguage(languagesite);

                // Assert
                Assert.That(result, Is.EqualTo("0")); 
            }
        }

        [TestCase(" en-US")]
        [TestCase("en-US ")]
        [TestCase(" en-US ")]
        [TestCase("en-US")]
        public void ChangeLanguage_ValidLanguageCode_ReturnOne(string languagesite)
        {
            using (var controller = CreateController())
            {
                // Arrange
                LangService.GetCurrentLanguage().Returns("zh-TW");
                LangService.ChangeLanguage(Arg.Any<string>()).Returns(true);

                // Action
                var result = controller.ChangeLanguage(languagesite);

                // Assert
                Assert.That(result, Is.EqualTo("1"));
            }
        }

        private HeaderController CreateController()
        {
            return new HeaderController(LangService, SessionService, AppSettings, _licenseeDataAccess);
        }
    }
}