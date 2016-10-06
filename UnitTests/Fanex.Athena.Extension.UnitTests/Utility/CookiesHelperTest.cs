namespace Fanex.Athena.Extension.UnitTests.Utility
{
    using System.Web;
    using Extension.Utility;
    using NUnit.Framework;

    [TestFixture]
    public class CookiesHelperTest
    {
        [Test]
        public void GetCookies_CookieNameExist_ReturnCorrectData()
        {
            // Arrange
            const string CookieName = "myCookie";
            const string CookieValue = "myValue";

            HttpContext.Current = GetHttpContext();
            HttpContext.Current.Request.Cookies.Add(new HttpCookie(CookieName, CookieValue));

            // Action
            var result = CookiesHelper.GetCookies(CookieName);

            // Assert
            Assert.That(result, Is.EqualTo(CookieValue));
        }

        [Test]
        public void GetCookies_CookieNameNotExist_ReturnEmptyString()
        {
            // Arrange
            const string CookieName = "myCookie";
            var expected = string.Empty;

            HttpContext.Current = GetHttpContext();

            // Action
            var result = CookiesHelper.GetCookies(CookieName);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        private HttpContext GetHttpContext()
        {
            return new HttpContext(
                new HttpRequest(null, "http://tempuri.org", null),
                new HttpResponse(null));
        }
    }
}