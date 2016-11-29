namespace Fanex.Athena.Service.Core.UnitTests.Users
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Core.Common.Interfaces;
    using Core.Users.Interfaces;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.Service.Core.Users;
    using Model.Core.DataDomain;
    using Model.Core.DTO;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Newtonsoft.Json;
    using NSubstitute;
    using NUnit.Framework;
    using Fanex.Athena.Model.Core.MetaModel.Account;

    [TestFixture]
    public class OneAuthenticationService_SignIn_VerifyUserInfoTest
    {
        private OneAuthenticationService _oneAuthenticationService;
        private ISessionService _sessionService;
        private ILicenseeService _licenseeService;
        private ICurrencyService _currencyService;
        private IFormsAuthenticationService _formAuthenticationService;
        private IUserService _userService;
        [SetUp]
        public void SetUp()
        {
            _sessionService = Substitute.For<ISessionService>();
            _licenseeService = Substitute.For<ILicenseeService>();
            _currencyService = Substitute.For<ICurrencyService>();
            _userService = Substitute.For<IUserService>();
            _sessionService.UserSession.Returns(x => null);
            _sessionService.LicenseeSession.Returns(x => null);
            _licenseeService.GetLicenseeByName(Arg.Any<string>()).Returns(new Licensee { Id = 1, Name = "test", IsAllowedCustomerComm = true });
            _formAuthenticationService = Substitute.For<IFormsAuthenticationService>();
        }

        [TearDown]
        public void TearDown()
        {
            _oneAuthenticationService = null;
            _sessionService = null;
            _licenseeService = null;
            _currencyService = null;
            _formAuthenticationService = null;
        }

        [Test]
        public void SignIn_NotFoundUser_ReturnErrorNotFoundUser()
        {
            // Arrange
            var tokenInfo = new OneAuthenticationToken { UserID = "1", UserSEQ = 1, UserName = "pgmAlex", SiteName = "spondemo" };
            var fakeHttpContext = Substitute.For<HttpContextBase>();
            fakeHttpContext.User.Identity.IsAuthenticated.Returns(false);
            _userService.GetByUsername(tokenInfo.UserName, Arg.Any<int>()).Returns(x => null);
            _oneAuthenticationService = new OneAuthenticationService(_sessionService, _licenseeService, _currencyService, _formAuthenticationService, _userService);

            var expectedResult = new ResultInfo { Code = 800, Message = "Not found user" };

            // Action
            var result = _oneAuthenticationService.SignIn(fakeHttpContext, tokenInfo);

            // Assert
            Assert.AreEqual(expectedResult.IsSuccessful, result.IsSuccessful);
            Assert.AreEqual(expectedResult.Code, result.Code);
        }

        [Test]
        public void SignIn_ExistUser_UpdateUserSEOAndCallSetAuthorize()
        {
            // Arrange
            var counter = 0;
            var exptectedUserId = 10;
            var tokenInfo = new OneAuthenticationToken { UserID = "1", UserSEQ = 1, UserName = "pgmAlex", SiteName = "spondemo" };
            var fakeHttpContext = Substitute.For<HttpContextBase>();
            fakeHttpContext.User.Identity.IsAuthenticated.Returns(false);
            _userService.GetByUsername(tokenInfo.UserName, Arg.Any<int>()).Returns(x => new User { UserID = exptectedUserId });

            _oneAuthenticationService = Substitute.ForPartsOf<OneAuthenticationService>(_sessionService, _licenseeService, _currencyService, _formAuthenticationService, _userService);

            _oneAuthenticationService.When(x => x.SetAuthorize(Arg.Any<Licensee>(), Arg.Any<string>(), tokenInfo)).DoNotCallBase();
            _oneAuthenticationService.When(x => x.SetAuthorize(Arg.Any<Licensee>(), Arg.Any<string>(), tokenInfo)).Do(x => counter++);

            // Action
            var result = _oneAuthenticationService.SignIn(fakeHttpContext, tokenInfo);

            // Assert
            _oneAuthenticationService.Received().SetAuthorize(Arg.Any<Licensee>(), Arg.Any<string>(), Arg.Is<OneAuthenticationToken>(tokenInfo));
            Assert.AreEqual(exptectedUserId, tokenInfo.UserSEQ);
            Assert.AreEqual(1, counter);
        }


    }
}