namespace Fanex.Athena.Extension.Tests.Utility
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
            //Arrange
            const string cookieName = "myCookie";
            const string cookieValue = "myValue";

            HttpContext.Current = GetHttpContext();
            HttpContext.Current.Request.Cookies.Add(new HttpCookie(cookieName, cookieValue));

            //Action
            var result = CookiesHelper.GetCookies(cookieName);

            //Assert
            Assert.AreEqual(cookieValue, result);
        }

        [Test]
        public void GetCookies_CookieNameNotExist_ReturnEmptyString()
        {
            //Arrange
            const string cookieName = "myCookie";
            var expected = string.Empty;

            HttpContext.Current = GetHttpContext();

            //Action
            var result = CookiesHelper.GetCookies(cookieName);

            //Assert
            Assert.AreEqual(expected, result);
        }

        private HttpContext GetHttpContext()
        {
            return new HttpContext(
                new HttpRequest(null, "http://tempuri.org", null),
                new HttpResponse(null));
        }
    }
}