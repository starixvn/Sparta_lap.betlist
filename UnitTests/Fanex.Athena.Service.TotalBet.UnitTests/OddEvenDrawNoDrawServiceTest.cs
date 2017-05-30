namespace Fanex.Athena.Service.TotalBet.UnitTests
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
    public class OddEvenDrawNoDrawServiceTest
    {
        private IOddEvenDrawNoDrawDataAccess _dataAccess;

        private IOddEvenDrawNoDrawService _service;

        [OneTimeSetUp]
        public void SetUp()
        {
            _dataAccess = Substitute.For<IOddEvenDrawNoDrawDataAccess>();
            _service = new OddEvenDrawNoDrawService(_dataAccess);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _dataAccess = null;
            _service = null;
        }

        [Test]
        public void GetOddEvenDrawNoDrawDataList_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = new List<OddEvenDrawNoDraw>
            {
                new OddEvenDrawNoDraw()
            };
            OddEvenDrawNoDraw totalData;
            _dataAccess.GetOddEventDrawNoDrawDataList(Arg.Any<OddEvenDrawNoDrawMetaModel>(), out totalData)
                .Returns(expectedResult);

            // Action
            var result = _service.GetOddEvenDrawNoDrawDataList(new OddEvenDrawNoDrawMetaModel(), out totalData);

            // Assert
            _dataAccess.ReceivedWithAnyArgs().GetOddEventDrawNoDrawDataList(Arg.Any<OddEvenDrawNoDrawMetaModel>(), out totalData);
            Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
        }
    }
}