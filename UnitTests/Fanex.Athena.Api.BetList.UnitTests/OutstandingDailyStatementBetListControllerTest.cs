namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Base.UnitTests;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Utility.Interfaces;
    using Service.StatelessCore.Interfaces;
    using UnitTest.Common.Helpers;

    public class OutstandingDailyStatementBetListControllerTest : BaseApiControllerHelperTest
    {
        private const string ApiPath = "/OutstandingDailyStatementBetList/GetData";

        private IAuthorizationService _authorizationService;

        private IBetListApiClient _betListApiClient;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _betListApiClient = Substitute.For<IBetListApiClient>();
            _authorizationService = Substitute.For<IAuthorizationService>();

            MockSessionData();
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _betListApiClient = null;
            _authorizationService = null;
        }

        [Test]
        public async Task Index_NoPermission_ReturnEmptyData()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<BetListMetaModel>();

                _authorizationService.HasPermissionOnData(SessionLicenseeId, metaModel.CustId, metaModel.TransId).Returns(false);

                // Action
                var result = await controller.Index(metaModel);

                // Assert
                Assert.That(result, Is.Empty);
            }
        }

        [Test]
        public async Task Index_HasPermission_CallApiWithCorrectParameters()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<BetListMetaModel>();

                _authorizationService.HasPermissionOnData(SessionLicenseeId, metaModel.CustId, metaModel.TransId).Returns(true);

                var expect = new HttpResponseMessage();
                _betListApiClient.PostAsync(ApiPath, metaModel, SessionLanguage).Returns(Task.FromResult(expect));

                // Action
                var actual = await controller.Index(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(expect, Is.EqualTo(actual));
                    _betListApiClient.Received().PostAsync(ApiPath, metaModel, SessionLanguage);
                });
            }
        }

        private OutstandingDailyStatementBetListController CreateController()
        {
            return new OutstandingDailyStatementBetListController(SessionService, ProductService, LangService, _betListApiClient, _authorizationService);
        }
    }
}