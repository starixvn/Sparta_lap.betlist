namespace Fanex.Athena.Service.Report.UnitTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess.Report.Interfaces;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class WeeklyLiveMatchServiceTest
    {
        private IWeeklyLiveMatchDataAccess _dataAccess;
        private WeeklyLiveMatchService _service;

        [SetUp]
        public void SetUp()
        {
            _dataAccess = Substitute.For<IWeeklyLiveMatchDataAccess>();
            _service = new WeeklyLiveMatchService(_dataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _dataAccess = null;
            _service = null;
        }

        [Test]
        public async Task GetData_InvalidMetaModel_ReturnEmptyData()
        {
            // Arrange
            var metaModel = new WeeklyLiveMatchMetaModel { FromDate = new System.DateTime(2017, 2, 1), ToDate = new System.DateTime(2017, 1, 1) };

            // Action
            var result = await _service.GetData(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList, Is.Empty);
                Assert.That(result.TotalData.Count, Is.Zero);
                _dataAccess.DidNotReceive().GetDataList(metaModel);
            });
        }

        [Test]
        public void GetData_ValidMetaModel_ReturnDataFromDataAccess()
        {
            // Arrange
            var metaModel = new WeeklyLiveMatchMetaModel();

            // Action
            var result = _service.GetData(metaModel);

            // Assert
            _dataAccess.Received().GetDataList(metaModel);
        }

        [Test]
        public void GetWeekHeaders_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var fromDate = new DateTime(2018, 1, 9);
            var toDate = new DateTime(2018, 1, 26);
            const int NumberOfWeeks = 3;

            var expected = new[]
            {
                "09 Jan - 14 Jan 2018",
                "15 Jan - 21 Jan 2018",
                "22 Jan - 26 Jan 2018"
            };

            // Action
            var result = _service.GetWeekHeaders(fromDate, toDate, NumberOfWeeks);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.ElementAt(0), Is.EqualTo(expected[0]));
                Assert.That(result.ElementAt(1), Is.EqualTo(expected[1]));
                Assert.That(result.ElementAt(2), Is.EqualTo(expected[2]));
                Assert.That(result.Count, Is.EqualTo(NumberOfWeeks));
            });            
        }
    }
}