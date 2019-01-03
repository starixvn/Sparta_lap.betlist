namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Model.Core.DTO.Common;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report;
    using Service.Core.Common.Interfaces;
    using Service.Report.Interfaces;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public partial class ReportAPIControllerTest : BaseControllerHelperTest
    {
        private const int CurrencyId = 134;
        private const string AgentListXml = "Xml";

        private IBetTypeService _betTypeService;
        private ICustomerTurnoverService _customerTurnoverService;
        private ITurnoverService _turnoverService;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();
            _betTypeService = Substitute.For<IBetTypeService>();
            _customerTurnoverService = Substitute.For<ICustomerTurnoverService>();
            _turnoverService = Substitute.For<ITurnoverService>();
            SetUpDefaultFakeData();
        }

        [Test]
        public void GetBettypesBySport_EmptySport_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = controller.GetBettypesBySport(new List<SportProductItem>()) as JsonResult;
                var resultData = result.Data as IEnumerable<SportProductItem>;

                // Assert
                Assert.That(resultData, Is.Empty);
            }
        }

        [Test]
        public void GetBettypesBySport_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var sports = new List<SportProductItem>
                {
                    new SportProductItem()
                };
                var dataList = new List<CheckedItem>
                {
                    new CheckedItem()
                };
                _betTypeService.GetBettypes(
                    SessionService.LicenseeSession.LicenseeId,
                    sports,
                    SessionService.UserSession.Language).Returns(dataList);

                // Action
                var result = controller.GetBettypesBySport(sports) as JsonResult;
                var resultData = result.Data as IEnumerable<CheckedItem>;

                // Assert
                Assert.That(resultData, Is.EqualTo(dataList));
            }
        }

        [Test]
        public void CustomerTurnover_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                SessionService.GenerateAgentListXml(CurrencyId).Returns(AgentListXml);
                var metaModel = new CustomerTurnoverMetaModel
                {
                    CurrencyId = CurrencyId
                };
                var data = new ReportResultModel<CustTurnover>
                {
                    DataList = new List<CustTurnover>
                    {
                        new CustTurnover()
                    },
                    TotalData = new CustTurnover(),
                    TotalRows = 999
                };
                _customerTurnoverService.GetDataList(metaModel, UserCurrencyList, SessionService.UserSession.Language).Returns(data);

                // Action
                var result = controller.CustomerTurnover(metaModel) as JsonResult;
                var resultData = ObjectHelper.ToDictionary(result.Data);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(metaModel.AgentListXml, Is.EqualTo(AgentListXml));
                    Assert.That(resultData["Data"], Is.EqualTo(data.DataList));
                    Assert.That(resultData["Total"], Is.EqualTo(data.TotalData));
                    Assert.That(resultData["TotalRow"], Is.EqualTo(data.TotalRows));
                });
            }
        }

        private ReportAPIController CreateController()
        {
            return new ReportAPIController(
                SessionService,
                AppSettings,
                _betTypeService,
                _customerTurnoverService,
                _turnoverService);
        }
    }
}