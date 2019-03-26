namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Web.Mvc;
    using Base.UnitTests;
    using Customer;
    using DataAccess.Customer.Interfaces;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Extensions;

    [TestFixture]
    public partial class SearchCustomerApiControllerTest : CultureBaseControllerHelperTest
    {
        [Test]
        public void SearchCustomer_WhenCall_ReturnCorrectData()
        {
            var searchCustomerDataAccess = Substitute.For<ISearchCustomerDataAccess>();
            using (var controller = new SearchCustomerAPIController(
               LangService,
               SessionService,
               AppSettings,
               searchCustomerDataAccess))
            {
                // Arrange
                const string Query = " test   ";
                const string AgentListXml = "Agent List XML";
                var expectedCustomerNames = new[] { "test1", "test2" };

                SetUpDefaultFakeData();
                SessionService.GenerateAgentListXml().Returns(AgentListXml);
                searchCustomerDataAccess.SearchCustomers(0, string.Empty, string.Empty, false)
                    .ReturnsForAnyArgs(expectedCustomerNames);

                // Action
                var result = controller.SearchCustomer(Query) as JsonResult;
                var resultData = result.Data as object;

                // Assert
                Assert.Multiple(() =>
                {
                    searchCustomerDataAccess.Received(1).SearchCustomers(
                        SessionService.UserSession.UserId,
                        Query.Trim(),
                        AgentListXml,
                        false);
                    Assert.That(resultData.GetValue("query"), Is.EqualTo(Query.Trim()));
                    Assert.That(resultData.GetValue("suggestions"), Is.EqualTo(expectedCustomerNames));
                });
            }
        }
    }
}