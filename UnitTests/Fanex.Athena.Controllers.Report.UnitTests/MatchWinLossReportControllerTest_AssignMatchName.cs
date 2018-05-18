namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System;
    using System.Data;
    using Base.UnitTests;
    using Constant.DataDomain;
    using DataAccess.Core.Configuration.Interfaces;
    using NSubstitute;
    using NUnit.Framework;
    using Report;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Report.Interfaces;
    using UnitTest.Common.Helpers;
    using VendorAdapter.Interfaces;

    [TestFixture]
    public class MatchWinLossReportControllerTest_AssignMatchName : BaseControllerHelperTest
    {
        private ICacheRefAdapter _cacheRefAdapter;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            _cacheRefAdapter = Substitute.For<ICacheRefAdapter>();

            base.FixtureSetUp();
        }

        [Test]
        public void AssignMatchName_Racing_AssignCorrectMatchName()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var row = CreateDataRow(ProductTypes.Racing);
                row["race"] = FakeHelper.Create<string>();

                // Action
                controller.AssignMatchName(row, ProductTypes.Racing);

                // Assert
                Assert.That(row["matchname"], Is.EqualTo(ViewResources.Race + row["race"]));
            }
        }

        [Test]
        public void AssignMatchName_NumberGame_AssignCorrectMatchName()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var row = CreateDataRow(ProductTypes.NumberGame);
                row["matchcode"] = FakeHelper.Create<string>();

                // Action
                controller.AssignMatchName(row, ProductTypes.NumberGame);

                // Assert
                Assert.That(row["matchname"], Is.EqualTo(ViewResources.NumberGameNo + " " + row["matchcode"]));
            }
        }

        [Test]
        public void AssignMatchName_VirtualHrBettypes_AssignCorrectMatchName()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const int Bettype = 1231;
                var row = CreateDataRow(Bettype);
                row["KickoffTime"] = new DateTime(2018, 1, 2, 3, 4, 5);

                // Action
                controller.AssignMatchName(row, Bettype);

                // Assert
                Assert.That(row["matchname"], Is.EqualTo("01/02/2018 03:04:05 AM"));
            }
        }

        [TestCase(false, "Team 1-vs-Team 2")]
        [TestCase(true, "Team 1 (N)-vs-Team 2")]
        public void AssignMatchName_OtherProducts_AssignCorrectMatchName(bool isNeutral, string expectedMatchName)
        {
            using (var controller = CreateController())
            {
                // Arrange
                const int Bettype = -1;
                var row = CreateDataRow(Bettype);
                row["homeid"] = 1;
                row["isneutral"] = isNeutral;
                row["awayid"] = 2;
                _cacheRefAdapter.GetTeamName(Arg.Any<object>()).Returns(x => "Team " + x[0]);

                // Action
                controller.AssignMatchName(row, Bettype);

                // Assert
                Assert.That(row["matchname"], Is.EqualTo(expectedMatchName));
            }
        }

        private static DataRow CreateDataRow(int newProductId)
        {
            var table = new DataTable();
            table.Columns.Add("newproductid", typeof(int));
            table.Columns.Add("race", typeof(string));
            table.Columns.Add("matchcode", typeof(string));
            table.Columns.Add("KickoffTime", typeof(DateTime));
            table.Columns.Add("homeid", typeof(int));
            table.Columns.Add("isneutral", typeof(bool));
            table.Columns.Add("awayid", typeof(int));
            table.Columns.Add("matchname", typeof(string));
            var row = table.NewRow();
            row["newproductid"] = newProductId;
            return row;
        }

        private MatchWinLossReportController CreateController(string productCodeList = null)
        {
            var controller = Substitute.ForPartsOf<MatchWinLossReportController>(
                LangService,
                SessionService,
                AppSettings,
                ClientViewHelper,
                Substitute.For<IProductService>(),
                Substitute.For<IBalanceUpDataAccess>(),
                _cacheRefAdapter,
                Substitute.For<IMatchWinLossService>());
            return controller;
        }
    }
}