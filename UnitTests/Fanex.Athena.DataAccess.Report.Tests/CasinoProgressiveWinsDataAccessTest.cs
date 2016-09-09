namespace Fanex.Athena.DataAccess.Report.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Data;
    using Model.Core.DTO.Configuration;
    using Model.Core.MetaModel.Product;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CasinoProgressiveWinsDataAccessTest
    {
        private IObjectDb _objectDb;
        private CasinoProgressiveWinsDataAccess _dataAccess;
        private IDataReader _reader;
        private CasinoProgressiveWinsMetaModel _metaModel;

        [SetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();

            var balanceUp = new BalanceUp()
            {
                MoveBetsDay = new DateTime(2016, 8, 10),
                BalanceUpDay = new DateTime(2016, 8, 30)
            };
            _dataAccess = Substitute.ForPartsOf<CasinoProgressiveWinsDataAccess>(balanceUp);
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_CasinoProgressiveWins")).Returns(_objectDb);

            _reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader().Returns(_reader);

            _metaModel = new CasinoProgressiveWinsMetaModel
            {
                LicenseeId = 1,
                UserId = 2,
                CurrencyId = 3,
                ProductOptions = GetProductOptions()
            };
        }

        [Test]
        public void GetDataList_WhenCall_ParametersPassedCorrectly()
        {
            // Action
            _dataAccess.GetDataList(_metaModel);

            // Assert
            _objectDb.Received().AddInParameter("licenseeid", DbType.Int32, _metaModel.LicenseeId);
            _objectDb.Received().AddInParameter("userid", DbType.Int32, _metaModel.UserId);
            _objectDb.Received().AddInParameter("currencyid", DbType.Int32, _metaModel.CurrencyId);
            _objectDb.Received().AddInParameter("fromdate", DbType.Date, _metaModel.FromDate.Date);
            _objectDb.Received().AddInParameter("todate", DbType.Date, _metaModel.ToDate.Date);
            _objectDb.Received().AddInParameter("movebetsday", DbType.DateTime, new DateTime(2016, 8, 10));
            _objectDb.Received().AddInParameter("BalanceUpDay", DbType.DateTime, new DateTime(2016, 8, 30));
            _objectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
            _objectDb.Received().AddInParameter("showNG", DbType.Boolean, false);
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
            var result = _dataAccess.GetDataList(_metaModel).ElementAt(0);

            // Assert
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
        }

        private IList<ProductOption> GetProductOptions()
        {
            return new List<ProductOption>
            {
                new ProductOption { Code = "SB", Status = true },
                new ProductOption { Code = "NG", Status = false }
            };
        }

        private void ConfigureReaderForGetDataListMethod(CasinoProgressiveWins expected)
        {
            _metaModel.BaseCurrency = "UUS";
            _reader.Read().Returns(true, false);

            _reader["WinlostDate"].Returns(expected.WinlostDate);
            _reader["SystemId"].Returns(expected.SystemId);
            _reader["ExternalId"].Returns(expected.ExternalId);
            _reader["RefNo"].Returns(expected.RefNo);
            _reader["WinAmountuus"].Returns(expected.WinAmount);
            _reader["BetType"].Returns(expected.BetType);
            _reader["SportType"].Returns(expected.SportType);
            _reader["TransDesc"].Returns(expected.TransDesc);
            _reader["Currency"].Returns(expected.CurrencyId);
            _reader["CustId"].Returns(expected.CustId);
            _reader["TranId3rd"].Returns(expected.GameId);
        }
    }
}