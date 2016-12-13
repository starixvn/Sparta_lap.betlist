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
    public class TotalGoalServiceTest
    {
        private ITotalGoalDataAccess _dataAccess;

        private ITotalGoalService _service;

        [OneTimeSetUp]
        public void SetUp()
        {
            _dataAccess = Substitute.For<ITotalGoalDataAccess>();
            _service = new TotalGoalService(_dataAccess);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _dataAccess = null;
            _service = null;
        }

        [Test]
        public void GetTotalGoalDataList_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = new List<TotalGoal>
            {
                new TotalGoal()
            };
            TotalGoal totalData;
            _dataAccess.GetTotalGoalDataList(Arg.Any<TotalGoalMetaModel>(), out totalData)
                .Returns(expectedResult);

            // Action
            var result = _service.GetTotalGoalDataList(new TotalGoalMetaModel(), out totalData);

            // Assert
            _dataAccess.ReceivedWithAnyArgs().GetTotalGoalDataList(Arg.Any<TotalGoalMetaModel>(), out totalData);
            Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
        }
    }
}