namespace Fanex.Athena.Service.Report.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess.Report.Interfaces;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report;

    public class DailyRacingWinLossServiceTest
    {
        private IDailyRacingWinLossDataAccess _dataAccess;
        private DailyRacingWinLossService _service;

        [SetUp]
        public void SetUp()
        {
            _dataAccess = Substitute.For<IDailyRacingWinLossDataAccess>();
            _service = new DailyRacingWinLossService(_dataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _dataAccess = null;
            _service = null;
        }

        [Test]
        public void GetDataList_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var metaModel = new DailyRacingWinLossMetaModel();
            var dataList = new List<DailyRacingWinLoss>();
            dataList.Add(new DailyRacingWinLoss());

            _dataAccess.GetDataList(metaModel).ReturnsForAnyArgs(dataList);

            // Action
            var result = _service.GetDataList(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.Received().GetDataList(metaModel);
                Assert.That(result.Count, Is.EqualTo(dataList.Count));
            });
        }

        [Test]
        public void GetDailyRacingWinLossData_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var metaModel = new DailyRacingWinLossMetaModel();
            var dataList = new List<DailyRacingWinLoss>();
            dataList.Add(new DailyRacingWinLoss
            {
                CurrencyName = "uus",
                WinTO = 1
            });
            dataList.Add(new DailyRacingWinLoss
            {
                CurrencyName = "uus",
                WinTO = 2
            });

            _dataAccess.GetDataList(metaModel).ReturnsForAnyArgs(dataList);

            // Action
            var result = _service.GetDailyRacingWinLossData(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.Received().GetDataList(metaModel);
                Assert.That(result.ContentData.Count, Is.EqualTo(1));
                Assert.That(result.ContentTotalData.Values.First().WinTO, Is.EqualTo(3));
                Assert.That(result.CurrencyTotalData.Values.First().WinTO, Is.EqualTo(3));
                Assert.That(result.GrandTotalData.WinTO, Is.EqualTo(3));
            });
        }
    }
}