namespace Fanex.Athena.Api.Report.UnitTests
{
    using System.Collections.Generic;
    using Extension.Utility;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;

    public abstract class ApiReportBaseControllerTest
    {
        protected ISessionService SessionService { get; private set; }

        protected IProductService ProductService { get; private set; }

        protected IAppSettings AppSetting { get; private set; }

        [OneTimeSetUp]
        public virtual void SetUp()
        {
            SessionService = Substitute.For<ISessionService>();
            ProductService = Substitute.For<IProductService>();
            AppSetting = Substitute.For<IAppSettings>();
        }

        [OneTimeTearDown]
        public virtual void TearDown()
        {
            SessionService = null;
            ProductService = null;
            AppSetting = null;
        }

        public void MockSessionData()
        {
            var userSession = new UserSession
            {
                CurrencyList = new Dictionary<int, string> { { -1, "EUR" }, { -2, "USD" } },
                Language = "en"
            };

            var licenseeSession = new LicenseeSession
            {
                LicenseeId = 1
            };

            SessionService.UserSession.Returns(userSession);
            SessionService.LicenseeSession.Returns(licenseeSession);
            SessionService.GenerateAgentListXml(Arg.Any<int>(), Arg.Any<bool>()).Returns(string.Empty);
        }

        public void MockProductAccess(string[] productCode)
        {
            var productList = new List<Product>();

            for (var i = 0; i < productCode.Length; i++)
            {
                productList.Add(new Product { ProductCode = productCode[i] });
            }

            ProductService.GetSingleWalletProductAccess(Arg.Any<int>(), Arg.Any<string>(), functionConfigKey: Arg.Any<string>()).Returns(new ProductAccess(productList));
        }

        public void MockUserSessionData(UserSession userSession)
        {
            SessionService.UserSession.Returns(userSession);
        }

        public void MockLicenseeSessionData(LicenseeSession licenseeSession)
        {
            SessionService.LicenseeSession.Returns(licenseeSession);
        }

        public void MockGetValueAppSettings(string valueName, string value)
        {
            AppSetting.GetValue(valueName).Returns(value);
        }
    }
}