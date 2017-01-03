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
    public class HandicapOverUnderServiceTest
    {
        private IHandicapOverUnderDataAccess _dataAccess;

        private IHandicapOverUnderService _service;

        [OneTimeSetUp]
        public void SetUp()
        {
            _dataAccess = Substitute.For<IHandicapOverUnderDataAccess>();
            _service = new HandicapOverUnderService(_dataAccess);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _dataAccess = null;
            _service = null;
        }

        [Test]
        public void GetHandicapOverUnderDataList_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = new List<HandicapOverUnder>
            {
                new HandicapOverUnder()
            };
            HandicapOverUnder totalData;
            _dataAccess.GetHandicapOverUnderDataList(Arg.Any<HandicapOverUnderMetaModel>(), out totalData)
                .Returns(expectedResult);

            // Action
            var result = _service.GetHandicapOverUnderDataList(new HandicapOverUnderMetaModel(), out totalData);

            // Assert
            _dataAccess.ReceivedWithAnyArgs().GetHandicapOverUnderDataList(Arg.Any<HandicapOverUnderMetaModel>(), out totalData);
            Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
        }
    }
}