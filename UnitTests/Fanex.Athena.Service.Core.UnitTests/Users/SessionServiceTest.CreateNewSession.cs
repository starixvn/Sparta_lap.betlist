namespace Fanex.Athena.Service.Core.UnitTests.Users
{
    using System.Collections.Generic;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public partial class SessionServiceTest
    {
        [Test]
        public void CreateNewSession_WhenCall_UserSessionIsCreatedProperly()
        {
            // Action
            CreateDefaultNewSession();

            // Assert
            Assert.That(_sessionService.UserSession.UserId, Is.EqualTo(DefaultUserId));
        }

        [Test]
        public void CreateNewSession_WhenCall_LicenseeSessionIsCreatedProperly()
        {
            // Arrange
            const int LicenseeId = 1;
            var licensees = new List<Licensee>
            {
                new Licensee { Id = 1, Name = "spondemo" }
            };
            _licenseeDataAccess.GetByUser(DefaultUserId).Returns(licensees);

            // Action
            CreateDefaultNewSession(LicenseeId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_sessionService.LicenseeSession.LicenseeId, Is.EqualTo(LicenseeId));
                Assert.That(_sessionService.LicenseeSession.LicenseeName, Is.EqualTo("spondemo"));
            });
        }

        [Test]
        public void CreateNewSession_LicenseeIdIsZero_LicenseeSessionIsNull()
        {
            // Action
            CreateDefaultNewSession(0);

            // Assert
            Assert.That(_sessionService.LicenseeSession, Is.Null);
        }
    }
}