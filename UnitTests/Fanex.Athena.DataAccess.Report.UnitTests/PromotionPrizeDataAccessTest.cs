namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Linq;
    using Core.Configuration.Interfaces;
    using Core.UnitTests.TestHelpers;
    using Data;
    using Model.Core.DTO.Configuration;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public class PromotionPrizeDataAccessTest : BaseDataAccessHelperTest<PromotionPrizeDataAccess>
    {
        private const string USp = "Lrf_Rpt_PP_PromotionPrize";

        private readonly BalanceUp _balanceUp = FakeHelper.Create<BalanceUp>();

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            ObjectDb = Substitute.For<IObjectDb>();

            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            balanceUpDataAccess.GetBalanceUp().Returns(_balanceUp);

            DataAccess = Substitute.ForPartsOf<PromotionPrizeDataAccess>(balanceUpDataAccess);
            DataAccess.GetObjectDb(Arg.Is(USp)).Returns(ObjectDb);
        }

        [Test]
        public void GetData_WhenCall_ParametersPassedCorrectly()
        {
            // Assert
            var metaModel = FakeHelper.Create<PromotionPrizeMetaModel>();

            // Action
            DataAccess.GetData(metaModel, UserCurrencyList);

            // Assert
            Assert.Multiple(() =>
            {
                DataAccess.Received(1).GetObjectDb(USp);
                ObjectDb.Received(1).SetParameterValue("AgentId", metaModel.AgentId);
                ObjectDb.Received(1).SetParameterValue("UserName", metaModel.UserName);
                ObjectDb.Received(1).SetParameterValue("FromDate", metaModel.FromDate);
                ObjectDb.Received(1).SetParameterValue("ToDate", metaModel.ToDate);
                ObjectDb.Received(1).SetParameterValue("MoveBetsDay", _balanceUp.MoveBetsDay);
                ObjectDb.Received(1).SetParameterValue("AgentList", metaModel.AgentListXml);
            });
        }

        [Test]
        public void GetData_WhenCall_ReturnMainDataCorrectly()
        {
            // Assert
            var metaModel = FakeHelper.Create<PromotionPrizeMetaModel>();
            var expectedData = new
            {
                CurrencyId,
                UserName = FakeHelper.Create<string>(),
                NickName = FakeHelper.Create<string>(),
                Amount = FakeHelper.Create<decimal>(),
                Date = FakeHelper.Create<DateTime>()
            };
            MapReaderData(expectedData);
            Reader.Read().Returns(true, false, false);

            // Action
            var actual = DataAccess.GetData(metaModel, UserCurrencyList);
            var item = actual.DataList.ElementAt(0);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(item.CurrencyName, Is.EqualTo(CurrencyName));
                Assert.That(item.SystemId, Is.EqualTo(expectedData.UserName));
                Assert.That(item.ExternalId, Is.EqualTo(expectedData.NickName));
                Assert.That(item.Amount, Is.EqualTo(expectedData.Amount));
                Assert.That(item.Date.Date, Is.EqualTo(expectedData.Date.Date));
                Assert.That(actual.TotalRows, Is.EqualTo(actual.DataList.Count()));
            });
        }

        [Test]
        public void GetData_WhenCall_ReturnSummaryDataCorrectly()
        {
            // Assert
            var metaModel = FakeHelper.Create<PromotionPrizeMetaModel>();
            var expectedData = new
            {
                SumAmountUSD = FakeHelper.Create<decimal>(),
                SumAmountEUR = FakeHelper.Create<decimal>(),
                SumAmountGBP = FakeHelper.Create<decimal>()
            };
            MapReaderData(expectedData);
            Reader.Read().Returns(false, true);

            // Action
            var actual = DataAccess.GetData(metaModel, UserCurrencyList);
            var item = actual.SummaryTotal;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(item["USD"].Amount, Is.EqualTo(expectedData.SumAmountUSD));
                Assert.That(item["EUR"].Amount, Is.EqualTo(expectedData.SumAmountEUR));
                Assert.That(item["GBP"].Amount, Is.EqualTo(expectedData.SumAmountGBP));
            });
        }
    }
}