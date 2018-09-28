namespace Fanex.Athena.Service.Core.UnitTests.Users
{
    using Core.Users.Interfaces;
    using Core.Utility.Interfaces;
    using DataAccess.Core.Users.Interfaces;
    using Fanex.Athena.Service.Core.Users;
    using Model.Core.DTO.OneAuth;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class OneAuthenticationServiceTest_GetTokenInfo
    {
        private OneAuthenticationService _oneAuthenticationService;
        private IOneBookSecurityWrapper _oneBookSecurityWrapper;

        [SetUp]
        public void SetUp()
        {
            _oneBookSecurityWrapper = Substitute.For<IOneBookSecurityWrapper>();
            _oneAuthenticationService = Substitute.ForPartsOf<OneAuthenticationService>(
                null,
                null,
                null,
                _oneBookSecurityWrapper,
                Substitute.For<ICoreLogger>(),
                Substitute.For<IOneAuthDataAccess>());
        }

        [TearDown]
        public void TearDown()
        {
            _oneAuthenticationService = null;
            _oneBookSecurityWrapper = null;
        }

        [Test]
        public void GetTokenInfo_WhenCall_ReturnValidInfo()
        {
            // Arrange
            var token = FakeHelper.Create<string>();

            var expectedResult = new OneAuthToken
            {
                IsAdminstrator = true,
                Currency = new OneAuthCurrency[]
                {
                    new OneAuthCurrency()
                },
                Permissions = new OneAuthPermission[]
                {
                    new OneAuthPermission()
                }
            };
            _oneBookSecurityWrapper.Authenticate(token)
                .Returns(expectedResult);

            // Action
            var result = _oneAuthenticationService.GetTokenInfo(token);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}