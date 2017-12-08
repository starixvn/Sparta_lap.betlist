namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Data;
    using System.Linq;
    using Core.Configuration.Interfaces;
    using Data;
    using Model.Core.DTO.Configuration;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class LicenseeWinLossByTimeZoneDataAccessTest
    {
        private LicenseeWinLossByTimeZoneDataAccess _dataAccess;
        private IBalanceUpDataAccess _balanceUpDataAccess;
        private IObjectDb _objectDb;
        private IDataReader _dataReader;
        private BalanceUp _balance;

        [SetUp]
        public void SetUp()
        {
            _balance = new BalanceUp
            {
                MoveBetsDay = new DateTime(2017, 8, 25),
                BalanceUpDay = new DateTime(2017, 8, 31)
            };

            _balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _balanceUpDataAccess.GetBalanceUp().Returns(_balance);
            _dataAccess = Substitute.ForPartsOf<LicenseeWinLossByTimeZoneDataAccess>(_balanceUpDataAccess);

            _objectDb = Substitute.For<IObjectDb>();
            _dataReader = Substitute.For<IDataReader>();

            const string SpName = "Lrf_Rpt_LicWinlossByTimeZone";
            _dataAccess.GetObjectDb(Arg.Is(SpName)).Returns(_objectDb);
            _objectDb.ExecuteReader().Returns(_dataReader);
        }

        [TearDown]
        public void TearDown()
        {
            _balanceUpDataAccess = null;
            _balance = null;
            _dataAccess = null;
            _objectDb = null;
        }

        [Test]
        public void GetData_WhenCall_PassCorrectParams()
        {
            // Arrange
            _dataReader.Read().Returns(false);

            var metaModel = GetDefaultMetaModel();

            // Action
            _dataAccess.GetData(metaModel);

            // Assert
            AssertParamsPassCorrectly(metaModel);
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var metaModel = GetDefaultMetaModel();
            _dataReader.Read().Returns(true, false);
            DataAccessTestHelper.MapReaderData(
                _dataReader,
                new
                {
                    ReportBy = "20171122"
                });

            // Action
            var result = _dataAccess.GetData(metaModel);

            // Assert
            Assert.That(result.DataList.ElementAt(0).ReportBy.Date, Is.EqualTo(new DateTime(2017, 11, 22)));
        }

        private static LicenseeWinLossByTimeZoneMetaModel GetDefaultMetaModel()
        {
            return new LicenseeWinLossByTimeZoneMetaModel
            {
                BaseCurrency = "EUR",
                FromDate = new DateTime(2017, 1, 1),
                ToDate = new DateTime(2017, 2, 2),
                ProductCodeList = "SB,BA",
                AgentListXml = "<Param></Param>",
                TimeZone = -4,
                IsIncludeDrawData = true,
                ReportBy = 2,
                GroupBy = 1
            };
        }

        private void AssertParamsPassCorrectly(LicenseeWinLossByTimeZoneMetaModel metaModel)
        {
            _objectDb.Received().AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
            _objectDb.Received().AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
            _objectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, _balance.MoveBetsDay);
            _objectDb.Received().AddInParameter("ProductList", DbType.String, metaModel.ProductCodeList);
            _objectDb.Received().AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("TimeZone", DbType.Int16, metaModel.TimeZone);
            _objectDb.Received().AddInParameter("ReportBy", DbType.Byte, metaModel.ReportBy);
            _objectDb.Received().AddInParameter("GroupBy", DbType.Byte, metaModel.GroupBy);
            _objectDb.Received().AddInParameter("IncludeDraw", DbType.Boolean, metaModel.IsIncludeDrawData);
        }
    }
}