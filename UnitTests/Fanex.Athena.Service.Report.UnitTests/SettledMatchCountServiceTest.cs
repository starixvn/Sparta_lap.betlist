namespace Fanex.Athena.Service.Report.UnitTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Constant.DataDomain;
    using DataAccess.Report.Interfaces;
    using Model.Core.DTO.Common;
    using Model.Report.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Report;
    using VendorAdapter.Interfaces;

    public class SettledMatchCountServiceTest
    {
        private ISettledMatchCountDataAccess _dataAccess;
        private SettledMatchCountService _service;

        [SetUp]
        public void SetUp()
        {
            _dataAccess = Substitute.For<ISettledMatchCountDataAccess>();
            _service = new SettledMatchCountService(_dataAccess, Substitute.For<ICacheRefAdapter>());
        }

        [TearDown]
        public void TearDown()
        {
            _dataAccess = null;
            _service = null;
        }

        [Test]
        public async Task GetDataList_SportIsNotAllowed_ReturnEmptyData()
        {
            // Arrange
            var productList = new List<Product>
            {
                new Product
                {
                    ProductCode = Product.SB
                }
            };
            var productAccess = new ProductAccess(productList);
            var dataList = new List<SettledMatchCount>();
            dataList.Add(new SettledMatchCount
            {
                SportType = (int)SportType.NumberGame
            });

            _dataAccess.GetDataList(false, 0).ReturnsForAnyArgs(dataList);

            // Action
            var result = await _service.GetDataList(false, 0, productAccess);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetDataList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var productList = new List<Product>
            {
                new Product { ProductCode = Product.NG },
                new Product { ProductCode = Product.VS }
            };
            var productAccess = new ProductAccess(productList);
            var dataList = new List<SettledMatchCount>
            {
                new SettledMatchCount { SportType = (int)SportType.NumberGame, IsToday = 0, Entered = 1 },
                new SettledMatchCount { SportType = (int)SportType.NumberGame, IsToday = 0, NonEntered = 1 },
                new SettledMatchCount { SportType = (int)SportType.LiveCasino, IsToday = 1, Entered = 1 },
                new SettledMatchCount { SportType = (int)SportType.VirtualSport, IsToday = 1, NonEntered = 1 },
                new SettledMatchCount { SportType = (int)SportType.AllBet, IsToday = 1, NonEntered = 1 },
                new SettledMatchCount { SportType = (int)SportType.AllBet, IsToday = 0, NonEntered = 1 }
            };

            _dataAccess.GetDataList(false, 0).ReturnsForAnyArgs(dataList);

            const int ExpectedToDayRecords = 3;

            // Action
            var result = await _service.GetDataList(false, 0, productAccess);

            // Assert
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(ExpectedToDayRecords));
        }
    }
}