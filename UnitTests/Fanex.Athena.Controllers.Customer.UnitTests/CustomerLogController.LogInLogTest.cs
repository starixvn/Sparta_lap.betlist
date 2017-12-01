namespace Fanex.Athena.Controllers.Customer.UnitTests
{
    using System.Web.Mvc;
    using NUnit.Framework;

    public partial class CustomerLogControllerTest
    {
        [Test]
        public void LogInLog_WhenCall_ReturnCorrectView()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var expectedViewName = "~/Views/Customer/CustomerLog/LogInLog/Main.cshtml";

                // Action
                var result = controller.LogInLog() as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo(expectedViewName));
            }
        }
    }
}