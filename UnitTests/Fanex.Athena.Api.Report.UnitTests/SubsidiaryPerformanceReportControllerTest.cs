namespace Fanex.Athena.Api.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Base.UnitTests;
    using Constant.DataDomain;
    using Fanex.Athena.Service.Report.Interfaces;
    using Model.Report.DTO.SubsidiaryPerformance;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public partial class SubsidiaryPerformanceReportControllerTest : BaseApiControllerHelperTest
    {
        private ISubsidiaryService _subsidiaryService;

        private ISubsidiaryPerformanceService _service;

        private SubsidiaryPerformanceReportController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();
            _subsidiaryService = Substitute.For<ISubsidiaryService>();
            _service = Substitute.For<ISubsidiaryPerformanceService>();
            _controller = Substitute.ForPartsOf<SubsidiaryPerformanceReportController>(SessionService, ProductService, LangService, _subsidiaryService, _service);
            _controller.When(x => x.CheckValidProducts(Arg.Any<string>(), Arg.Any<string>())).DoNotCallBase();
            MockSessionData();
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _service = null;
            _controller = null;
        }

        [TearDown]
        public void TearDown()
        {
            SessionService.ClearReceivedCalls();
            _subsidiaryService.ClearReceivedCalls();
            _service.ClearReceivedCalls();
        }

        [Test]
        public void Index_SubsidiaryView_DoNotCallServiceToGetSubsidiaryList()
        {
            // Arrange
            var metaModel = GetMetaModel();

            SessionService.GenerateAgentListXml(Arg.Any<int>(), Arg.Any<bool>()).Returns("<Root></Root>");
            SessionService.UserSession.Returns(new Session.Model.UserSession
            {
                IsManageableSubsidiary = true,
                UserLevel = UserLevelType.Subsidiary
            });

            var resultData = new SubsidiaryResultViewModel
            {
                Total = new Dictionary<string, SubsidiaryPerformanceBase>()
            };
            _service.GetData(Arg.Any<SubsidiaryPerformanceMetaModel>()).Returns(resultData);

            // Action
            _controller.Index(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                SessionService.Received().GenerateAgentListXml(0, false);
                _subsidiaryService.DidNotReceiveWithAnyArgs().GetCurrencyAgentList(Arg.Any<string>(), Arg.Any<bool>(), 0);
            });
        }

        [Test]
        public void Index_LicenseeView_CallServiceToGetSubsidiaryList()
        {
            // Arrange
            var metaModel = GetMetaModel();

            SessionService.GenerateAgentListXml(Arg.Any<IDictionary<int, IEnumerable<int>>>()).Returns("<Root></Root>");
            SessionService.UserSession.Returns(new Session.Model.UserSession
            {
                IsManageableSubsidiary = true,
                UserLevel = UserLevelType.Licensee
            });

            var currencyAgentList = new Dictionary<int, IEnumerable<int>> { { 1, new[] { 1, 2, 3 } } };
            _subsidiaryService.GetCurrencyAgentList(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<int>()).Returns(currencyAgentList);

            var resultData = new SubsidiaryResultViewModel
            {
                Total = new Dictionary<string, SubsidiaryPerformanceBase>()
            };
            _service.GetData(Arg.Any<SubsidiaryPerformanceMetaModel>()).Returns(resultData);

            // Action
            _controller.Index(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                SessionService.Received().GenerateAgentListXml(currencyAgentList);
                _subsidiaryService.Received().GetCurrencyAgentList(metaModel.SubsidiaryList, metaModel.IsExcludeTest, 0);
            });
        }

        [Test]
        public void Index_FromDateIsGreaterThanToDate_DoNotCallService()
        {
            // Arrange
            var metaModel = new SubsidiaryPerformanceMetaModel
            {
                FromDate = new DateTime(2017, 8, 30),
                ToDate = new DateTime(2017, 8, 1),
                ProductCodeList = "SB,BA",
                BaseCurrency = "EUR"
            };

            // Action
            _controller.Index(metaModel);

            // Assert
            _service.DidNotReceiveWithAnyArgs().GetData(Arg.Any<SubsidiaryPerformanceMetaModel>());
        }

        [Test]
        public void Index_ProductCodeListIsEmpty_DoNotCallService()
        {
            // Arrange
            var metaModel = new SubsidiaryPerformanceMetaModel
            {
                FromDate = new DateTime(2017, 8, 30),
                ToDate = new DateTime(2017, 8, 30),
                ProductCodeList = string.Empty,
                BaseCurrency = "EUR"
            };

            // Action
            _controller.Index(metaModel);

            // Assert
            _service.DidNotReceiveWithAnyArgs().GetData(Arg.Any<SubsidiaryPerformanceMetaModel>());
        }

        [Test]
        public void Index_BaseCurrencyIsEmpty_DoNotCallService()
        {
            // Arrange
            var metaModel = new SubsidiaryPerformanceMetaModel
            {
                FromDate = new DateTime(2017, 8, 30),
                ToDate = new DateTime(2017, 8, 30),
                BaseCurrency = string.Empty
            };

            // Action
            _controller.Index(metaModel);

            // Assert
            _service.DidNotReceiveWithAnyArgs().GetData(Arg.Any<SubsidiaryPerformanceMetaModel>());
        }

        [Test]
        public void Index_MetaModelIsNotValid_ReturnEmptyList()
        {
            // Arrange
            var metaModel = new SubsidiaryPerformanceMetaModel();

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.That(result.Data, Is.Empty);
        }

        [Test]
        public void Index_InvalidSubsidiaryList_ReturnEmptyList()
        {
            // Arrange
            var metaModel = new SubsidiaryPerformanceMetaModel
            {
                FromDate = new DateTime(2017, 8, 30),
                ToDate = new DateTime(2017, 8, 30),
                ProductCodeList = "SB",
                SubsidiaryList = string.Empty,
                BaseCurrency = "USD"
            };

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.That(result.Data, Is.Empty);
        }

        private SubsidiaryPerformanceMetaModel GetMetaModel()
        {
            return new SubsidiaryPerformanceMetaModel
            {
                FromDate = new DateTime(2017, 8, 30),
                ToDate = new DateTime(2017, 8, 30),
                ProductCodeList = "SB,BA",
                SubsidiaryList = "New York,India",
                BaseCurrency = "USD"
            };
        }
    }
}