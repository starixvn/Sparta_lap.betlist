namespace Fanex.Athena.Controllers.Customer.UnitTests
{
    using System.Web.Mvc;
    using NUnit.Framework;

    public partial class CustomerLogControllerTest
    {
        [Test]
        public void GroupCommission_WhenCall_ReturnCorrectView()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var expectedViewName = "~/Views/Customer/CustomerLog/GroupCommLog/Main.cshtml";

                // Action
                var result = controller.GroupCommission() as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo(expectedViewName));
            }
        }
    }
}