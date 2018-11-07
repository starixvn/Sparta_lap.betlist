namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Web.Mvc;
    using Base.UnitTests;
    using Customer;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Customer.Interfaces;
    using UnitTest.Common.Extensions;

    [TestFixture]
    public partial class SearchCustomerApiControllerTest : BaseControllerHelperTest
    {
        [Test]
        public void SearchCustomer_WhenCall_ReturnCorrectData()
        {
            var customerListService = Substitute.For<ICustomerListService>();
            using (var controller = new SearchCustomerAPIController(
               LangService,
               SessionService,
               AppSettings,
               customerListService))
            {
                // Arrange
                const string Query = " test   ";
                const string AgentListXml = "Agent List XML";
                var expectedCustomerNames = new[] { "test1", "test2" };

                SetUpDefaultFakeData();
                SessionService.GenerateAgentListXml().Returns(AgentListXml);
                customerListService.SearchCustomers(0, string.Empty, string.Empty)
                    .ReturnsForAnyArgs(expectedCustomerNames);

                // Action
                var result = controller.SearchCustomer(Query) as JsonResult;
                var resultData = result.Data as object;

                // Assert
                Assert.Multiple(() =>
                {
                    customerListService.Received(1).SearchCustomers(
                        SessionService.UserSession.UserId,
                        Query.Trim(),
                        AgentListXml);
                    Assert.That(resultData.GetValue("query"), Is.EqualTo(Query.Trim()));
                    Assert.That(resultData.GetValue("suggestions"), Is.EqualTo(expectedCustomerNames));
                });
            }
        }
    }
}