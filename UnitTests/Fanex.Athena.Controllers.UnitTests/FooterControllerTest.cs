namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Web.Mvc;
    using NUnit.Framework;

    public class FooterControllerTest
    {
        [Test]
        public void Index_WhenCall_ReturnCorrectViewName()
        {
            using (var controller = new FooterController())
            {
                // Action
                var result = controller.Index() as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/Footer/Index.cshtml"));
            }
        }
    }
}
