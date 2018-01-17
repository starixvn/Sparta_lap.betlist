namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using Base.UnitTests;
    using Model.Casino.DTO;
    using Model.Casino.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;
    using UnitTest.Common.NUnitExtensions;

    [TestFixture]
    public partial class HandHistoryControllerTest : BaseControllerHelperTest
    {
        [Test]
        public void RNGCasinoData_MetaModelIsNull_ReturnEmptyResult()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = controller.RNGCasinoData(null) as JsonResult;

                // Assert
                Assert.That(new JavaScriptSerializer().Serialize(result.Data), Is.EqualTo(@"{""Param"":"""",""Data"":"""",""Total"":""""}"));
            }
        }

        [Test]
        public void RNGCasinoData_InvalidMetaModel_ReturnEmptyData()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new RNGCasinoResultMetaModel();

                // Action
                var result = controller.RNGCasinoData(metaModel) as JsonResult;
                var resultDictionary = ObjectHelper.ToDictionary(result.Data);

                // Assert
                Assert.Multiple(() =>
                {
                    _rngCasinoService.DidNotReceiveWithAnyArgs().GetRNGCasinoResult(metaModel);
                    Assert.That(resultDictionary["Data"], Is.Empty);
                    Assert.That(resultDictionary["Param"], Is.EqualTo(metaModel));
                });
            }
        }

        [Test]
        public void RNGCasinoData_ValidMetaModel_ReturnCorrectData()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new RNGCasinoResultMetaModel
                {
                    RefNo = 1,
                    CustId = 2,
                    FromDate = new DateTime(2018, 1, 1),
                    ToDate = new DateTime(2018, 1, 2),
                    SystemId = "abc"
                };
                var sampleItem = new RNGCasinoResult
                {
                    Stake = 1,
                    Bet = 2,
                    WinLoss = 3,
                    RollingComm = 4
                };
                var dataList = new List<RNGCasinoResult> { sampleItem, sampleItem };
                _rngCasinoService.GetRNGCasinoResult(metaModel).Returns(dataList);
                var expectedTotalData = new RNGCasinoResult
                {
                    Stake = 2,
                    Bet = 4,
                    WinLoss = 6,
                    RollingComm = 8
                };

                // Action
                var result = controller.RNGCasinoData(metaModel) as JsonResult;
                var resultDictionary = ObjectHelper.ToDictionary(result.Data);

                // Assert
                Assert.Multiple(() =>
                {
                    _rngCasinoService.Received(1).GetRNGCasinoResult(metaModel);
                    Assert.That(resultDictionary["Data"], Is.EqualTo(dataList));
                    Assert.That(resultDictionary["Param"], Is.EqualTo(metaModel));
                    Assert.That(resultDictionary["Total"], Tobe.SameProperties(expectedTotalData));
                });
            }
        }
    }
}