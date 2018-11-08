namespace Fanex.Athena.Controllers.BetList.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.DataAccess.BetList.Interfaces;
    using Fanex.BetList.Core.Entities;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.BetList.ViewModel;
    using NSubstitute;
    using NUnit.Framework;

    internal class CashOutBetListControllerTest : BetListBaseControllerHelperTest
    {
        private ICashOutBetListDataAccess _cashOutBetListDataAccess;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _cashOutBetListDataAccess = Substitute.For<ICashOutBetListDataAccess>();

            SetUpDefaultFakeData();
        }

        [Test]
        public void Detail_WhenCall_ReturnCorrectView()
        {
            using (var controller = CreateController())
            {
                // Arrang
                var metaModel = new CashOutBetListDetailMetaModel { TransId = 2 };

                _cashOutBetListDataAccess.GetDetail(metaModel).Returns(new CashOutBetListDetailViewModel
                {
                    OriginalTicket = new LrfTicket { TransId = metaModel.TransId, Status = "running" },
                    CashOutTicketList = new List<CashOutTicket>()
                });

                // Action
                var result = controller.Detail(metaModel) as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/BetList/CashOutDetail/Main.cshtml"));
            }
        }

        [Test]
        public void Detail_WhenCall_ReturnCorrectViewModel()
        {
            using (var controller = CreateController())
            {
                // Arrang
                var metaModel = new CashOutBetListDetailMetaModel { TransId = 2, IsCashOut = true };

                _cashOutBetListDataAccess.GetDetail(metaModel).Returns(new CashOutBetListDetailViewModel
                {
                    OriginalTicket = new LrfTicket { TransId = metaModel.TransId, Status = "running" },
                    CashOutTicketList = new List<CashOutTicket>()
                });

                // Action
                var result = controller.Detail(metaModel) as ViewResult;
                var model = (CashOutBetListDetailViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(model.IsCashOut, Is.True);
                    Assert.That(model.IsHighlightOriginalTicket, Is.True);
                    Assert.That(model.CashOutTicketTextList, Is.Empty);
                });
            }
        }

        [Test]
        public void Detail_WhenCall_ReturnCorrectOriginalTicketText()
        {
            using (var controller = CreateController())
            {
                var metaModel = new CashOutBetListDetailMetaModel { TransId = 2 };

                _cashOutBetListDataAccess.GetDetail(metaModel).Returns(new CashOutBetListDetailViewModel
                {
                    OriginalTicket = new LrfTicket { TransId = metaModel.TransId, Status = "running", ExternalId = "Test", UserName = "Test" },
                    CashOutTicketList = new List<CashOutTicket>()
                });

                // Action
                var result = controller.Detail(metaModel) as ViewResult;
                var model = (CashOutBetListDetailViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(model.OriginalTicketText.ExternalId, Is.EqualTo(model.OriginalTicket.ExternalId));
                    Assert.That(model.OriginalTicketText.SystemId, Is.EqualTo(model.OriginalTicket.UserName));
                });
            }
        }

        [Test]
        public void Detail_WhenCall_ReturnCorrectCashOutTicketText()
        {
            using (var controller = CreateController())
            {
                // Arrang
                var metaModel = new CashOutBetListDetailMetaModel { TransId = 2 };

                _cashOutBetListDataAccess.GetDetail(metaModel).Returns(new CashOutBetListDetailViewModel
                {
                    OriginalTicket = new LrfTicket { TransId = metaModel.TransId, Status = "running" },
                    CashOutTicketList = new List<CashOutTicket> { new CashOutTicket { Stake = 1, Price = 2, WinLoss = 3, Status = "running" } }
                });

                // Action
                var result = controller.Detail(metaModel) as ViewResult;
                var model = (CashOutBetListDetailViewModel)result.Model;
                var firstCashOutTicketText = model.CashOutTicketTextList.ElementAt(0);
                var firstCashOutTicket = model.CashOutTicketList.ElementAt(0);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(firstCashOutTicketText.Stake, Is.EqualTo("1.00"));
                    Assert.That(firstCashOutTicketText.Price, Is.EqualTo("2.00"));
                    Assert.That(firstCashOutTicketText.WinLoss, Is.EqualTo("3.00"));
                });
            }
        }

        [TestCase(1, 900, false)]
        [TestCase(1, 912, false)]
        [TestCase(0, 913, false)]
        [TestCase(1, 913, true)]
        public void Detail_WhenCall_ReturnCorrectIsHighlightOriginalTicket(long betdaqid, int betType, bool expected)
        {
            using (var controller = CreateController())
            {
                // Arrang
                var metaModel = new CashOutBetListDetailMetaModel { TransId = 2, BetdaqId = betdaqid, BetType = betType };

                _cashOutBetListDataAccess.GetDetail(metaModel).Returns(new CashOutBetListDetailViewModel
                {
                    OriginalTicket = new LrfTicket { TransId = 3, Status = "running" },
                    CashOutTicketList = new List<CashOutTicket>()
                });

                // Action
                var result = controller.Detail(metaModel) as ViewResult;
                var model = (CashOutBetListDetailViewModel)result.Model;

                // Assert
                Assert.That(model.IsHighlightOriginalTicket, Is.EqualTo(expected));
            }
        }

        private CashOutBetListController CreateController()
        {
            var controller = Substitute.ForPartsOf<CashOutBetListController>(
                LangService,
                SessionService,
                AppSettings,
                ProductService,
                BetListService,
                BalanceUpDataAccess,
                CacheRefAdapter,
                _cashOutBetListDataAccess,
                Substitute.For<ITicketHelper>());
            return controller;
        }
    }
}