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
    public class MoneyLineServiceTest
    {
        private IMoneyLineDataAccess _dataAccess;

        private IMoneyLineService _service;

        [OneTimeSetUp]
        public void SetUp()
        {
            _dataAccess = Substitute.For<IMoneyLineDataAccess>();
            _service = new MoneyLineService(_dataAccess);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _dataAccess = null;
            _service = null;
        }

        [Test]
        public void GetMoneyLineDataList_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = new List<MoneyLine>
            {
                new MoneyLine()
            };
            MoneyLine totalData;
            _dataAccess.GetMoneyLineDataList(Arg.Any<MoneyLineMetaModel>(), out totalData)
                .Returns(expectedResult);

            // Action
            var result = _service.GetMoneyLineDataList(new MoneyLineMetaModel(), out totalData);

            // Assert
            _dataAccess.ReceivedWithAnyArgs().GetMoneyLineDataList(Arg.Any<MoneyLineMetaModel>(), out totalData);
            Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
        }
    }
}