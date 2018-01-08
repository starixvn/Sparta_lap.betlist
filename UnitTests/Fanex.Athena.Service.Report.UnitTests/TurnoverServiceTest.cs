namespace Fanex.Athena.Service.Report.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess.Report.Interfaces;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class TurnoverServiceTest
    {
        private ITurnoverDataAccess _dataAccess;
        private TurnoverService _service;

        [SetUp]
        public void SetUp()
        {
            _dataAccess = Substitute.For<ITurnoverDataAccess>();
            _service = new TurnoverService(_dataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _dataAccess = null;
            _service = null;
        }

        [Test]
        public void GetDataList_InvalidMetaModel_ReturnEmptyData()
        {
            // Arrange
            var metaModel = new TurnoverMetaModel { Year = 2018, Month = 2, FromDay = 2, ToDay = 1 };

            // Action
            var result = _service.GetDataList(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Empty);
                _dataAccess.DidNotReceive().GetDataList(metaModel);
            });
        }

        [Test]
        public void GetDataList_ValidMetaModel_ReturnDataFromDataAccess()
        {
            // Arrange
            var metaModel = new TurnoverMetaModel();

            // Action
            var result = _service.GetDataList(metaModel);

            // Assert
            _dataAccess.Received().GetDataList(metaModel);
        }

        [Test]
        public void SumTotalData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var dataList = new List<TurnoverDTO>
            {
                new TurnoverDTO { Deposit = 1 },
                new TurnoverDTO { Deposit = 2 },
                new TurnoverDTO { Deposit = 3 }
            };

            // Action
            var result = _service.SumTotalData(dataList);

            // Assert
            Assert.That(result.Deposit, Is.EqualTo(dataList.Sum(p => p.Deposit)));
        }
    }
}