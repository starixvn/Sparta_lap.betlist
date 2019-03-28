namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using System;
    using System.Data;
    using System.Threading.Tasks;
    using Core.Common.Constants;
    using Core.Common.Interfaces;
    using Core.UnitTests.TestHelpers;
    using Model.BetSettings.MetaModel;
    using Model.Core.MetaModel.Common;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CoreCasinoBetSettingDataAccessTest : BaseDataAccessHelperTest<CoreCasinoBetSettingDataAccess>
    {
        private IUserLogDataAccess _userLogDataAccess;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            _userLogDataAccess = Substitute.For<IUserLogDataAccess>();
            base.FixtureSetUp();
        }

        protected override void SetUpDataAccess()
        {
            DataAccess = Substitute.ForPartsOf<CoreCasinoBetSettingDataAccess>(
                _userLogDataAccess);
        }

        [Test]
        public async Task UpdateCasinoStatusAsync_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerUpdCasinoStatus")).Returns(ObjectDb);
            var metaModel = new CoreCasinoStatusMetaModel
            {
                UserId = 1,
                CustId = 2,
                Status = 0,
                Language = "LanguageTest",
                Ip = "IP"
            };
            var sportId = 3;

            // Action
            await DataAccess.UpdateCasinoStatusAsync(metaModel, sportId);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("userid", metaModel.UserId);
                ObjectDb.Received(1).SetParameterValue("custid", metaModel.CustId);
                ObjectDb.Received(1).SetParameterValue("sportid", sportId);
                ObjectDb.Received(1).SetParameterValue("language", metaModel.Language);
                ObjectDb.Received(1).SetParameterValue("ip", metaModel.Ip);
                ObjectDb.Received(1).SetParameterValue("status", metaModel.Status);
                ObjectDb.Received(1).SetParameterValue("ErrCode", null);
                ObjectDb.Received(1).SetParameterValue("ErrMsg", null);
            });
        }

        [TestCase(1, 0)]
        [TestCase(2, 1)]
        public async Task WriteLogUpdatingAction_WhenCall_ParametersPassedCorrectly(int status, int expectedTrackingStatus1)
        {
            // Arrange
            const string TrackingValueTemplate = "<Lrf_CustomerUpdCasinoStatus UserID=\"1\" CustID=\"2\" SportID=\"3\" "
                + "Status=\"{0}\" Ip=\"IP\" Lang=\"abc\" RoleID=\"1\" ErrCode=\"0\" ErrMsg=\"\"/>";
            const string SPName = "Lrf_CustomerUpdCasinoStatus";
            DataAccess.GetObjectDb(Arg.Is(SPName)).Returns(ObjectDb);
            var metaModel = new CoreCasinoStatusMetaModel
            {
                UserId = 1,
                CustId = 2,
                Status = status,
                Language = "abc",
                Ip = "IP"
            };
            const int SportId = 3;

            // Action
            await DataAccess.UpdateCasinoStatusAsync(metaModel, SportId);

            Func<UserLogMetaModel, bool> matchUserInfo = x =>
                x.ActionID == UserLogAction.LiveCasinoBetSetting &&
                x.CustID == metaModel.CustId &&
                x.UserId == metaModel.UserId;

            Func<UserLogMetaModel, bool> matchTrackingValues = x =>
                x.SPName == SPName &&
                x.TrackingValue1 == string.Format(TrackingValueTemplate, expectedTrackingStatus1) &&
                x.TrackingValue2 == string.Format(TrackingValueTemplate, status);

            // Assert
            _userLogDataAccess.Received(1).WriteLog(Arg.Is<UserLogMetaModel>(x =>
                matchUserInfo(x) &&
                matchTrackingValues(x)));
        }
    }
}