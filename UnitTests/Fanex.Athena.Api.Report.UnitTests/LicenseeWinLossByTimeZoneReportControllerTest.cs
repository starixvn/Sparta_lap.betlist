namespace Fanex.Athena.Api.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using DataAccess.Report.Interfaces;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [TestFixture]
    public class LicenseeWinLossByTimeZoneReportControllerTest
    {
        private ISessionService _sessionService;

        private ILicenseeWinLossByTimeZoneDataAccess _dataAccess;

        [OneTimeSetUp]
        public void SetUp()
        {
            _sessionService = Substitute.For<ISessionService>();
            _dataAccess = Substitute.For<ILicenseeWinLossByTimeZoneDataAccess>();
        }

        [OneTimeTearDown]
        public void FixtureTearDown()
        {
            _dataAccess = null;
            _sessionService = null;
        }

        [Test]
        public void Index_MetaModelIsInvalid_ReturnEmptyResult()
        {
            // Arrange
            var metaModel = new LicenseeWinLossByTimeZoneMetaModel
            {
                ProductCodeList = string.Empty
            };
            dynamic result;

            // Action
            using (var controller = CreateController())
            {
                result = controller.Index(metaModel);
            }

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.DidNotReceiveWithAnyArgs().GetData(Arg.Any<LicenseeWinLossByTimeZoneMetaModel>());
                Assert.That(result.Data, Is.Empty);
            });
        }

        [Test]
        public void Index_ValidMetaModel_AssignCorrectAgentListXml()
        {
            // Arrange
            const string AgentListXml = "<Root><Agent AgentId=\"1\" Currency=\"1\"/></Root>";
            var metaModel = CreateValidMetaModel();
            metaModel.CurrencyId = 999;
            _sessionService.GenerateAgentListXml(metaModel.CurrencyId, metaModel.IsExcludeTestData).Returns(AgentListXml);
            _dataAccess.GetData(metaModel).ReturnsForAnyArgs(new ReportResultModel<LicenseeWinLossByTimeZone>());

            // Action
            using (var controller = CreateController())
            {
                controller.Index(metaModel);
            }

            // Assert
            _dataAccess.Received(1).GetData(Arg.Is<LicenseeWinLossByTimeZoneMetaModel>(x => x.AgentListXml == AgentListXml));
        }

        [Test]
        public void Index_ValidMetaModel_ReturnCorrectResultFromDataAccess()
        {
            // Arrange
            var metaModel = CreateValidMetaModel();

            var resultModel = new ReportResultModel<LicenseeWinLossByTimeZone>
            {
                DataList = new List<LicenseeWinLossByTimeZone>
                {
                    new LicenseeWinLossByTimeZone()
                },
                TotalData = new LicenseeWinLossByTimeZone { BetCount = 123 }
            };
            _dataAccess.GetData(metaModel).Returns(resultModel);
            dynamic result;

            // Action
            using (var controller = CreateController())
            {
                result = controller.Index(metaModel);
            }

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Data, Is.EqualTo(resultModel.DataList));
                Assert.That(result.Total, Is.EqualTo(resultModel.TotalData));
            });
        }

        private static LicenseeWinLossByTimeZoneMetaModel CreateValidMetaModel()
        {
            return new LicenseeWinLossByTimeZoneMetaModel
            {
                FromDate = new DateTime(2017, 1, 1),
                ToDate = new DateTime(2017, 1, 2),
                ProductCodeList = "SB",
                BaseCurrency = "EUR",
            };
        }

        private LicenseeWinLossByTimeZoneReportController CreateController()
        {
            return Substitute.ForPartsOf<LicenseeWinLossByTimeZoneReportController>(
                _sessionService,
                Substitute.For<IProductService>(),
                _dataAccess);
        }
    }
}