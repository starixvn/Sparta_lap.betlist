namespace Fanex.Athena.Service.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess.Report.Interfaces;
    using Model.Core.DTO.Common;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report;

    public class CustomerTurnoverServiceTest
    {
        private readonly IDictionary<int, string> _currencyList = new Dictionary<int, string> { { 20, "UUS" } };
        private ICustomerTurnoverDataAccess _dataAccess;
        private CustomerTurnoverService _service;

        [SetUp]
        public void SetUp()
        {
            _dataAccess = Substitute.For<ICustomerTurnoverDataAccess>();
            _service = new CustomerTurnoverService(_dataAccess);
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
            var metaModel = new CustomerTurnoverMetaModel();

            // Action
            var result = _service.GetDataList(metaModel, _currencyList, string.Empty);

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.DidNotReceiveWithAnyArgs().GetDataList(metaModel, _currencyList, string.Empty);
                Assert.That(result.DataList, Is.Empty);
            });
        }

        [Test]
        public void GetDataList_ValidModel_NoData_ReturnEmptyData()
        {
            // Arrange
            var metaModel = new CustomerTurnoverMetaModel
            {
                FromDate = new DateTime(2017, 1, 1),
                ToDate = new DateTime(2017, 2, 2),
                SportProducts = new List<SportProductItem>
                {
                    new SportProductItem { Checked = true }
                },
                Bettypes = new List<CheckedItem>
                {
                    new CheckedItem { Checked = true }
                }
            };

            _dataAccess.GetDataList(metaModel, _currencyList, string.Empty).ReturnsForAnyArgs(new ReportResultModel<CustTurnover>
            {
                DataList = new List<CustTurnover>()
            });

            // Action
            var result = _service.GetDataList(metaModel, _currencyList, string.Empty);

            // Assert
            Assert.That(result.DataList, Is.Empty);
        }

        [Test]
        public void GetDataList_ValidMetaModel_ReturnDataFromDataAccess()
        {
            // Arrange
            var metaModel = new CustomerTurnoverMetaModel
            {
                ShowDraw = true,
                FromDate = new DateTime(2017, 1, 1),
                ToDate = new DateTime(2017, 2, 2),
                SportProducts = new List<SportProductItem>
                {
                    new SportProductItem { Checked = true }
                },
                Bettypes = new List<CheckedItem>
                {
                    new CheckedItem { Checked = true }
                }
            };
            var dataList = new List<CustTurnover>
            {
                new CustTurnover { UserName = "1" },
                new CustTurnover { UserName = "1" },
                new CustTurnover { UserName = "2" },
                new CustTurnover { UserName = "2" },
                new CustTurnover { UserName = "3" }
            };
            var expectedResult = new ReportResultModel<CustTurnover>
            {
                DataList = dataList,
                TotalData = new CustTurnover
                {
                    CustomerComm = 123
                },
                TotalRows = 999
            };

            _dataAccess.GetDataList(metaModel, _currencyList, string.Empty).ReturnsForAnyArgs(expectedResult);

            // Action
            var result = _service.GetDataList(metaModel, _currencyList, string.Empty);

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.Received().GetDataList(metaModel, _currencyList, string.Empty);
                Assert.That(result.DataList.Count, Is.EqualTo(dataList.Count));
                Assert.That(result.TotalData, Is.EqualTo(expectedResult.TotalData));
                Assert.That(result.TotalRows, Is.EqualTo(expectedResult.TotalRows));
            });
        }
    }
}