namespace Fanex.Athena.Api.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Base.UnitTests;
    using DataAccess.Report.Interfaces;
    using Excel.Base.Entities;
    using Excel.Report;
    using Model.Core.DTO.Users;
    using Model.Core.MetaModel.Product;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Model.Report.ViewModel;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;

    public class MicrogamingWinlossReportControllerTest : BaseApiControllerHelperTest
    {
        private IMicroGamingWinLossDataAccess _dataAccess;

        private MicrogamingWinlossReportController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _dataAccess = Substitute.For<IMicroGamingWinLossDataAccess>();

            _controller = Substitute.ForPartsOf<MicrogamingWinlossReportController>(SessionService, ProductService, LangService, _dataAccess);

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
        public void Index_WhenCall_GetCorrectDataFromDataAccess()
        {
            // Arrange
            var metaModel = CreateValidMetaModel();
            var expectedResult = SetUpForGettingData(metaModel);

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.That(result.Data.Count, Is.EqualTo(expectedResult.DataList.Count()));
        }

        [Test]
        public void Index_ExportToExcel_ExportWithCorrectData()
        {
            // Arrange
            var metaModel = CreateValidMetaModel();
            metaModel.IsExportExcel = true;

            var expectedResult = SetUpForGettingData(metaModel);

            MicroGamingWinLossViewModel actualExcelModel = null;
            var excel = Substitute.ForPartsOf<MicroGamingWinLossExcel>(
                   string.Empty);
            excel.When(x => x.Export()).DoNotCallBase();

            _controller.CreateExcel(
                  Arg.Any<ThirdPartyWinLossMetaModel>(),
                  string.Empty,
                  Arg.Any<ConvertingOption>(),
                  Arg.Any<MicroGamingWinLossViewModel>()).ReturnsForAnyArgs(excel);
            _controller.When(x => x.CreateExcel(
                   Arg.Any<ThirdPartyWinLossMetaModel>(),
                  Arg.Any<string>(),
                  Arg.Any<ConvertingOption>(),
                  Arg.Any<MicroGamingWinLossViewModel>())).Do(p => actualExcelModel = (MicroGamingWinLossViewModel)p[3]);

            // Action
            _controller.Index(metaModel);

            // Assert
            Assert.That(actualExcelModel.MainResult.Count(), Is.EqualTo(expectedResult.DataList.Count()));
        }

        private ThirdPartyWinLossMetaModel CreateValidMetaModel()
        {
            var model = new ThirdPartyWinLossMetaModel
            {
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

        private ReportResultModel<MicroGamingWinLoss> SetUpForGettingData(ThirdPartyWinLossMetaModel metaModel)
        {
            var dataList = new List<MicroGamingWinLoss>();

            var random = new Random();
            var total = random.Next(5, 10);
            for (int i = 0; i < total; i++)
            {
                dataList.Add(new MicroGamingWinLoss());
            }

            var result = new ReportResultModel<MicroGamingWinLoss>
            {
                DataList = dataList,
                TotalDataList = new List<MicroGamingWinLoss>(),
                SummaryTotal = new Dictionary<string, MicroGamingWinLoss>()
            };

            _dataAccess.GetSummaryWinloss(metaModel).Returns(result);
            return result;
        }
    }
}