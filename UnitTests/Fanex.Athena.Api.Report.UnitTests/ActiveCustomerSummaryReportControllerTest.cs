namespace Fanex.Athena.Api.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Base.UnitTests;
    using DataAccess.Report.Interfaces;
    using Excel.Base.Entities;
    using Excel.Report;
    using Extension.Helper;
    using Extension.Utility;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Model.Core.MetaModel.Product;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Model.Report.ResultModel;
    using Model.Report.ViewModel;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;
    using Session.Enum;
    using Session.Model;

    public class ActiveCustomerSummaryReportControllerTest : BaseApiControllerHelperTest
    {
        private IActiveCustomerSummaryDataAccess _dataAccess;
        private ActiveCustomerSummaryReportController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _dataAccess = Substitute.For<IActiveCustomerSummaryDataAccess>();

            _controller = Substitute.ForPartsOf<ActiveCustomerSummaryReportController>(_dataAccess, SessionService, ProductService, LangService);

            _controller.When(x => x.ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>())).DoNotCallBase();

            MockSessionData();
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _controller = null;
            _dataAccess = null;
        }

        [Test]
        public void Index_InvalidRequest_ReturnEmptyData()
        {
            // Arrange
            var model = new ActiveCustomerMetaModel
            {
                CurrencyId = 1,
                ProductOptions = new List<ProductOption>()
            };
            StubUserSessionData(new UserSession
            {
                CurrencyList = new Dictionary<int, string>(),
                ApplicationMode = AppMode.Self,
                FunctionIdList = new int[0]
            });
            StubLicenseeSessionData(new LicenseeSession { LicenseeId = 0 });

            // Action
            var result = _controller.Index(model);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.IsEmpty(result.Data);
                Assert.That(result.GrantTotalCustomer, Is.EqualTo(0));
                Assert.That(result.ProductOptions, Is.EqualTo(model.ProductOptions.ToJson()));
            });
        }

        [Test]
        public void Index_InvalidModel_ReturnEmptyData()
        {
            // Arrange
            var model = CreateValidMetaModel();
            model.FromDate = model.ToDate.AddDays(1);

            // Action
            var result = _controller.Index(model);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.IsEmpty(result.Data);
                Assert.That(result.GrantTotalCustomer, Is.EqualTo(0));
                Assert.That(result.ProductOptions, Is.EqualTo(model.ProductOptions.ToJson()));
            });
        }

        [Test]
        public void Index_NoDataReturnedFromDataAccess_ReturnEmptyData()
        {
            // Arrange
            var metaModel = CreateValidMetaModel();

            _dataAccess.GetActiveCustomers(Arg.Any<ActiveCustomerMetaModel>()).Returns(new ActiveCustomerSummaryResultModel
            {
                DataList = new SortableList<ActiveCustomer>()
            });

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.IsEmpty(result.Data);
                Assert.That(result.GrantTotalCustomer, Is.EqualTo(0));
                Assert.That(result.ProductOptions, Is.EqualTo(metaModel.ProductOptions.ToJson()));
            });
        }

        [Test]
        public void Index_ValidModelNoSorting_GetCorrectDataFromDataAccess()
        {
            // Arrange
            var metaModel = CreateValidMetaModel();
            var expectedResult = SetUpForGettingData(metaModel);

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.That(result.Data, Is.EqualTo(expectedResult.DataList));
            Assert.That(result.GrantTotalCustomer, Is.EqualTo(expectedResult.TotalCustomer));
        }

        [Test]
        public void Index_ValidModelSortByTotalCustomer_GetCorrectDataFromDataAccess()
        {
            // Arrange
            var sortBy = nameof(ActiveCustomer.TotalCustomer);
            var sortOrder = "asc";
            var metaModel = CreateValidMetaModel(sortBy: sortBy, sortOrder: sortOrder);
            var expectedResult = SetUpForGettingData(metaModel);
            expectedResult.DataList.Sort(sortBy, sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase));

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.That(result.Data, Is.EqualTo(expectedResult.DataList));
            Assert.That(result.GrantTotalCustomer, Is.EqualTo(expectedResult.TotalCustomer));
        }

        [Test]
        public void Index_ValidModelSortByCustomerCount_GetCorrectDataFromDataAccess()
        {
            // Arrange
            var sortBy = nameof(ActiveCustomer.CustomerCount) + "A";
            var sortOrder = "asc";
            var metaModel = CreateValidMetaModel(sortBy: sortBy, sortOrder: sortOrder);
            var expectedResult = SetUpForGettingData(metaModel);
            expectedResult.DataList.SortByDictionary("A", nameof(ActiveCustomer.CustomerCount), sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase));

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.That(result.Data, Is.EqualTo(expectedResult.DataList));
            Assert.That(result.GrantTotalCustomer, Is.EqualTo(expectedResult.TotalCustomer));
        }

        [TestCase(new[] { Product.BA }, Product.BA, true, false)]
        [TestCase(new[] { Product.BA }, Product.SB, false, false)]
        public void Index_ExportToExcel_ExportWithCorrectData(string[] productCodeOption, string productCode, bool visibleBa, bool visibleRb)
        {
            // Arrange
            var metaModel = CreateValidMetaModel(productCodeOption);
            metaModel.IsExportExcel = true;

            var expectedResult = SetUpForGettingData(metaModel);

            ProductService.GetProductAccess(Arg.Any<int>(), Arg.Any<string>()).Returns(
                new ProductAccess(new List<Product> { new Product { ProductCode = productCode } }));

            var actualViewModel = new ActiveCustomerViewModel();
            var actualShowingOption = new ShowingOption();
            var excel = Substitute.ForPartsOf<ActiveCustomerSummaryExcel>(
                   Arg.Any<string>(),
                   Arg.Any<ConvertingOption>(),
                   Arg.Do<ShowingOption>(x => actualShowingOption = x),
                   new ActiveCustomerMetaModel(),
                   new ActiveCustomerViewModel
                   {
                       MainResult = new ActiveCustomer[] { }
                   });
            excel.When(x => x.Export()).DoNotCallBase();

            _controller.CreateExcel(
                  Arg.Any<string>(),
                  Arg.Any<ConvertingOption>(),
                  Arg.Any<ShowingOption>(),
                  new ActiveCustomerMetaModel(),
                  new ActiveCustomerViewModel()).ReturnsForAnyArgs(excel);
            _controller.When(x => x.CreateExcel(
                   Arg.Any<string>(),
                   Arg.Any<ConvertingOption>(),
                   Arg.Any<ShowingOption>(),
                   Arg.Any<ActiveCustomerMetaModel>(),
                   Arg.Any<ActiveCustomerViewModel>())).Do(p => actualViewModel = (ActiveCustomerViewModel)p[4]);

            // Action
            _controller.Index(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualViewModel.MainResult, Is.EqualTo(expectedResult.DataList.ToList()));
                Assert.That(actualShowingOption.BAVisible, Is.EqualTo(visibleBa));
                Assert.That(actualShowingOption.RBVisible, Is.EqualTo(visibleRb));
            });
        }

        private ActiveCustomerMetaModel CreateValidMetaModel(string[] productCodes = null, string sortBy = null, string sortOrder = "asc")
        {
            var model = new ActiveCustomerMetaModel
            {
                CurrencyId = 1,
                AgentListXml = string.Empty,
                FromDate = DateTime.Now.AddDays(-2),
                ToDate = DateTime.Now.AddDays(-1),
                ProductOptions = new List<ProductOption>(),
                SortBy = sortBy,
                SortOrder = sortOrder
            };

            if (productCodes != null)
            {
                foreach (var code in productCodes)
                {
                    model.ProductOptions.Add(new ProductOption
                    {
                        Code = code,
                        Name = code,
                        Status = true
                    });
                }
            }
            else
            {
                model.ProductOptions.Add(new ProductOption
                {
                    Code = Product.BA,
                    Name = Product.BA,
                    Status = true
                });
            }

            StubUserSessionData(new UserSession
            {
                CurrencyList = new Dictionary<int, string>
                {
                    { model.CurrencyId, nameof(model.CurrencyId) }
                },
                ApplicationMode = AppMode.Self,
                FunctionIdList = new int[0]
            });
            StubLicenseeSessionData(new LicenseeSession { LicenseeId = 0 });
            SessionService.GenerateAgentListXml(Arg.Any<int>(), Arg.Any<bool>()).Returns(string.Empty);

            return model;
        }

        private ActiveCustomerSummaryResultModel SetUpForGettingData(ActiveCustomerMetaModel metaModel)
        {
            var unsortedData = new SortableList<ActiveCustomer>();

            var random = new Random();
            var total = random.Next(5, 10);
            var customerCount = new[] { "A", "B", "C", "D" };

            for (int i = 0; i < total; i++)
            {
                var item = new ActiveCustomer
                {
                    TransactionDate = DateTime.Today.AddDays(random.Next(1, 10)),
                    TotalCustomer = random.Next(1, 10),
                    CustomerCount = new Dictionary<string, int>()
                };

                for (int j = 0; j < customerCount.Count(); j++)
                {
                    var value = random.Next(1, 10);
                    item.CustomerCount.Add(customerCount[j], value);
                }

                unsortedData.Add(item);
            }

            var expectResult = new ActiveCustomerSummaryResultModel
            {
                TotalCustomer = unsortedData.Sum(y => y.TotalCustomer),
                DataList = unsortedData
            };

            _dataAccess.GetActiveCustomers(metaModel).Returns(expectResult);

            return expectResult;
        }
    }
}