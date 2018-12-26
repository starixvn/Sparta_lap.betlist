namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System.Collections.Generic;
    using Base.UnitTests;
    using DataAccess.BetList.Interfaces;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.StatelessCore.Interfaces;
    using Session.Model;

    public class OutstandingDailyStatementBetListControllerTest : BaseApiControllerHelperTest
    {
        private IAuthorizationService _authorizationService;

        private IDailyStatementBetListDataAccess _dailyStatementBetListDataAccess;

        private OutstandingDailyStatementBetListController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _authorizationService = Substitute.For<IAuthorizationService>();
            _dailyStatementBetListDataAccess = Substitute.For<IDailyStatementBetListDataAccess>();

            _controller = Substitute.ForPartsOf<OutstandingDailyStatementBetListController>(
                SessionService,
                ProductService,
                LangService,
                _authorizationService,
                _dailyStatementBetListDataAccess);

            StubUserSessionData(new UserSession { UserId = SessionUserId });

            StubLicenseeSessionData(new LicenseeSession { LicenseeId = SessionLicenseeId });
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _authorizationService = null;
            _dailyStatementBetListDataAccess = null;
            _controller = null;
        }

        [Test]
        public void Index_NoPermission_ReturnEmptyData()
        {
            // Arrange
            var metaModel = new BetListMetaModel { CustId = 1, TransId = 1 };

            _authorizationService.HasPermissionOnData(SessionLicenseeId, metaModel.CustId, metaModel.TransId).Returns(false);

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Total.SumWinLoss, Is.EqualTo(0));
                Assert.That(result.Total.SumWinLoss, Is.EqualTo(0));

                Assert.That(result.Main, Is.Empty);
            });
        }

        [Test]
        public void Index_HasPermission_ReturnCorrectData()
        {
            // Arrange
            var metaModel = new BetListMetaModel { CustId = 1, TransId = 1 };

            _authorizationService.HasPermissionOnData(SessionLicenseeId, metaModel.CustId, metaModel.TransId).Returns(true);

            var dailyStatement = new List<DailyStatement> { new DailyStatement { Index = 1 } };
            _dailyStatementBetListDataAccess.GetDailyStatementData(metaModel, Arg.Any<bool>()).Returns(dailyStatement);

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Total.SumWinLoss, Is.EqualTo(0));
                Assert.That(result.Total.SumWinLoss, Is.EqualTo(0));

                Assert.That(result.Main, Is.EqualTo(dailyStatement));
            });
        }
    }
}