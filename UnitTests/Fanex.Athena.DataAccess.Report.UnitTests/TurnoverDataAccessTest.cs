namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System.Collections.Generic;
    using System.Data;
    using Core.UnitTests.TestHelpers;
    using Model.Core.MetaModel.Product;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class TurnoverDataAccessTest : BaseDataAccessHelperTest<TurnoverDataAccess>
    {
        private const string SpName = "Lrf_Rpt_TurnOver";

        [Test]
        public void GetDataList_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var metaModel = new TurnoverMetaModel
            {
                Username = "abc",
                AgentListXml = "xml",
                Year = 2017,
                Month = 3,
                FromDay = 1,
                ToDay = 2,
                ProductOptions = new List<ProductOption>
                {
                    new ProductOption { Code = "SB", Status = true },
                    new ProductOption { Code = "NG", Status = false }
                }
            };
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);

            // Action
            DataAccess.GetDataList(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("username", metaModel.Username);
                ObjectDb.Received(1).SetParameterValue("AgentList", metaModel.AgentListXml);
                ObjectDb.Received(1).SetParameterValue("fromdate", metaModel.FromDate);
                ObjectDb.Received(1).SetParameterValue("todate", metaModel.ToDate);
                ObjectDb.Received(1).SetParameterValue("showSB", true);
                ObjectDb.Received(1).SetParameterValue("showNG", false);
            });
        }

        [Test]
        public void GetDataList_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);
            var readerDataMapping = new
            {
                OpeningBalanceUSD = 999
            };

            MapReaderData(readerDataMapping);

            // Action
            var result = DataAccess.GetDataList(new TurnoverMetaModel
            {
                BaseCurrency = "USD"
            });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(1));
                Assert.That(result[0].OpeningBalance, Is.EqualTo(999));
            });
        }
    }
}