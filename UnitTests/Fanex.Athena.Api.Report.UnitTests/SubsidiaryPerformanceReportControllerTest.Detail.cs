namespace Fanex.Athena.Api.Report.UnitTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Base.UnitTests;
    using Model.Core.ResultModel;
    using Model.Report.DTO.SubsidiaryPerformance;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public partial class SubsidiaryPerformanceReportControllerTest : BaseApiControllerHelperTest
    {
        [Test]
        public async Task Detail_InvalidMetaModel_ReturnEmptyList()
        {
            // Arrange
            var metaModel = new SubsidiaryPerformanceMetaModel
            {
                ProductCodeList = string.Empty
            };

            // Action
            var result = await _controller.Detail(metaModel);

            // Assert
            Assert.That(result.Data, Is.Empty);
        }

        [Test]
        public async Task Detail_EmptyAgentList_ThrowException()
        {
            // Arrange
            var metaModel = GetMetaModel();
            _subsidiaryService.GetCurrencyAgentList(Arg.Any<string>(), Arg.Any<bool>()).Returns(new Dictionary<int, IEnumerable<int>>());

            // Action
            var result = await _controller.Detail(metaModel);

            // Assert
            Assert.That(result.Data, Is.Empty);
        }

        [Test]
        public async Task Detail_WhenCall_ReturnDataFromService()
        {
            // Arrange
            const string AgentListXml = "<sml>So Much Lie</sml>";
            var metaModel = GetMetaModel();
            var agentList = new Dictionary<int, IEnumerable<int>>
            {
                { 1, new[] { 100 } }
            };
            var resultData = new ReportResultModel<SubsidiaryPerformanceDetail>
            {
                DataList = new SubsidiaryPerformanceDetail[] { new SubsidiaryPerformanceDetail() },
                SummaryTotal = new Dictionary<string, SubsidiaryPerformanceDetail> { { "abc", new SubsidiaryPerformanceDetail() } },
                TotalData = new SubsidiaryPerformanceDetail()
            };
            _subsidiaryService.GetCurrencyAgentList(Arg.Any<string>(), Arg.Any<bool>()).Returns(agentList);
            SessionService.GenerateAgentListXml(agentList).Returns(AgentListXml);
            _service.GetDetailData(metaModel, Arg.Any<IDictionary<int, string>>()).Returns(resultData);

            // Action
            var result = await _controller.Detail(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(metaModel.AgentListXml, Is.EqualTo(AgentListXml));
                Assert.That(result.Data, Is.EqualTo(resultData.DataList));
                Assert.That(result.Total, Is.EqualTo(resultData.SummaryTotal));
                Assert.That(result.GrandTotal, Is.EqualTo(resultData.TotalData));
            });
        }
    }
}