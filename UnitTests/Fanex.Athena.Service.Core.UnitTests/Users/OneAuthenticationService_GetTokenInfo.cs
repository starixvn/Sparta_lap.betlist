namespace Fanex.Athena.Service.Core.UnitTests.Users
{
    using Core.Users.Interfaces;
    using Fanex.Athena.Service.Core.Users;
    using Model.Core.DTO.OneAuth;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class OneAuthenticationService_GetTokenInfo
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
                null,
                null,
                _oneBookSecurityWrapper);
        }

        [TearDown]
        public void TearDown()
        {
            _oneAuthenticationService = null;
            _oneBookSecurityWrapper = null;
        }

        [Test]
        public void GetTokenInfo_NotHaveSession_CallSetAuthorize()
        {
            // Arrange
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
            _oneBookSecurityWrapper.Authenticate(Arg.Any<string>())
                .Returns(expectedResult);

            // Action
            var result = _oneAuthenticationService.GetTokenInfo("abc");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsAdminstrator, Is.True);
                Assert.That(result.Currency.Length, Is.EqualTo(1));
                Assert.That(result.Permissions.Length, Is.EqualTo(1));
            });
        }
    }
}