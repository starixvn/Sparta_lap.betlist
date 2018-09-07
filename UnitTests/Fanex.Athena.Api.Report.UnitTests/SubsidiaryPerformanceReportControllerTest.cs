namespace Fanex.Athena.Api.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Script.Serialization;
    using Base.UnitTests;
    using Fanex.Athena.Service.Report.Interfaces;
    using Model.Report.DTO;
    using Model.Report.DTO.SubsidiaryPerformance;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SubsidiaryPerformanceReportControllerTest : BaseApiControllerHelperTest
    {
        private ISubsidiaryPerformanceService _service;

        private SubsidiaryPerformanceReportController _controller;

        private JavaScriptSerializer _serializer;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _service = Substitute.For<ISubsidiaryPerformanceService>();
            _controller = Substitute.ForPartsOf<SubsidiaryPerformanceReportController>(SessionService, ProductService, LangService, _service);
            _serializer = new JavaScriptSerializer();

            MockSessionData();
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _service = null;
            _controller = null;
        }

        [Test]
        public void Index_WhenCall_ServiceIsCalledWithCorrectMetaModel()
        {
            // Arrange
            const string AgentListXml = "<Root><Agent AgentId=\"1\" Currency=\"1\"/><Agent AgentId=\"2\" Currency=\"1\"/></Root>";
            var metaModel = GetMetaModel();
            metaModel.AgentListXml = AgentListXml;

            SessionService.GenerateAgentListXml(Arg.Any<IEnumerable<int>>()).Returns(AgentListXml);
            SubsidiaryPerformanceMetaModel passedMetaModel = null;
            var resultData = new SubsidiaryResultViewModel
            {
                Total = new Dictionary<string, SubsidiaryPerformanceBase>()
            };

            _service.GetData(Arg.Do<SubsidiaryPerformanceMetaModel>(x => passedMetaModel = x)).Returns(resultData);

            // Action
            _controller.Index(metaModel);

            // Assert
            Assert.That(metaModel, Is.EqualTo(passedMetaModel));
        }

        [Test]
        public void Index_SubsidiaryListHasValue_GenerateAgentListXmlIsCalledWithCorrectParameter()
        {
            // Arrange
            var metaModel = GetMetaModel();

            var operatorList = new List<Operator>
            {
                new Operator { AgentId = 1, OperatorName = "New York", CurrencyId = 1 },
                new Operator { AgentId = 3, OperatorName = "New York 2", CurrencyId = 1 },
                new Operator { AgentId = 2, OperatorName = "India", CurrencyId = 2 }
            };
            _service.GetOperatorList(SessionLicenseeName).Returns(operatorList);
            IDictionary<int, IEnumerable<int>> currencyAgentList = null;
            SessionService.GenerateAgentListXml(Arg.Do<IDictionary<int, IEnumerable<int>>>(x => currencyAgentList = x));

            var resultData = new SubsidiaryResultViewModel
            {
                Data = new List<SubsidiaryPerformanceDataModel>(),
                Total = new Dictionary<string, SubsidiaryPerformanceBase>()
            };
            _service.GetData(Arg.Any<SubsidiaryPerformanceMetaModel>()).Returns(resultData);

            // Action
            _controller.Index(metaModel);

            // Assert
            Assert.That(currencyAgentList.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Index_SubsidiaryListHasNoValue_ReturnEmptyResult()
        {
            // Arrange
            var metaModel = GetMetaModel();

            var currencyIdList = new List<Operator>();
            _service.GetOperatorList(SessionLicenseeName).Returns(currencyIdList);
            var expectedResult = _serializer.Serialize(new SubsidiaryResultViewModel { Data = new List<SubsidiaryPerformanceDataModel>() });

            // Action
            var resultData = _controller.Index(metaModel);
            var result = _serializer.Serialize(resultData);

            // Assert
            SessionService.DidNotReceiveWithAnyArgs().GenerateAgentListXml(Arg.Any<IDictionary<int, IEnumerable<int>>>());
            Assert.That(result, Is.EqualTo(expectedResult));
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
            var expectedResult = _serializer.Serialize(new SubsidiaryResultViewModel
            {
                Data = new List<SubsidiaryPerformanceDataModel>()
            });

            // Action
            var result = _serializer.Serialize(_controller.Index(metaModel));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
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