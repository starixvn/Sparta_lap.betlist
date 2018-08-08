namespace Fanex.Athena.Service.Report.UnitTests
{
    using System.Collections.Generic;
    using System.Data;
    using Constant.DataDomain;
    using DataAccess.Report.Interfaces;
    using Model.Core.DTO.Common;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report;
    using VendorAdapter.Interfaces;

    public class MatchWinLossServiceTest
    {
        private IMatchWinLossDataAccess _dataAccess;
        private MatchWinLossService _service;

        [SetUp]
        public void SetUp()
        {
            _dataAccess = Substitute.For<IMatchWinLossDataAccess>();
            _service = new MatchWinLossService(_dataAccess, Substitute.For<ICacheRefAdapter>());
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
            var metaModel = new MatchWinLossMetaModel();
            var dataList = new List<MatchWinLoss>
            {
                new MatchWinLoss { ProductName = Product.SB, IsNeutral = true },
                new MatchWinLoss { ProductName = Product.VS2 },
                new MatchWinLoss { ProductName = Product.VS, SportType = (int)SportType.VirtualCycling },
                new MatchWinLoss { ProductName = Product.NG }
            };

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
        public void GetDetailDataSet_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var metaModel = new MatchWinLossMetaModel();
            var dataSet = new DataSet();

            _dataAccess.GetDetailDataSet(metaModel).ReturnsForAnyArgs(dataSet);

            // Action
            var result = _service.GetDetailDataSet(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.Received().GetDetailDataSet(metaModel);
                Assert.That(result, Is.EqualTo(dataSet));
            });
        }

        [Test]
        public void GetDetailDataList_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var metaModel = new MatchWinLossMetaModel();
            var dataList = new List<MatchWinLoss>();
            dataList.Add(new MatchWinLoss());

            _dataAccess.GetDetailDataList(metaModel).ReturnsForAnyArgs(dataList);

            // Action
            var result = _service.GetDetailDataList(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.Received().GetDetailDataList(metaModel);
                Assert.That(result, Is.EqualTo(dataList));
            });
        }

        [Test]
        public void GetOutrightParlayList_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var metaModel = new MatchWinLossMetaModel();
            var dataList = new List<MatchWinLoss>();
            dataList.Add(new MatchWinLoss());

            _dataAccess.GetOutrightParlayList(metaModel).ReturnsForAnyArgs(dataList);

            // Action
            var result = _service.GetOutrightParlayList(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.Received().GetOutrightParlayList(metaModel);
                Assert.That(result, Is.EqualTo(dataList));
            });
        }
    }
}