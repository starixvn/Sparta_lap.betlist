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

    public abstract class BaseApiReportControllerHelperTest
    {
        protected const int SessionLicenseeId = 1;

        protected ISessionService SessionService { get; private set; }

        protected IProductService ProductService { get; private set; }

        protected IAppSettings AppSetting { get; private set; }

        [OneTimeSetUp]
        public virtual void FixtureSetUp()
        {
            SessionService = Substitute.For<ISessionService>();
            ProductService = Substitute.For<IProductService>();
            AppSetting = Substitute.For<IAppSettings>();
        }

        [OneTimeTearDown]
        public virtual void FixtureTearDown()
        {
            SessionService = null;
            ProductService = null;
            AppSetting = null;
        }

        protected void MockSessionData()
        {
            var userSession = new UserSession
            {
                CurrencyList = new Dictionary<int, string> { { -1, "EUR" }, { -2, "USD" } },
                Language = "en"
            };

            var licenseeSession = new LicenseeSession
            {
                LicenseeId = SessionLicenseeId
            };

            StubUserSessionData(userSession);
            StubLicenseeSessionData(licenseeSession);
            SessionService.GenerateAgentListXml(Arg.Any<int>(), Arg.Any<bool>()).Returns(string.Empty);
        }

        protected void StubSingleWalletProductAccess(string[] productCode)
        {
            var productList = new List<Product>();

            for (var i = 0; i < productCode.Length; i++)
            {
                productList.Add(new Product { ProductCode = productCode[i] });
            }

            ProductService.GetSingleWalletProductAccess(Arg.Any<int>(), Arg.Any<string>(), functionConfigKey: Arg.Any<string>()).Returns(new ProductAccess(productList));
        }

        protected void StubProductAccess(string[] productCode)
        {
            var productList = new List<Product>();

            for (var i = 0; i < productCode.Length; i++)
            {
                productList.Add(new Product { ProductCode = productCode[i] });
            }

            ProductService.GetProductAccess(Arg.Any<int>(), Arg.Any<string>(), functionConfigKey: Arg.Any<string>()).Returns(new ProductAccess(productList));
        }

        protected void StubUserSessionData(UserSession userSession)
        {
            SessionService.UserSession.Returns(userSession);
        }

        protected void StubLicenseeSessionData(LicenseeSession licenseeSession)
        {
            SessionService.LicenseeSession.Returns(licenseeSession);
        }

        protected void StubGetValueAppSettings(string valueName, string value)
        {
            AppSetting.GetValue(valueName).Returns(value);
        }
    }
}