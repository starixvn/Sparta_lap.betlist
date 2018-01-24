namespace Fanex.Athena.Service.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class ColossusBetsWinLossServiceTest
    {
        private IColossusBetsWinLossDataAccess _dataAccess;
        private ColossusBetsWinLossService _service;

        [SetUp]
        public void SetUp()
        {
            _dataAccess = Substitute.For<IColossusBetsWinLossDataAccess>();
            _service = new ColossusBetsWinLossService(_dataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _dataAccess = null;
            _service = null;
        }

        [Test]
        public void GetSummaryWinloss_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var metaModel = new ThirdPartyWinLossMetaModel();

            var dataList = new SortableList<ColossusBetWinLoss>
            {
                new ColossusBetWinLoss { Date = new DateTime(2017, 1, 1) },
                new ColossusBetWinLoss { Date = new DateTime(2017, 1, 1) },
                new ColossusBetWinLoss { Date = new DateTime(2017, 1, 2) },
            };
            _dataAccess.GetSummaryWinloss(metaModel).Returns(new SortableReportResultModel<ColossusBetWinLoss> { DataList = dataList });

            var expectedRowSpan = new int[] { 2, -1, 1 };

            // Action
            var result = _service.GetSummaryWinloss(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList.Count, Is.EqualTo(dataList.Count));
                Assert.That(result.DataList[0].RowSpan, Is.EqualTo(expectedRowSpan[0]));
                Assert.That(result.DataList[1].RowSpan, Is.EqualTo(expectedRowSpan[1]));
                Assert.That(result.DataList[2].RowSpan, Is.EqualTo(expectedRowSpan[2]));
            });
        }

        [Test]
        public void GetWinlossDetail_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var metaModel = new ThirdPartyWinLossMetaModel();

            // Action
            _service.GetWinlossDetail(metaModel);

            // Assert
            _dataAccess.Received().GetWinlossDetail(metaModel);
        }

        [Test]
        public void CalculateTotal_NoData_ReturnEmptyList()
        {
            // Arrange
            var dataList = Enumerable.Empty<ColossusBetWinLoss>();

            // Action
            var result = _service.CalculateTotal(dataList);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void CalculateTotal_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var dataList = new List<ColossusBetWinLoss>
            {
                new ColossusBetWinLoss { CurrencyName = "1" },
                new ColossusBetWinLoss { CurrencyName = "1" },
                new ColossusBetWinLoss { CurrencyName = "2" }
            };

            const int ExpectedTotal = 2;

            // Action
            var result = _service.CalculateTotal(dataList);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(ExpectedTotal));
        }
    }
}