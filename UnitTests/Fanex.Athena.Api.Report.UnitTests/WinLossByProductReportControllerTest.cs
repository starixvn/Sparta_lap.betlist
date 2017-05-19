namespace Fanex.Athena.Api.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http.Results;
    using System.Web.Script.Serialization;
    using Excel.Base.Entities;
    using Excel.Report;
    using Fanex.Athena.Service.Report.Interfaces;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Model.Report.ViewModel;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    public class WinLossByProductReportControllerTest
    {
        private ISessionService _sessionService;
        private IProductService _productService;
        private IWinLossByProductService _service;
        private WinLossByProductReportController _controller;
        private JavaScriptSerializer _serializer;

        [SetUp]
        public void SetUp()
        {
            _sessionService = Substitute.For<ISessionService>();
            _productService = Substitute.For<IProductService>();
            _service = Substitute.For<IWinLossByProductService>();
            _controller = Substitute.ForPartsOf<WinLossByProductReportController>(_sessionService, _productService, _service);
            _serializer = new JavaScriptSerializer();
            CreateSession();
        }

        [TearDown]
        public void TearDown()
        {
            _service = null;
            _controller = null;
            _sessionService = null;
        }

        [Test]
        public void Index_WhenCallWithInvalidModel_ReturnBadRequestWithCorrectMessage()
        {
            // Arrange
            var metaModel = new WinLossByProductMetaModel { CurrencyId = 1 };

            // Action
            var result = _controller.Index(metaModel) as BadRequestErrorMessageResult;

            // Assert
            Assert.That(result.Message, Is.EqualTo(ViewResources.InvalidParams));
        }

        [TestCase(100, "NoCustomerFoundMsg")]
        [TestCase(101, "MoreThanOneCustomerFoundMsg")]
        public void Index_WhenCallWithLevel0_CallAndGetCorrectDataFromService(int errorCode, string errorMessageKey)
        {
            // Arrange
            const int CurrencyId = -2, Level = 0, CustId = 1;
            IDictionary<string, IList<WinLossByProduct>> summaryData;
            IResultInfo resultInfo;
            var dataListResult = new List<WinLossByProduct>
            {
                new WinLossByProduct()
            };

            var summaryTotalResult = new Dictionary<string, IList<WinLossByProduct>> { { "EUR", new List<WinLossByProduct>() } };
            _service.GetWinlossByProducts(Arg.Any<WinLossByProductMetaModel>(), out summaryData, out resultInfo).Returns(x =>
            {
                x[1] = summaryTotalResult;
                x[2] = new ResultInfo
                {
                    Code = errorCode
                };
                return dataListResult;
            });

            // Action
            var result = _controller.Index(new WinLossByProductMetaModel(), null, null, Level, CurrencyId, CustId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Data.Count, Is.EqualTo(1));
                Assert.That(result.SummaryTotal.Count, Is.EqualTo(1));
                Assert.That(result.ErrorMessage, Is.EqualTo(ViewResources.ResourceManager.GetString(errorMessageKey)));
            });
        }

        [Test]
        public void Index_Level0_MemberIdReturnFromSP()
        {
            // Arrange
            const int CurrencyId = -2, Level = 0, CustId = 1;
            IDictionary<string, IList<WinLossByProduct>> summaryData;
            IResultInfo resultInfo;
            var metaModel = new WinLossByProductMetaModel();
            _service.GetWinlossByProducts(Arg.Any<WinLossByProductMetaModel>(), out summaryData, out resultInfo).Returns(x =>
            {
                x[2] = new ResultInfo
                {
                    ExtendedData = "123;456"
                };
                return new List<WinLossByProduct>();
            });

            // Action
            var result = _controller.Index(metaModel, null, null, Level, CurrencyId, CustId);

            // Assert
            Assert.That(metaModel.MemberId, Is.EqualTo(123));
        }

        [Test]
        public void Index_WhenCallWithLevel1_CallAndGetCorrectDataFromService()
        {
            // Arrange
            const int CurrencyId = -2, Level = 1, CustId = 1;
            IDictionary<string, IList<WinLossByProduct>> summaryData;
            var dataListResult = new List<WinLossByProduct>
            {
                new WinLossByProduct()
            };

            var summaryTotalResult = new Dictionary<string, IList<WinLossByProduct>> { { "EUR", new List<WinLossByProduct>() } };
            _service.GetWinlossByProductAgents(Arg.Any<WinLossByProductMetaModel>(), out summaryData).Returns(x =>
            {
                x[1] = summaryTotalResult;
                return dataListResult;
            });

            // Action
            var result = _controller.Index(new WinLossByProductMetaModel(), null, null, Level, CurrencyId, CustId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Data.Count, Is.EqualTo(1));
                Assert.That(result.SummaryTotal.Count, Is.EqualTo(1));
            });
        }

        [Test]
        public void Index_WhenCallWithLevel2_CallAndGetCorrectDataFromService()
        {
            // Arrange
            const int CurrencyId = -2, Level = 2, CustId = 1;
            IDictionary<string, IList<WinLossByProduct>> summaryData;
            var dataListResult = new List<WinLossByProduct>
            {
                new WinLossByProduct()
            };

            var summaryTotalResult = new Dictionary<string, IList<WinLossByProduct>> { { "EUR", new List<WinLossByProduct>() } };
            _service.GetWinlossByProductCustomers(Arg.Any<WinLossByProductMetaModel>(), out summaryData).Returns(x =>
            {
                x[1] = summaryTotalResult;
                return dataListResult;
            });

            // Action
            var result = _controller.Index(new WinLossByProductMetaModel(), null, null, Level, CurrencyId, CustId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Data.Count, Is.EqualTo(1));
                Assert.That(result.SummaryTotal.Count, Is.EqualTo(1));
            });
        }

        [Test]
        public void Index_IsExportExcelTrue_CallExcelStream()
        {
            // Arrange
            const int CurrencyId = -2, Level = 1, CustId = 1;
            _productService.GetProductAccess(Arg.Any<int>(), Arg.Any<string>()).Returns(
                new ProductAccess(new List<Product>()));
            var metaModel = new WinLossByProductMetaModel
            {
                IsExportExcel = true
            };
            var excel = Substitute.ForPartsOf<WinlossByProductExcel>(
                string.Empty,
                new ConvertingOption(),
                new ShowingOption(),
                metaModel,
                new WinLossByProductViewModel(),
                string.Empty,
                string.Empty);
            excel.When(x => x.Export()).DoNotCallBase();
            _controller.CreateExcel(
                Arg.Any<WinLossByProductMetaModel>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<ShowingOption>(),
                Arg.Any<ConvertingOption>(),
                Arg.Any<WinLossByProductViewModel>())
                .Returns(excel);
            _controller.When(x => x.ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>())).DoNotCallBase();

            // Action
            var result = _controller.Index(metaModel, null, null, Level, CurrencyId, CustId);

            // Assert
            _controller.Received().ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>());
        }

        [Test]
        public void Index_WhenCall_MetaModelAreAssignedDataCorrectly()
        {
            // Arrange
            const int CustId = 1, CurrencyId = -2, Level = 1, MemberId = 2;
            const string SystemId = "pig", CurrencyName = "UUS";
            var fromDate = new DateTime(2017, 4, 1);
            var toDate = new DateTime(2017, 4, 18);
            var metaModel = new WinLossByProductMetaModel();

            _sessionService.GenerateAgentListXml(Arg.Any<int>(), Arg.Any<bool>()).Returns("XmlData");

            // Action
            _controller.Index(metaModel, fromDate, toDate, Level, CurrencyId, CustId, MemberId, SystemId, CurrencyName);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(metaModel.AgentListXml, Is.EqualTo("XmlData"));
                Assert.That(metaModel.DrillDownCurrencyId, Is.EqualTo(CurrencyId));
                Assert.That(metaModel.DrillDownCurrencyName, Is.EqualTo(CurrencyName));
                Assert.That(metaModel.MemberId, Is.EqualTo(MemberId));
                Assert.That(metaModel.DrillDownFromDate, Is.EqualTo(fromDate));
                Assert.That(metaModel.DrillDownToDate, Is.EqualTo(toDate));
            });
        }

        private void CreateSession()
        {
            var userSession = new UserSession
            {
                CurrencyList = new Dictionary<int, string>
            {
                { -1, "EUR" }, { -2, "USD" }
            }
            };

            var licenseeSession = new LicenseeSession
            {
                LicenseeId = 1
            };

            _sessionService.UserSession.Returns(userSession);
            _sessionService.LicenseeSession.Returns(licenseeSession);
        }
    }
}