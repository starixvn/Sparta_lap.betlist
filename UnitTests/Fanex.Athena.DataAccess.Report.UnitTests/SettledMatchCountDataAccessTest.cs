namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System.Collections.Generic;
    using Core.UnitTests.TestHelpers;
    using Model.Report.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.NUnitExtensions;

    [TestFixture]
    public class SettledMatchCountDataAccessTest : BaseDataAccessHelperTest<SettledMatchCountDataAccess>
    {
        private const string SpName = "Lrf_Rpt_MatchSelResultEntered";

        [Test]
        public void GetDataList_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);
            dynamic uspParam = null;
            ObjectDb.Query<SettledMatchCount>(Arg.Do<dynamic>(arg => uspParam = arg));
            var expectedResult = new
            {
                isShowAll = true,
                licenseeid = 1
            };

            // Action
            DataAccess.GetDataList(true, 1);

            // Assert
            Assert.That(uspParam, Tobe.SameProperties(expectedResult));
        }

        [Test]
        public void GetDataList_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);
            var expectedReturnedData = new List<SettledMatchCount>
            {
                new SettledMatchCount
                {
                    SportType = 1,
                    Entered = 2
                }
            };

            ObjectDb.Query<SettledMatchCount>(Arg.Any<object>()).ReturnsForAnyArgs(expectedReturnedData);

            // Action
            var result = DataAccess.GetDataList(Arg.Any<bool>(), Arg.Any<int>());

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(1));
                Assert.That(result[0].SportType, Is.EqualTo(1));
                Assert.That(result[0].Entered, Is.EqualTo(2));
            });
        }
    }
}