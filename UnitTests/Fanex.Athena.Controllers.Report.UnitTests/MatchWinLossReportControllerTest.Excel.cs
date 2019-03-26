namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System.Data;
    using Base.UnitTests;
    using Constant.DataDomain;
    using Excel.Base.Entities;
    using Model.Core.DataDomain;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Web.ResultExtension;

    [TestFixture]
    public partial class MatchWinLossReportControllerTest : CultureBaseControllerHelperTest
    {
        [TestCase("BA,NG,SB,VS,KN", true)]
        [TestCase("hihi", false)]
        public void Excel_WhenCall_PassParamsToCreateExcelCorrectly(string productCodeList, bool expectedProductVisible)
        {
            using (var controller = CreateController(productCodeList))
            {
                // Arrange
                MockCreatingExcel(controller);
                var metaModel = new MatchWinLossMetaModel
                {
                    BaseCurrency = "UUS",
                    CurrencyName = "All",
                    CurrencyId = 0,
                    IsExportExcel = true
                };
                var expectedData = new DataSet();
                expectedData.Tables.Add(new DataTable("MainData"));
                expectedData.Tables.Add(new DataTable("TotalData"));

                _matchWinLossService.GetDetailDataSet(metaModel).Returns(expectedData);

                // Action
                var result = controller.Index(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    controller.Received(1).CreateExcel(
                        Arg.Any<string>(),
                        metaModel,
                        Arg.Is<ShowingOption>(x => x.BAVisible == expectedProductVisible &&
                                            x.NGVisible == expectedProductVisible &&
                                            x.SBVisible == expectedProductVisible &&
                                            x.VSVisible == expectedProductVisible &&
                                            x.KNVisible == expectedProductVisible),
                        Arg.Is<ConvertingOption>(x =>
                            x.IsConvert == metaModel.IsConvert &&
                            x.BaseCurrency == metaModel.BaseCurrency &&
                            x.CurrencyName == metaModel.CurrencyName),
                        Arg.Is<DataTable>(x => x.TableName == "MainData"),
                        Arg.Is<DataTable>(x => x.TableName == "TotalData"));
                    Assert.That(result, Is.InstanceOf<ExcelResult>());
                });
            }
        }

        [TestCase(BetType.Ng1StBallOe, "Number Game No abc", "Number Game")]
        public void Excel_WhenCall_AddColumnsForSortingCorrectly(
            int bettype,
            string expectedMatchName,
            string expectedLeagueName)
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string MatchCode = "abc";
                const string Race = "mmm";
                MockCreatingExcel(controller);
                var metaModel = new MatchWinLossMetaModel
                {
                    BaseCurrency = "UUS",
                    CurrencyName = "All",
                    CurrencyId = 0,
                    IsExportExcel = true
                };
                var expectedData = new DataSet();
                var mainTable = new DataTable("MainData");
                mainTable.Columns.Add("bettype", typeof(int));
                mainTable.Columns.Add("matchcode", typeof(string));
                mainTable.Columns.Add("race", typeof(string));
                mainTable.Rows.Add(bettype, MatchCode, Race);
                expectedData.Tables.Add(mainTable);
                expectedData.Tables.Add(new DataTable("TotalData"));

                _matchWinLossService.GetDetailDataSet(metaModel).Returns(expectedData);

                // Action
                var result = controller.Index(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(mainTable.Rows[0]["newproductid"], Is.EqualTo(ProductTypes.NumberGame));
                    Assert.That(mainTable.Rows[0]["matchname"], Is.EqualTo(expectedMatchName));
                    Assert.That(mainTable.Rows[0]["leaguename"], Is.EqualTo(expectedLeagueName));
                });
            }
        }
    }
}