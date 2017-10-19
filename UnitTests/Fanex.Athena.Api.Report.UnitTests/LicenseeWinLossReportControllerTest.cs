namespace Fanex.Athena.Api.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess.Report.Interfaces;
    using Excel.Report;
    using Excel.Report.Models;
    using Extension.Helper;
    using Extension.Utility;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Model.Core.MetaModel.Product;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Model.Report.ViewModel;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;

    public class LicenseeWinLossReportControllerTest : BaseApiReportControllerHelperTest
    {
        private IAppSettings _appSettings;

        private ILicenseeWinLossDataAccess _dataAccess;

        private LicenseeWinLossReportController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _appSettings = Substitute.For<IAppSettings>();
            _dataAccess = Substitute.For<ILicenseeWinLossDataAccess>();

            _controller = Substitute.ForPartsOf<LicenseeWinLossReportController>(SessionService, ProductService, _appSettings, _dataAccess);

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
            var model = new LicenseeWinLossMetaModel
            {
                ProductOptions = new List<ProductOption>
                {
                    new ProductOption()
                }
            };

            // Action
            var result = _controller.Index(model);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Data, Is.Empty);
                Assert.That(result.Total, Is.Empty);
                Assert.That(result.SummaryTotal, Is.Empty);
                Assert.That(result.ProductOptions, Is.EqualTo(model.ProductOptions.ToJson()));
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
            Assert.That(result.Data.Count, Is.EqualTo(expectedResult.DataList.Count));
        }

        [TestCase("ActualStake")]
        [TestCase("ActualStakeSingle")]
        public void Index_SortByActualStake_ReturnCorrectOrderedData(string sortBy)
        {
            // Arrange
            var metaModel = CreateValidMetaModel(sortBy: sortBy);
            var expectedResult = SetUpForGettingData(metaModel);
            expectedResult.DataList.Sort("ActualStake", true);

            // Action
            var result = _controller.Index(metaModel);
            IList<LicenseeWinLossGridViewModel> dataList = result.Data;

            // Assert
            Assert.That(dataList.Select(x => x.ActualStake).ToArray(), Is.EqualTo(expectedResult.DataList.Select(x => x.ActualStake).ToArray()));
        }

        [Test]
        public void Index_SortByLicenseeWLSB_ReturnCorrectOrderedData()
        {
            // Arrange
            var metaModel = CreateValidMetaModel(sortBy: "LicenseeWLSB");
            var resultModel = SetUpForGettingData(metaModel);
            resultModel.DataList.SortByDictionary("SB", "LicenseeWL", true);
            var expectedResult = resultModel.DataList.Select(x => x.LicenseeWL["SB"]).ToArray();

            // Action
            var result = _controller.Index(metaModel);
            IList<LicenseeWinLossGridViewModel> dataList = result.Data;

            // Assert
            Assert.That(dataList.Select(x => x.LicenseeWLSB).ToArray(), Is.EqualTo(expectedResult));
        }

        [Test]
        public void Index_ExportToExcel_ExportWithCorrectData()
        {
            // Arrange
            var metaModel = CreateValidMetaModel();
            metaModel.IsExportExcel = true;

            var expectedResult = SetUpForGettingData(metaModel);

            ProductService.GetProductAccess(Arg.Any<int>(), Arg.Any<string>()).Returns(
                new ProductAccess(
                    new List<Product>
                    {
                        new Product { ProductCode = "SB" },
                        new Product { ProductCode = "BA" }
                    }));

            var actualExcelModel = new LicenseeWinLossExcelModel();
            var excel = Substitute.ForPartsOf<LicenseeWinLossExcel>(
                   string.Empty);
            excel.When(x => x.Export()).DoNotCallBase();

            _controller.CreateExcel(
                  string.Empty,
                  Arg.Any<LicenseeWinLossExcelModel>()).ReturnsForAnyArgs(excel);
            _controller.When(x => x.CreateExcel(
                   Arg.Any<string>(),
                   Arg.Any<LicenseeWinLossExcelModel>())).Do(p => actualExcelModel = (LicenseeWinLossExcelModel)p[1]);

            // Action
            _controller.Index(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualExcelModel.DataList.Count(), Is.EqualTo(expectedResult.DataList.Count()));
                Assert.That(actualExcelModel.SBVisible, Is.True);
                Assert.That(actualExcelModel.BAVisible, Is.False);
                Assert.That(actualExcelModel.RBVisible, Is.False);
            });
        }

        private LicenseeWinLossMetaModel CreateValidMetaModel(string sortBy = null, string sortOrder = "asc")
        {
            var model = new LicenseeWinLossMetaModel
            {
                SortBy = sortBy,
                SortOrder = sortOrder,
                FromDate = new DateTime(2016, 1, 1),
                ToDate = new DateTime(2017, 1, 1),
                ProductOptions = new List<ProductOption>
                {
                    new ProductOption
                    {
                        Code = "SB",
                        Status = true
                    }
                }
            };

            StubUserSessionData(new UserSession
            {
                CurrencyList = new Dictionary<int, string>
                {
                    { model.CurrencyId, nameof(model.CurrencyId) }
                },
                FunctionIdList = new List<int>()
            });
            StubLicenseeSessionData(new LicenseeSession { LicenseeId = 0 });
            SessionService.GenerateAgentListXml(Arg.Any<int>(), Arg.Any<bool>()).Returns(string.Empty);

            return model;
        }

        private SortableSummaryReportResultModel<LicenseeWinLoss> SetUpForGettingData(LicenseeWinLossMetaModel metaModel)
        {
            var dataList = new SortableList<LicenseeWinLoss>();

            var random = new Random();
            var total = random.Next(5, 10);
            for (int i = 0; i < total; i++)
            {
                dataList.Add(new LicenseeWinLoss
                {
                    ActualStake = random.Next(1, 100),
                    LicenseeWL = new Dictionary<string, decimal>()
                    {
                        { "SB", random.Next(1, 100) }
                    },
                    TurnOver = new Dictionary<string, decimal>()
                });
            }

            var result = new SortableSummaryReportResultModel<LicenseeWinLoss>
            {
                DataList = dataList,
                TotalData = new List<LicenseeWinLoss>(),
                SummaryTotal = new Dictionary<string, LicenseeWinLoss>()
            };

            _dataAccess.GetSummaryWinloss(metaModel).Returns(result);
            return result;
        }
    }
}