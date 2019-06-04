namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public class ApiBetListBaseControllerTest : ApiBetListBaseControllerHelperTest
    {
        private const int DefaultPageSize = 100;

        private const int DefaultPageIndex = 1;

        [SetUp]
        public void SetUp()
        {
            MockSessionData();

            AuthorizationService.HasPermissionOnData(SessionLicenseeId, Arg.Any<int>(), Arg.Any<long>()).Returns(true);
        }

        [Test]
        public void CheckDataPermission_NoPermission_ThrowException()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var custId = FakeHelper.Create<int>();
                var transId = FakeHelper.Create<long>();

                AuthorizationService.HasPermissionOnData(SessionLicenseeId, Arg.Any<int>(), Arg.Any<long>()).Returns(false);

                // Assert
                Assert.Throws<UnauthorizedAccessException>(() => controller.CheckDataPermission(custId, transId));
            }
        }

        [Test]
        public void CheckDataPermission_HasPermission_DoNotThing()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var custId = FakeHelper.Create<int>();
                var transId = FakeHelper.Create<long>();

                // Action
                controller.CheckDataPermission(custId, transId);

                // Assert
                AuthorizationService.Received(1).HasPermissionOnData(SessionLicenseeId, custId, transId);
            }
        }

        [Test]
        public async Task GetBetListResult_WhenCall_CallBetListApiWithCorrectParameters()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var apiPath = FakeHelper.Create<string>();
                var uspKey = FakeHelper.Create<string>();
                var metaModel = new BetListMetaModel();

                var expect = new HttpResponseMessage();
                StubBetListApiPostAsync(metaModel, apiPath, expect);

                // Action
                var actual = await controller.GetBetListResult(metaModel, uspKey, apiPath);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.Multiple(() =>
                    {
                        Assert.That(expect, Is.EqualTo(actual));
                        Assert.That(metaModel.PageIndex, Is.EqualTo(1));
                        Assert.That(metaModel.PageSize, Is.EqualTo(int.MaxValue - 1));
                        Assert.That(metaModel.LicenseeId, Is.EqualTo(SessionLicenseeId));
                        Assert.That(metaModel.BalanceUpDay, Is.EqualTo(DateTime.SpecifyKind(DefaultBalanceUp.BalanceUpDay.Date, DateTimeKind.Utc)));
                        Assert.That(metaModel.MoveBetsDay, Is.EqualTo(DateTime.SpecifyKind(DefaultBalanceUp.MoveBetsDay.Date, DateTimeKind.Utc)));
                        Assert.That(metaModel.AgentListXml, Is.EqualTo(AgentListXml));
                        Assert.That(metaModel.HasPlayerCom, Is.EqualTo(SessionService.LicenseeSession.IsAllowedCustomerComm));
                        Assert.That(metaModel.CurrencyList, Is.EqualTo(UserCurrencyList));
                        Assert.That(metaModel.UspKey, Is.EqualTo(uspKey));
                        BetListApiClient.Received().PostAsync(apiPath, metaModel, SessionLanguage);
                    });
                });
            }
        }

        [TestCase(1, 50, 1, 50)]
        [TestCase(-1, 100, DefaultPageIndex, 100)]
        [TestCase(2, -1, 2, DefaultPageSize)]
        public async Task GetPagingBetListResult_WhenCall_CallBetListApiWithCorrectParameters(int pageIndex, int pageSize, int expectedPageIndex, int expectedPageSize)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var apiPath = FakeHelper.Create<string>();
                var uspKey = FakeHelper.Create<string>();
                var metaModel = new BetListMetaModel();

                var expect = new HttpResponseMessage();
                StubBetListApiPostAsync(metaModel, apiPath, expect);

                // Action
                var actual = await controller.GetPagingBetListResult(metaModel, uspKey, pageIndex, pageSize, apiPath);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.Multiple(() =>
                    {
                        Assert.That(expect, Is.EqualTo(actual));
                        Assert.That(metaModel.IsServerPaging, Is.True);
                        Assert.That(metaModel.PageIndex, Is.EqualTo(expectedPageIndex));
                        Assert.That(metaModel.PageSize, Is.EqualTo(expectedPageSize));
                        Assert.That(metaModel.LicenseeId, Is.EqualTo(SessionLicenseeId));
                        Assert.That(metaModel.BalanceUpDay, Is.EqualTo(DateTime.SpecifyKind(DefaultBalanceUp.BalanceUpDay.Date, DateTimeKind.Utc)));
                        Assert.That(metaModel.MoveBetsDay, Is.EqualTo(DateTime.SpecifyKind(DefaultBalanceUp.MoveBetsDay.Date, DateTimeKind.Utc)));
                        Assert.That(metaModel.AgentListXml, Is.EqualTo(AgentListXml));
                        Assert.That(metaModel.HasPlayerCom, Is.EqualTo(SessionService.LicenseeSession.IsAllowedCustomerComm));
                        Assert.That(metaModel.CurrencyList, Is.EqualTo(UserCurrencyList));
                        Assert.That(metaModel.UspKey, Is.EqualTo(uspKey));
                        BetListApiClient.Received().PostAsync(apiPath, metaModel, SessionLanguage);
                    });
                });
            }
        }

        private FakeApiBetListBaseController CreateController()
        {
            return new FakeApiBetListBaseController(
                SessionService,
                ProductService,
                LangService,
                AuthorizationService,
                BetListApiClient,
                BalanceUpDataAccess);
        }
    }
}