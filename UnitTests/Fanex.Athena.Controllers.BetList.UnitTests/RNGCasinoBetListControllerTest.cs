namespace Fanex.Athena.Controllers.BetList.UnitTests
{
    using System;
    using System.Collections;
    using System.Web.Mvc;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;

    public class RNGCasinoBetListControllerTest : BetListBaseControllerHelperTest
    {
        private static IEnumerable BetListTitleTestCase
        {
            get
            {
                const long TransTypeYesterday = 0;
                const long TransTypeTodayRunning = 3;
                var title = $"{ViewResources.ProductCS} - {ViewResources.BetList} - {ViewResources.DetailTickets} - ";

                yield return new TestCaseData(TransTypeYesterday, title + ViewResources.Yesterday);
                yield return new TestCaseData(TransTypeTodayRunning, title + ViewResources.Today + " - " + ViewResources.Running);
                yield return new TestCaseData(1, title + ViewResources.Today + " - " + ViewResources.Settled);
            }
        }

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            SetUpDefaultFakeData();
        }

        [Test]
        public void Detail_WhenCall_ReturnCorrectView()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = controller.Detail(new BetListMetaModel()) as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/RngCasinoDetail/Main.cshtml"));
            }
        }

        [Test]
        public void Detail_WhenCall_ReturnCorrectViewModel()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel
                {
                    FromDate = new DateTime(2018, 2, 1),
                    ToDate = new DateTime(2018, 2, 10),
                    Date = new DateTime(2018, 2, 5)
                };

                // Action
                var result = controller.Detail(metaModel) as ViewResult;
                var model = (BetListMetaModel)result.Model;

                // Assert
                AssertDefaultBetListViewModel(model, metaModel);
            }
        }

        [TestCaseSource(nameof(BetListTitleTestCase))]
        public void Detail_WhenCall_ReturnCorrectViewBag(long transType, string expectedTitle)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel
                {
                    TransType = transType
                };

                // Action
                var result = controller.Detail(metaModel) as ViewResult;

                // Assert
                Assert.That(result.ViewBag.BetListTitle, Is.EqualTo(expectedTitle));
            }
        }

        private RNGCasinoBetListController CreateController()
        {
            var controller = Substitute.ForPartsOf<RNGCasinoBetListController>(
                LangService,
                SessionService,
                AppSettings,
                ClientViewHelper,
                AuthorizationService,
                ProductService,
                BetListService,
                BalanceUpDataAccess,
                CacheRefAdapter);
            return controller;
        }
    }
}