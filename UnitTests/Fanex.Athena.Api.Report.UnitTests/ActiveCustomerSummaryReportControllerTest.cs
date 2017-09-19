namespace Fanex.Athena.Api.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
    using Model.Report.ViewModel;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;

    public class ActiveCustomerSummaryReportControllerTest : ApiReportBaseControllerTest
    {
        private IActiveCustomerSummaryDataAccess _dataAccess;
        private ActiveCustomerSummaryReportController _controller;

        [OneTimeSetUp]
        public override void SetUp()
        {
            base.SetUp();

            _dataAccess = Substitute.For<IActiveCustomerSummaryDataAccess>();

            _controller = Substitute.ForPartsOf<ActiveCustomerSummaryReportController>(_dataAccess, SessionService, ProductService);

            _controller.When(x => x.ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>())).DoNotCallBase();

            MockSessionData();
        }

        [OneTimeTearDown]
        public override void TearDown()
        {
            base.TearDown();

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
            MockUserSessionData(new UserSession
            {
                CurrencyList = new Dictionary<int, string>(),
                ApplicationMode = AppMode.Self,
                FunctionIdList = new List<int>()
            });
            MockLicenseeSessionData(new LicenseeSession { LicenseeId = 0 });

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

            int totalCustomer;
            _dataAccess.GetActiveCustomers(Arg.Any<ActiveCustomerMetaModel>(), out totalCustomer).Returns(x =>
            {
                return new SortableList<ActiveCustomer>();
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
            int totalCustomer;
            var metaModel = CreateValidMetaModel();
            var expectedUnSortedData = SetUpForGettingData(metaModel, out totalCustomer);

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.That(result.Data, Is.EqualTo(expectedUnSortedData));
        }

        [Test]
        public void Index_ValidModelSortByTotalCustomer_GetCorrectDataFromDataAccess()
        {
            // Arrange
            int totalCustomer;
            var sortBy = nameof(ActiveCustomer.TotalCustomer);
            var sortOrder = "asc";
            var metaModel = CreateValidMetaModel(sortBy, sortOrder);
            var expectedData = SetUpForGettingData(metaModel, out totalCustomer);
            expectedData.Sort(sortBy, sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase));

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.That(result.Data, Is.EqualTo(expectedData));
        }

        [Test]
        public void Index_ValidModelSortByCustomerCount_GetCorrectDataFromDataAccess()
        {
            // Arrange
            int totalCustomer;
            var sortBy = nameof(ActiveCustomer.CustomerCount) + "A";
            var sortOrder = "asc";
            var metaModel = CreateValidMetaModel(sortBy, sortOrder);
            var expectedData = SetUpForGettingData(metaModel, out totalCustomer);
            expectedData.SortByDictionary<string, int>("A", nameof(ActiveCustomer.CustomerCount), sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase));

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.That(result.Data, Is.EqualTo(expectedData));
        }

        [Test]
        public void Index_ExportToExcel_ExportWithCorrectData()
        {
            // Arrange
            int totalCustomer;
            var metaModel = CreateValidMetaModel();
            metaModel.IsExportExcel = true;
            var expectedDataList = SetUpForGettingData(metaModel, out totalCustomer);

            ProductService.GetProductAccess(Arg.Any<int>(), Arg.Any<string>()).Returns(
                new ProductAccess(new List<Product> { new Product { ProductCode = Product.BA } }));

            var actualViewModel = new ActiveCustomerViewModel();
            var actualShowingOption = new ShowingOption();
            var excel = Substitute.ForPartsOf<ActiveCustomerSummaryExcel>(
                   Arg.Any<string>(),
                   Arg.Any<ConvertingOption>(),
                   Arg.Do<ShowingOption>(x => actualShowingOption = x),
                   new ActiveCustomerMetaModel(),
                   new ActiveCustomerViewModel());
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
                Assert.That(actualViewModel.MainResult, Is.EqualTo(expectedDataList.ToList()));
                Assert.That(actualShowingOption.BAVisible, Is.True);
            });
        }

        private ActiveCustomerMetaModel CreateValidMetaModel(string sortBy = null, string sortOrder = "asc")
        {
            var model = new ActiveCustomerMetaModel
            {
                CurrencyId = 1,
                AgentListXml = string.Empty,
                FromDate = DateTime.Now.AddDays(-2),
                ToDate = DateTime.Now.AddDays(-1),
                ProductOptions = new List<ProductOption>
                {
                    new ProductOption
                    {
                        Code = Product.BA,
                        Name = Product.BA,
                        Status = true
                    }
                },
                SortBy = sortBy,
                SortOrder = sortOrder
            };
            MockUserSessionData(new UserSession
            {
                CurrencyList = new Dictionary<int, string>
                {
                    { model.CurrencyId, nameof(model.CurrencyId) }
                },
                ApplicationMode = AppMode.Self,
                FunctionIdList = new List<int>()
            });
            MockLicenseeSessionData(new LicenseeSession { LicenseeId = 0 });
            SessionService.GenerateAgentListXml(Arg.Any<int>(), Arg.Any<bool>()).Returns(string.Empty);

            return model;
        }

        private SortableList<ActiveCustomer> SetUpForGettingData(ActiveCustomerMetaModel metaModel, out int totalCustomer)
        {
            var expectedUnSortedData = new SortableList<ActiveCustomer>();
            var nonSortedData = new SortableList<ActiveCustomer>();

            var random = new Random();
            var total = random.Next(5, 10);
            var customerCount = new string[] { "A", "B", "C", "D" };

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

                nonSortedData.Add(item);
                expectedUnSortedData.Add(item);
            }

            _dataAccess.GetActiveCustomers(metaModel, out totalCustomer).Returns(x =>
            {
                x[1] = nonSortedData.Sum(y => y.TotalCustomer);
                return nonSortedData;
            });

            return expectedUnSortedData;
        }
    }
}