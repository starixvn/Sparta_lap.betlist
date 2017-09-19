namespace Fanex.Athena.Api.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http.Results;
    using DataAccess.Report.Interfaces;
    using Excel.Base.Entities;
    using Excel.Report;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Model.Core.MetaModel.Product;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
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
        private IWinLossByProductDataAccess _dataAccess;
        private WinLossByProductReportController _controller;

        [SetUp]
        public void SetUp()
        {
            _sessionService = Substitute.For<ISessionService>();
            _productService = Substitute.For<IProductService>();
            _dataAccess = Substitute.For<IWinLossByProductDataAccess>();
            _controller = Substitute.ForPartsOf<WinLossByProductReportController>(_sessionService, _productService, _dataAccess);
            CreateSession();
        }

        [TearDown]
        public void TearDown()
        {
            _dataAccess = null;
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
            IEnumerable<WinLossByProduct> totalData;
            IDictionary<string, IEnumerable<WinLossByProduct>> summaryData;
            IResultInfo resultInfo;
            var dataListResult = new List<WinLossByProduct>
            {
                new WinLossByProduct()
            };

            var summaryTotalResult = new Dictionary<string, IEnumerable<WinLossByProduct>> { { "EUR", new List<WinLossByProduct>() } };
            _dataAccess.GetWinlossByProducts(Arg.Any<WinLossByProductMetaModel>(), out totalData, out summaryData, out resultInfo).Returns(x =>
            {
                x[2] = summaryTotalResult;
                x[3] = new ResultInfo
                {
                    Code = errorCode
                };
                return dataListResult;
            });

            // Action
            var result = _controller.Index(CreateValidMetaModel(), null, null, Level, CurrencyId, CustId);

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.Received().GetWinlossByProducts(Arg.Any<WinLossByProductMetaModel>(), out totalData, out summaryData, out resultInfo);
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
            IEnumerable<WinLossByProduct> totalData;
            IDictionary<string, IEnumerable<WinLossByProduct>> summaryData;
            IResultInfo resultInfo;
            var metaModel = CreateValidMetaModel();
            _dataAccess.GetWinlossByProducts(Arg.Any<WinLossByProductMetaModel>(), out totalData, out summaryData, out resultInfo).Returns(x =>
            {
                x[3] = new ResultInfo
                {
                    ExtendedData = "123;456"
                };
                return new List<WinLossByProduct>();
            });

            // Action
            _controller.Index(metaModel, null, null, Level, CurrencyId, CustId);

            // Assert
            Assert.That(metaModel.MemberId, Is.EqualTo(123));
        }

        [Test]
        public void Index_WhenCallWithLevel1_CallAndGetCorrectDataFromService()
        {
            // Arrange
            const int CurrencyId = -2, Level = 1, CustId = 1;
            IEnumerable<WinLossByProduct> totalData;
            IDictionary<string, IEnumerable<WinLossByProduct>> summaryData;
            var dataListResult = new List<WinLossByProduct>
            {
                new WinLossByProduct()
            };

            var summaryTotalResult = new Dictionary<string, IEnumerable<WinLossByProduct>> { { "EUR", new List<WinLossByProduct>() } };
            _dataAccess.GetWinlossByProductAgents(Arg.Any<WinLossByProductMetaModel>(), out totalData, out summaryData).Returns(x =>
            {
                x[2] = summaryTotalResult;
                return dataListResult;
            });

            // Action
            var result = _controller.Index(CreateValidMetaModel(), null, null, Level, CurrencyId, CustId);

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.Received().GetWinlossByProductAgents(Arg.Any<WinLossByProductMetaModel>(), out totalData, out summaryData);
                Assert.That(result.Data.Count, Is.EqualTo(1));
                Assert.That(result.SummaryTotal.Count, Is.EqualTo(1));
            });
        }

        [Test]
        public void Index_WhenCallWithLevel2_CallAndGetCorrectDataFromService()
        {
            // Arrange
            const int CurrencyId = -2, Level = 2, CustId = 1;
            IEnumerable<WinLossByProduct> totalData;
            IDictionary<string, IEnumerable<WinLossByProduct>> summaryData;
            var dataListResult = new List<WinLossByProduct>
            {
                new WinLossByProduct()
            };

            var summaryTotalResult = new Dictionary<string, IEnumerable<WinLossByProduct>> { { "EUR", new List<WinLossByProduct>() } };
            _dataAccess.GetWinlossByProductCustomers(Arg.Any<WinLossByProductMetaModel>(), out totalData, out summaryData).Returns(x =>
            {
                x[2] = summaryTotalResult;
                return dataListResult;
            });

            // Action
            var result = _controller.Index(CreateValidMetaModel(), null, null, Level, CurrencyId, CustId);

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.Received().GetWinlossByProductCustomers(Arg.Any<WinLossByProductMetaModel>(), out totalData, out summaryData);
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
                string.Empty,
                string.Empty,
                null,
                null,
                null);
            excel.When(x => x.Export()).DoNotCallBase();
            _controller.CreateExcel(
                Arg.Any<WinLossByProductMetaModel>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<ShowingOption>(),
                Arg.Any<ConvertingOption>(),
                Arg.Any<IEnumerable<WinLossByProduct>>(),
                Arg.Any<IEnumerable<WinLossByProduct>>(),
                Arg.Any<IDictionary<string, IEnumerable<WinLossByProduct>>>())
                .Returns(excel);
            _controller.When(x => x.ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>())).DoNotCallBase();

            // Action
            _controller.Index(metaModel, null, null, Level, CurrencyId, CustId);

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

        private static WinLossByProductMetaModel CreateValidMetaModel()
        {
            return new WinLossByProductMetaModel
            {
                ProductOptions = new List<ProductOption>
                {
                    new ProductOption
                    {
                        Status = true
                    }
                }
            };
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