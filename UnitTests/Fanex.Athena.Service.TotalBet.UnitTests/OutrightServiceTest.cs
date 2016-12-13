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
    public class OutrightServiceTest
    {
        private IOutrightDataAccess _dataAccess;

        private IOutrightService _service;

        [OneTimeSetUp]
        public void SetUp()
        {
            _dataAccess = Substitute.For<IOutrightDataAccess>();
            _service = new OutrightService(_dataAccess);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _dataAccess = null;
            _service = null;
        }

        [Test]
        public void GetOutrightDataList_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = new List<Outright>
            {
                new Outright()
            };
            _dataAccess.GetOutrightDataList(Arg.Any<OutrightMetaModel>())
                .Returns(expectedResult);

            // Action
            var result = _service.GetOutrightDataList(new OutrightMetaModel());

            // Assert
            _dataAccess.ReceivedWithAnyArgs().GetOutrightDataList(Arg.Any<OutrightMetaModel>());
            Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
        }
    }
}