namespace Fanex.Athena.DataAccess.Core.Tests.Configuration
{
    using System.Data;
    using Core.Configuration;
    using Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class MoveBetDataAccessTest
    {
        [Test]
        public void GetMoveBet_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            var dataAccess = Substitute.ForPartsOf<MoveBetDataAccess>();
            dataAccess.GetObjectDb(Arg.Is<string>("Lrf_ShowNotificationWhenProcessingData")).Returns(objectDb);

            // Action
            dataAccess.GetMoveBet();

            // Assert
            objectDb.Received().AddOutParameter("AvailableWinlostDate", DbType.DateTime, 10);
            objectDb.Received().AddOutParameter("EstimatedTime", DbType.DateTime, 10);
        }
    }
}