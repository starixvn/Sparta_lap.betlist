namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using Base.UnitTests;
    using Model.Core.DTO.Common;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Result.Core.Sportsbook.DTO;

    [TestFixture]
    public partial class ResultReportControllerTest : BaseControllerHelperTest
    {
        [Test]
        public void GetSports_InvalidModelState_ReturnEmptyResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                controller.ModelState.AddModelError("error", "hihi");

                // Action
                var result = controller.GetSports(new ResultMetalModel()) as JsonResult;

                // Assert
                Assert.That(result.Data, Is.EqualTo(string.Empty));
            }
        }

        [Test]
        public void GetSports_IsOutRight_ReturnOutrightResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const int SportId = 99;
                var dataList = new List<Outright>
                {
                    new Outright { SportType = SportId }
                };
                var metaModel = new ResultMetalModel
                {
                    SportId = SportId,
                    IsOutRight = true
                };
                _resultService.GetOutRight(metaModel).Returns(dataList);

                // Action
                var result = controller.GetSports(metaModel) as JsonResult;

                // Assert
                Assert.That(
                        new JavaScriptSerializer().Serialize(result.Data),
                        Is.EqualTo(@"{""Sports"":[{""SportId"":99,""SportName"":""""}],""Leagues"":[{""LeagueId"":0,""LeagueName"":"""",""IsOutRight"":false}]}"));
            }
        }

        [Test]
        public void GetSports_SportIdIsZero_ReturnNormalSportsResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const int SportId = 99;
                var dataList = new List<Sport>
                {
                    new Sport { SportId = SportId }
                };
                var metaModel = new ResultMetalModel
                {
                    SportId = 0,
                    FromDate = new DateTime(2017, 1, 1)
                };
                _resultService.GetSports(metaModel.FromDate, SessionService.LicenseeSession.LicenseeId).Returns(dataList);

                // Action
                var result = controller.GetSports(metaModel) as JsonResult;

                // Assert
                Assert.That(
                        new JavaScriptSerializer().Serialize(result.Data),
                        Is.EqualTo(@"{""Sports"":[{""SportId"":99,""SportName"":""""}],""Leagues"":[]}"));
            }
        }

        [Test]
        public void GetSports_SportIdIsNotZero_ReturnCorrectResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const int SportId = 99;
                var dataList = new List<League>
                {
                    new League()
                };
                var metaModel = new ResultMetalModel
                {
                    SportId = SportId,
                    FromDate = new DateTime(2017, 1, 1)
                };
                _resultService.GetLeagues(metaModel.FromDate, SportId).Returns(dataList);

                // Action
                var result = controller.GetSports(metaModel) as JsonResult;

                // Assert
                Assert.That(
                        new JavaScriptSerializer().Serialize(result.Data),
                        Is.EqualTo(@"{""Leagues"":[{""LeagueId"":0,""LeagueName"":"""",""IsOutRight"":false}]}"));
            }
        }
    }
}