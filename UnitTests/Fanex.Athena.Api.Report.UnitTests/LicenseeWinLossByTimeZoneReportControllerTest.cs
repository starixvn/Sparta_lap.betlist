namespace Fanex.Athena.Api.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Base.UnitTests;
    using DataAccess.Report.Interfaces;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [TestFixture]
    public class LicenseeWinLossByTimeZoneReportControllerTest : BaseApiControllerHelperTest
    {
        private ILicenseeWinLossByTimeZoneDataAccess _dataAccess;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _dataAccess = Substitute.For<ILicenseeWinLossByTimeZoneDataAccess>();
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _dataAccess = null;
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
                _dataAccess.DidNotReceiveWithAnyArgs().GetData(Arg.Any<LicenseeWinLossByTimeZoneMetaModel>(), UserCurrencyList);
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
            SessionService.GenerateAgentListXml(metaModel.CurrencyId, metaModel.IsExcludeTestData).Returns(AgentListXml);
            _dataAccess.GetData(metaModel, UserCurrencyList).ReturnsForAnyArgs(new ReportResultModel<LicenseeWinLossByTimeZone>());

            // Action
            using (var controller = CreateController())
            {
                controller.Index(metaModel);
            }

            // Assert
            _dataAccess.Received(1).GetData(Arg.Is<LicenseeWinLossByTimeZoneMetaModel>(x => x.AgentListXml == AgentListXml), UserCurrencyList);
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
            _dataAccess.GetData(metaModel, UserCurrencyList).Returns(resultModel);
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
                SessionService,
                ProductService,
                LangService,
                _dataAccess);
        }
    }
}