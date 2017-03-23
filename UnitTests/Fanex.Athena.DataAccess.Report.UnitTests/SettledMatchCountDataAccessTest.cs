namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System.Collections.Generic;
    using System.Data;
    using System.Web.Script.Serialization;
    using Data;
    using Model.Report.DTO;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SettledMatchCountDataAccessTest
    {
        private SettledMatchCountDataAccess _dataAccess;

        [OneTimeSetUp]
        public void Setup()
        {
            _dataAccess = Substitute.ForPartsOf<SettledMatchCountDataAccess>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _dataAccess = null;
        }

        [Test]
        public void GetDataList_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_MatchSelResultEntered")).Returns(objectDb);
            dynamic uspParam = null;
            objectDb.Query<SettledMatchCount>(Arg.Do<dynamic>(arg => uspParam = arg));
            var expectedResult = "{\"isShowAll\":true,\"licenseeid\":1}";

            // Action
            _dataAccess.GetDataList(true, 1);

            // Assert
            Assert.That(new JavaScriptSerializer().Serialize(uspParam), Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetDataList_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_MatchSelResultEntered")).Returns(objectDb);
            var expectedReturnedData = new List<SettledMatchCount>
            {
                new SettledMatchCount
                {
                    SportType = 1,
                    Entered = 2
                }
            };

            objectDb.Query<SettledMatchCount>(Arg.Any<object>()).ReturnsForAnyArgs(expectedReturnedData);

            // Action
            var result = _dataAccess.GetDataList(Arg.Any<bool>(), Arg.Any<int>());

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