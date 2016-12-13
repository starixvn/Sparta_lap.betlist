namespace Fanex.Athena.Service.Core.UnitTests.Common
{
    using System.Collections.Generic;
    using DataAccess.TotalBet.Interfaces;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using TotalBet;
    using TotalBet.Interfaces;

    [TestFixture]
    public class HalfTimeFullTimeServiceTest
    {
        private IHalfTimeFullTimeDataAccess _dataAccess;

        private IHalfTimeFullTimeService _service;

        [OneTimeSetUp]
        public void SetUp()
        {
            _dataAccess = Substitute.For<IHalfTimeFullTimeDataAccess>();
            _service = new HalfTimeFullTimeService(_dataAccess);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _dataAccess = null;
            _service = null;
        }

        [Test]
        public void GetHalfTimeFullTimeDataList_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = new List<HalfTimeFullTime>
            {
                new HalfTimeFullTime()
            };
            HalfTimeFullTime totalData;
            _dataAccess.GetHalfTimeFullTimeDataList(Arg.Any<HalfTimeFullTimeMetaModel>(), out totalData)
                .Returns(expectedResult);

            // Action
            var result = _service.GetHalfTimeFullTimeDataList(new HalfTimeFullTimeMetaModel(), out totalData);

            // Assert
            _dataAccess.ReceivedWithAnyArgs().GetHalfTimeFullTimeDataList(Arg.Any<HalfTimeFullTimeMetaModel>(), out totalData);
            Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
        }
    }
}