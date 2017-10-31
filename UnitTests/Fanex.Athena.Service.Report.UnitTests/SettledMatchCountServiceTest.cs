namespace Fanex.Athena.Service.Report.UnitTests
{
    using System.Collections.Generic;
    using DataAccess.Report.Interfaces;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Model.Report.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Report;

    public class SettledMatchCountServiceTest
    {
        private ISettledMatchCountDataAccess _dataAccess;
        private SettledMatchCountService _service;

        [SetUp]
        public void SetUp()
        {
            _dataAccess = Substitute.For<ISettledMatchCountDataAccess>();
            _service = new SettledMatchCountService(_dataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _dataAccess = null;
            _service = null;
        }

        [Test]
        public void GetDataList_WhenCall_ReturnEmptyData()
        {
            // Arrange
            var productList = new List<Product>
            {
                new Product
                {
                    ProductCode = Product.RB
                },
                new Product
                {
                    ProductCode = Product.LC
                }
            };
            var productAccess = new ProductAccess(productList);
            var dataList = new List<SettledMatchCount>();
            dataList.Add(new SettledMatchCount
            {
                SportType = (int)SportType.LiveCasino
            });
            dataList.Add(new SettledMatchCount
            {
                SportType = (int)SportType.HorseRacing,
                IsToday = 1
            });

            _dataAccess.GetDataList(false, 0).ReturnsForAnyArgs(dataList);

            // Action
            var result = _service.GetDataList(false, 0, productAccess);

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.Received().GetDataList(false, 0);
                Assert.That(result, Is.Not.Empty);
            });
        }
    }
}