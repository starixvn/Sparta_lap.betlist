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

    public class ThirdPartyWinLossServiceTest
    {
        private IThirdPartyWinLossDataAccess _dataAccess;
        private ThirdPartyWinLossService _service;

        [SetUp]
        public void SetUp()
        {
            _dataAccess = Substitute.For<IThirdPartyWinLossDataAccess>();
            _service = new ThirdPartyWinLossService(_dataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _dataAccess = null;
            _service = null;
        }

        [Test]
        public void GetAgentWinlossDetail_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var metaModel = new ThirdPartyWinLossMetaModel();

            // Action
            _service.GetAgentWinlossDetail(metaModel, string.Empty);

            // Assert
            _dataAccess.Received().GetAgentWinlossDetail(metaModel, string.Empty);
        }

        [Test]
        public void GetCustomerWinlossDetail_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var metaModel = new ThirdPartyWinLossMetaModel();

            // Action
            _service.GetCustomerWinlossDetail(metaModel, string.Empty);

            // Assert
            _dataAccess.Received().GetCustomerWinlossDetail(metaModel, string.Empty);
        }

        [Test]
        public void GetSummaryWinloss_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var metaModel = new ThirdPartyWinLossMetaModel();

            var dataList = new SortableList<ThirdPartyWinLoss>
            {
                new ThirdPartyWinLoss { Date = new DateTime(2017, 1, 1) },
                new ThirdPartyWinLoss { Date = new DateTime(2017, 1, 1) },
                new ThirdPartyWinLoss { Date = null },
            };
            _dataAccess.GetSummaryWinloss(metaModel).Returns(new SortableReportResultModel<ThirdPartyWinLoss> { DataList = dataList });

            var expectedRowSpan = new[] { 2, -1, 1 };

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
        public void TotalCalc_NoData_ReturnEmptyList()
        {
            // Arrange
            var dataList = Enumerable.Empty<ThirdPartyWinLoss>();

            // Action
            var result = _service.TotalCalc(dataList);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void TotalCalc_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var dataList = new List<ThirdPartyWinLoss>
            {
                new ThirdPartyWinLoss { CurrencyName = "1" },
                new ThirdPartyWinLoss { CurrencyName = "1" },
                new ThirdPartyWinLoss { CurrencyName = "2" }
            };

            const int ExpectedTotal = 2;

            // Action
            var result = _service.TotalCalc(dataList);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(ExpectedTotal));
        }
    }
}