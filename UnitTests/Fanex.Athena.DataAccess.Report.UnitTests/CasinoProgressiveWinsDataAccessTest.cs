namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core.Configuration.Interfaces;
    using Core.UnitTests.TestHelpers;
    using Model.Core.DTO.Configuration;
    using Model.Core.MetaModel.Product;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CasinoProgressiveWinsDataAccessTest : BaseDataAccessHelperTest<CasinoProgressiveWinsDataAccess>
    {
        private const string SpName = "Lrf_Rpt_CasinoProgressiveWins";

        private readonly BaseReportMetaModel _metaModel = new BaseReportMetaModel
        {
            LicenseeId = 1,
            UserId = 2,
            CurrencyId = 3,
            ProductOptions = new List<ProductOption>
            {
                new ProductOption { Code = "SB", Status = true },
                new ProductOption { Code = "NG", Status = false }
            }
        };

        protected override void SetUpDataAccess()
        {
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            var balanceUp = new BalanceUp
            {
                MoveBetsDay = new DateTime(2016, 8, 10),
                BalanceUpDay = new DateTime(2016, 8, 30)
            };
            balanceUpDataAccess.GetBalanceUp().Returns(balanceUp);

            DataAccess = Substitute.ForPartsOf<CasinoProgressiveWinsDataAccess>(
                AppSetting,
                CreateSessionStorage(),
                balanceUpDataAccess);
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);
        }

        [Test]
        public void GetDataList_WhenCall_ParametersPassedCorrectly()
        {
            // Action
            DataAccess.GetDataList(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                ObjectDb.Received(1).AddInParameter("fromdate", DbType.Date, _metaModel.FromDate.Date);
                ObjectDb.Received(1).AddInParameter("todate", DbType.Date, _metaModel.ToDate.Date);
                ObjectDb.Received(1).AddInParameter("movebetsday", DbType.DateTime, new DateTime(2016, 8, 10));
                ObjectDb.Received(1).AddInParameter("BalanceUpDay", DbType.DateTime, new DateTime(2016, 8, 30));
                ObjectDb.Received(1).AddInParameter("showSB", DbType.Boolean, true);
                ObjectDb.Received(1).AddInParameter("showNG", DbType.Boolean, false);
            });
        }

        [Test]
        public void GetDataList_WhenCall_DateReturnedCorrectly()
        {
            // Arrange
            var expected = new CasinoProgressiveWins
            {
                SystemId = "SystemIdTest",
                ExternalId = "ExternalIdTest",
                RefNo = 1,
                WinAmount = 10m,
                BetType = 2,
                SportType = 3,
                TransDesc = "TransDescTest",
                CurrencyId = 4,
                CustId = 5,
                GameId = "GameIdTest"
            };
            ConfigureReaderForGetDataListMethod(expected);

            // Action
            var result = DataAccess.GetDataList(_metaModel).ElementAt(0);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.SystemId, Is.EqualTo(expected.SystemId));
                Assert.That(result.ExternalId, Is.EqualTo(expected.ExternalId));
                Assert.That(result.RefNo, Is.EqualTo(expected.RefNo));
                Assert.That(result.WinAmount, Is.EqualTo(expected.WinAmount));
                Assert.That(result.BetType, Is.EqualTo(expected.BetType));
                Assert.That(result.SportType, Is.EqualTo(expected.SportType));
                Assert.That(result.TransDesc, Is.EqualTo(expected.TransDesc));
                Assert.That(result.CurrencyId, Is.EqualTo(expected.CurrencyId));
                Assert.That(result.CustId, Is.EqualTo(expected.CustId));
                Assert.That(result.GameId, Is.EqualTo(expected.GameId));
            });
        }

        [Test]
        public void GetDataList_WhenObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                SpName,
                () => DataAccess.GetDataList(_metaModel));
        }

        private void ConfigureReaderForGetDataListMethod(CasinoProgressiveWins expected)
        {
            _metaModel.BaseCurrency = "UUS";
            Reader.Read().Returns(true, false);

            Reader["WinlostDate"].Returns(expected.WinlostDate);
            Reader["SystemId"].Returns(expected.SystemId);
            Reader["ExternalId"].Returns(expected.ExternalId);
            Reader["RefNo"].Returns(expected.RefNo);
            Reader["WinAmountuus"].Returns(expected.WinAmount);
            Reader["BetType"].Returns(expected.BetType);
            Reader["SportType"].Returns(expected.SportType);
            Reader["TransDesc"].Returns(expected.TransDesc);
            Reader["Currency"].Returns(expected.CurrencyId);
            Reader["CustId"].Returns(expected.CustId);
            Reader["TranId3rd"].Returns(expected.GameId);
        }
    }
}