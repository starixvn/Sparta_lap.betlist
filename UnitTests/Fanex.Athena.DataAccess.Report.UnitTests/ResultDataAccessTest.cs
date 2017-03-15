namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.Script.Serialization;
    using Data;
    using Model.Core.DTO.Configuration;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ResultDataAccessTest
    {
        private ResultDataAccess _dataAccess;
        private BalanceUp _balance;
        private ResultMetalModel _metaModel;

        [OneTimeSetUp]
        public void Setup()
        {
            _balance = new BalanceUp
            {
                MoveBetsDay = new DateTime(2016, 8, 10),
                BalanceUpDay = new DateTime(2016, 8, 31)
            };

            _dataAccess = Substitute.ForPartsOf<ResultDataAccess>(_balance);

            _metaModel = new ResultMetalModel
            {
                FromDate = new DateTime(2017, 1, 1),
                ToDate = new DateTime(2017, 2, 2)
            };
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _dataAccess = null;
            _metaModel = null;
            _balance = null;
        }

        [Test]
        public void GetOutRight_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Result_Outright")).Returns(objectDb);
            dynamic result = null;
            objectDb.ExecuteReader(Arg.Do<dynamic>(arg => result = arg));

            // Action
            _dataAccess.GetOutRight(_metaModel);

            // Assert
            Assert.That(
                new JavaScriptSerializer().Serialize(result),
                Is.EqualTo(@"{""fromdate"":""\/Date(1483203600000)\/"",""todate"":""\/Date(1485968400000)\/"",""isshowall"":true}"));
        }

        private static void MapReaderData(IDataReader reader, Dictionary<string, object> readerDataMapping)
        {
            reader[Arg.Any<string>()].Returns(x =>
            {
                var paramName = x.ArgAt<string>(0);
                if (readerDataMapping.ContainsKey(paramName))
                {
                    return readerDataMapping[paramName];
                }

                return string.Empty;
            });
        }
    }
}