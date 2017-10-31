namespace Fanex.Athena.Api.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Excel.Base.Entities;
    using Excel.Report;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Model.Core.DTO.Users;
    using Model.Core.MetaModel.Product;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Model.Report.ViewModel;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;

    public class CasinoProgressiveWinsReportControllerTest : BaseApiReportControllerHelperTest
    {
        private ICasinoProgressiveWinsDataAccess _dataAccess;
        private CasinoProgressiveWinsReportController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            MockSessionData();
            SetUpForDataAccess();

            _controller = Substitute.ForPartsOf<CasinoProgressiveWinsReportController>(_dataAccess, SessionService, ProductService);
            _controller.When(x => x.ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>())).DoNotCallBase();
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _dataAccess = null;
            _controller = null;
        }

        [Test]
        public void Index_InvalidRequest_ReturnEmptyData()
        {
            // Arrange
            var model = new CasinoProgressiveWinsMetaModel
            {
                CurrencyId = 1
            };

            // Action
            var result = _controller.Index(model);

            // Assert
            Assert.IsEmpty(result.Data);
        }

        [Test]
        public void Index_InvalidModel_ReturnEmptyData()
        {
            // Arrange
            var model = new CasinoProgressiveWinsMetaModel
            {
                CurrencyId = -1,
                FromDate = DateTime.Today,
                ToDate = DateTime.Today.AddDays(-1)
            };

            // Action
            var result = _controller.Index(model);

            // Assert
            Assert.IsEmpty(result.Data);
        }

        [Test]
        public void Index_ValidModel_ReturnCorrectChoice()
        {
            // Arrange
            var model = new CasinoProgressiveWinsMetaModel
            {
                CurrencyId = -1,
                ProductOptions = new List<ProductOption>
                {
                    new ProductOption { Status = true }
                }
            };

            // Action
            var choice = _controller.Index(model).Data[0].Choice;

            // Assert
            StringAssert.Contains("<strong>", choice);
            StringAssert.Contains("<strong class='sport'>", choice);
            StringAssert.Contains("<strong class='bettype'>", choice);
        }

        [Test]
        public void Index_ExportExcel_ExportWithCorrectData()
        {
            // Arrange
            var model = new CasinoProgressiveWinsMetaModel
            {
                CurrencyId = -1,
                BaseCurrency = "USD",
                CurrencyName = "Currency",
                IsExportExcel = true,
                ProductOptions = new List<ProductOption>
                {
                    new ProductOption { Status = true }
                }
            };

            var expectedFileName = "CasinoProgressiveWin_Microgaming.xls";

            ConvertingOption actualOption = null;
            var excel = Substitute.ForPartsOf<CasinoProgressiveWinsExcel>(
                   Arg.Any<string>(),
                   Arg.Do<ConvertingOption>(x => actualOption = x),
                   new CasinoProgressiveWinsViewModel());
            excel.When(x => x.Export()).DoNotCallBase();

            _controller.CreateExcel(
                   Arg.Any<string>(),
                   Arg.Any<ConvertingOption>(),
                   new CasinoProgressiveWinsViewModel()).ReturnsForAnyArgs(excel);

            // Action
            _controller.Index(model);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualOption.IsConvert, Is.True);
                Assert.That(actualOption.BaseCurrency, Is.EqualTo(model.BaseCurrency));
                Assert.That(actualOption.CurrencyName, Is.EqualTo(model.CurrencyName));
                _controller.Received().ExcelStream(Arg.Any<IWorkbook>(), expectedFileName);
            });
        }

        private void SetUpForDataAccess()
        {
            _dataAccess = Substitute.For<ICasinoProgressiveWinsDataAccess>();
            _dataAccess.GetDataList(Arg.Any<CasinoProgressiveWinsMetaModel>()).Returns(new List<CasinoProgressiveWins>
            {
                new CasinoProgressiveWins { BetType = -100, SportType = -100 }
            });
        }
    }
}