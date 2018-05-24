namespace Fanex.Athena.Api.Report.UnitTests
{
    using System;
    using Base.UnitTests;
    using DataAccess.Report.Interfaces;
    using Extension.Utility;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Configuration;
    using Model.Report.MetaModel;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Configuration.Interfaces;

    public class LicenseeWinLossReportControllerTest_IsValidParams : BaseApiControllerHelperTest
    {
        private IReportSettingService _reportSettingService;

        private IAppSettings _appSettings;

        private ILicenseeWinLossDataAccess _dataAccess;

        private LicenseeWinLossReportController _controller;

        [SetUp]
        public void SetUp()
        {
            _appSettings = Substitute.For<IAppSettings>();
            _dataAccess = Substitute.For<ILicenseeWinLossDataAccess>();
            _reportSettingService = Substitute.For<IReportSettingService>();

            _controller = Substitute.ForPartsOf<LicenseeWinLossReportController>(
                SessionService,
                ProductService,
                _reportSettingService,
                LangService,
                _appSettings,
                _dataAccess);

            _controller.When(x => x.ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>())).DoNotCallBase();

            MockSessionData();
        }

        [Test]
        public void IsValidParams_ReportSettingIsNull_ReturnTrue()
        {
            // Arrange
            _reportSettingService.GetSetting(PermissionKeys.ReportLicenseeWinLoss).Returns((ReportSetting)null);

            // Action
            var result = _controller.IsValidParams(new LicenseeWinLossMetaModel());

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsValidParams_DisableBetOnChildLevel_ReturnFalse()
        {
            // Arrange
            _reportSettingService.GetSetting(PermissionKeys.ReportLicenseeWinLoss)
                .Returns(new ReportSetting
                {
                    IsDisableBetList = true
                });
            var metaModel = new LicenseeWinLossMetaModel
            {
                DrillDownLevel = 1
            };

            // Action
            var result = _controller.IsValidParams(metaModel);

            // Assert
            Assert.That(result, Is.False);
        }

        [TestCase("2017/12/21", "2018/2/2", false)]
        [TestCase("2018/2/2", "2018/9/9", false)]
        [TestCase("2018/2/2", "2018/3/3", true)]
        public void IsValidParams_DateRangeOnSummaryLevel_ReturnCorrectResult(
            string fromDate,
            string toDate,
            bool expectedResult)
        {
            // Arrange
            SetUpReportSetting();
            var metaModel = new LicenseeWinLossMetaModel
            {
                FromDate = DateTime.Parse(fromDate),
                ToDate = DateTime.Parse(toDate)
            };

            // Action
            var result = _controller.IsValidParams(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [TestCase("2017/12/21", "2018/2/2", false)]
        [TestCase("2018/2/2", "2018/9/9", false)]
        [TestCase("2018/2/2", "2018/3/3", true)]
        public void IsValidParams_DateRangeOnChildLevel_ReturnCorrectResult(
            string fromDate,
            string toDate,
            bool expectedResult)
        {
            // Arrange
            SetUpReportSetting();
            var metaModel = new LicenseeWinLossMetaModel
            {
                DrillDownLevel = 1,
                DrillDownFromDate = DateTime.Parse(fromDate),
                DrillDownToDate = DateTime.Parse(toDate)
            };

            // Action
            var result = _controller.IsValidParams(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        private void SetUpReportSetting()
        {
            _reportSettingService.GetSetting(PermissionKeys.ReportLicenseeWinLoss)
                            .Returns(new ReportSetting
                            {
                                MinDate = new DateTime(2018, 1, 1),
                                MaxDate = new DateTime(2018, 5, 5)
                            });
        }
    }
}