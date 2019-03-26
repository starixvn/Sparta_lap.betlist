namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public partial class ReportAPIControllerTest : CultureBaseControllerHelperTest
    {
        [Test]
        public void Turnover_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                SessionService.GenerateAgentListXml(CurrencyId, Arg.Any<bool>()).Returns(AgentListXml);
                var metaModel = new TurnoverMetaModel
                {
                    CurrencyId = CurrencyId
                };
                var data = new List<TurnoverDTO>
                    {
                        new TurnoverDTO()
                    };
                var total = new TurnoverDTO();
                _turnoverService.GetDataList(metaModel).Returns(data);
                _turnoverService.SumTotalData(data).Returns(total);

                // Action
                var result = controller.Turnover(metaModel) as JsonResult;
                var resultData = ObjectHelper.ToDictionary(result.Data);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(metaModel.AgentListXml, Is.EqualTo(AgentListXml));
                    Assert.That(resultData["Data"], Is.EqualTo(data));
                    Assert.That(resultData["TotalData"], Is.EqualTo(total));
                });
            }
        }
    }
}