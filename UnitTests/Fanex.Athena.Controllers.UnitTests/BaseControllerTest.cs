using Fanex.Athena.Extension.Utility;
using Fanex.Athena.Model.Core.DataDomain;
using Fanex.Athena.Service.Core.Common.Interfaces;
using Fanex.Athena.Service.Core.Users;
using Fanex.Athena.Service.Core.Users.Interfaces;
using Fanex.Athena.Web.RouterConfig;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fanex.Athena.Controllers.UnitTests
{
    [TestFixture]
    public class BaseControllerTest
    {
        protected UrlHelper Url { get; set; }

        protected ControllerContext controllerContext { get; set; }

        protected ILanguageService LangService;
        protected ISessionService SessionService;
        protected IAppSettings AppSettings;

        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            var httpContext = Substitute.For<HttpContextBase>();
            httpContext.Request.ApplicationPath.Returns("/");

            controllerContext = Substitute.For<ControllerContext>();
            controllerContext.HttpContext = httpContext;

            var requestContext = new RequestContext(httpContext, new RouteData());

            Url = new UrlHelper(requestContext, routes);
        }

        protected void FakeBaseControllerDependency()
        {
            LangService = Substitute.For<ILanguageService>();

           
            SessionService = Substitute.For<ISessionService>();
            SessionService.UserSession.Returns(new Model.Core.DTO.Users.UserSession
            {
                ViewType = ClientViewType.None,
                UserId=1,
                Language="en-Us",
                UserName="alex.vn",
                OnUserId=1,
                FunctionIdList=new List<int>()
            });
            
            AppSettings = Substitute.For<IAppSettings>();

            AppSettings.ClientViewType.Returns("1");
        }
    }
}
