namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using Model.Core.DTO.Configuration;
    using NUnit.Framework;

    [TestFixture]
    public class SubLicenseePerformanceDataAccessTest
    {
        private SubLicenseePerformanceDataAccess _dataAccess;
        private BalanceUp _balance;

        [OneTimeSetUp]
        public void SetUp()
        {
            _balance = new BalanceUp
            {
                MoveBetsDay = new DateTime(2017, 8, 25),
                BalanceUpDay = new DateTime(2017, 8, 31)
            };
            _dataAccess = new SubLicenseePerformanceDataAccess(_balance);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _balance = null;
            _dataAccess = null;
        }
    }
}