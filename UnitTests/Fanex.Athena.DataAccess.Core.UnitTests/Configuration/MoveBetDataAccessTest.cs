namespace Fanex.Athena.DataAccess.Core.UnitTests.Configuration
{
    using System;
    using System.Data;
    using Core.Configuration;
    using Data;
    using Model.Core.DTO.Configuration;
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

        [Test]
        public void GetMoveBet_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            var dataAccess = Substitute.ForPartsOf<MoveBetDataAccess>();
            dataAccess.GetObjectDb(Arg.Is<string>("Lrf_ShowNotificationWhenProcessingData")).Returns(objectDb);

            var availableWinlossDate = "8/01/2016";
            var estimatedTime = "8/15/2016";
            objectDb.GetParameterValue("AvailableWinlostDate").Returns(availableWinlossDate);
            objectDb.GetParameterValue("EstimatedTime").Returns(estimatedTime);

            var expected = new MoveBet
            {
                AvailableWinlostDate = DateTime.Parse(availableWinlossDate),
                EstimatedTime = DateTime.Parse(estimatedTime)
            };

            // Action
            var result = dataAccess.GetMoveBet();

            // Assert
            Assert.That(result.AvailableWinlostDate, Is.EqualTo(expected.AvailableWinlostDate));
            Assert.That(result.EstimatedTime, Is.EqualTo(expected.EstimatedTime));
        }
    }
}