namespace Fanex.Athena.Api.Report.UnitTests
{
    using System;
    using Base.UnitTests;
    using DataAccess.Report.Interfaces;
    using Model.Core.DataDomain;
    using Model.Core.ResultModel;
    using Model.Report;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using UnitTest.Common.Helpers;

    public class PromotionPrizeReportControllerTest : BaseApiControllerHelperTest
    {
        private const string FileName = "PP_PromotionPrize.xls";

        private IPromotionPrizeDataAccess _dataAccess;

        private PromotionPrizeReportController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();
            _dataAccess = Substitute.For<IPromotionPrizeDataAccess>();
            _controller = Substitute.ForPartsOf<PromotionPrizeReportController>(
                SessionService,
                ProductService,
                LangService,
                _dataAccess);
            _controller.When(x => x.CreareExcel(Arg.Any<string>(), Arg.Any<string>(), FileName, Arg.Any<ReportResultModel<PromotionPrize>>())).DoNotCallBase();

            MockSessionData();
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _controller = null;
            _dataAccess = null;
        }

        [TearDown]
        public void TearDown()
        {
            _controller.ClearReceivedCalls();
        }

        [Test]
        public void CreareExcel_InvalidModel_ReturnEmptyData()
        {
            // Arrange
            var model = new PromotionPrizeMetaModel
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today.AddDays(-1)
            };

            // Action
            var result = _controller.ExportExcel(model);

            // Assert
            _controller.Received().CreareExcel(
                Arg.Any<string>(),
                Arg.Any<string>(),
                FileName,
                Arg.Is<ReportResultModel<PromotionPrize>>(x => x.TotalRows == 0));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-2)]
        [TestCase(-3)]
        public void CreareExcel_CurrencyIdIsLessThanOrEqualZero_ReturnCorrectTitle(int currencyId)
        {
            // Arrange
            var model = FakeHelper.Create<PromotionPrizeMetaModel>();
            model.ToDate = model.FromDate.AddDays(1);
            model.CurrencyId = currencyId;

            var expectedTitle = $"{ViewResources.ProductPP} - {@ViewResources.PromotionPrize}, "
                + $"{model.FromDate.ToString(FormatKey.DateFormat)} --> {model.ToDate.ToString(FormatKey.DateFormat)} - "
                + ViewResources.AllCurrencies;

            // Action
            var result = _controller.ExportExcel(model);

            // Assert
            _controller.Received().CreareExcel(
                Arg.Any<string>(),
                expectedTitle,
                FileName,
                Arg.Any<ReportResultModel<PromotionPrize>>());
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void CreareExcel_CurrencyIdIsGreaterThanZero_ReturnCorrectTitle(int currencyId)
        {
            // Arrange
            var model = FakeHelper.Create<PromotionPrizeMetaModel>();
            model.ToDate = model.FromDate.AddDays(1);
            model.CurrencyId = currencyId;

            var expectedTitle = $"{ViewResources.ProductPP} - {@ViewResources.PromotionPrize}, "
                + $"{model.FromDate.ToString(FormatKey.DateFormat)} --> {model.ToDate.ToString(FormatKey.DateFormat)} - "
                + model.CurrencyName;

            // Action
            var result = _controller.ExportExcel(model);

            // Assert
            _controller.Received().CreareExcel(
                Arg.Any<string>(),
                expectedTitle,
                FileName,
                Arg.Any<ReportResultModel<PromotionPrize>>());
        }
    }
}