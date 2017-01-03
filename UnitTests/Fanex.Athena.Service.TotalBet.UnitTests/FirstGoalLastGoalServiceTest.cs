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
    public class FirstGoalLastGoalServiceTest
    {
        private IFirstGoalLastGoalDataAccess _dataAccess;

        private IFirstGoalLastGoalService _service;

        [OneTimeSetUp]
        public void SetUp()
        {
            _dataAccess = Substitute.For<IFirstGoalLastGoalDataAccess>();
            _service = new FirstGoalLastGoalService(_dataAccess);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _dataAccess = null;
            _service = null;
        }

        [Test]
        public void GetFirstGoalLastGoalDataList_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = new List<FirstGoalLastGoal>
            {
                new FirstGoalLastGoal()
            };
            FirstGoalLastGoal totalData;
            _dataAccess.GetFirstGoalLastGoalDataList(Arg.Any<FirstGoalLastGoalMetaModel>(), out totalData)
                .Returns(expectedResult);

            // Action
            var result = _service.GetFirstGoalLastGoalDataList(new FirstGoalLastGoalMetaModel(), out totalData);

            // Assert
            _dataAccess.ReceivedWithAnyArgs().GetFirstGoalLastGoalDataList(Arg.Any<FirstGoalLastGoalMetaModel>(), out totalData);
            Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
        }
    }
}