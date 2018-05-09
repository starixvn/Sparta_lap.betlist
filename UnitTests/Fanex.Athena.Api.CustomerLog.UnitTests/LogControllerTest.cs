namespace Fanex.Athena.Api.CustomerLog.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Session.Enum;
    using Base.UnitTests;
    using DataAccess.Customer.Interfaces;
    using Extension.Utility;
    using Model.Core.DataDomain;
    using Model.Customer.DTO.CustomerLog;
    using Model.Customer.MetaModel.CustomerLog;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Helpers;

    [TestFixture]
    public class LogControllerTest : BaseApiControllerHelperTest
    {
        private ICustomerLogDataAccess _customerLogDataAccess;

        private ILogInLogDataAccess _logInLogDataAccess;

        private IAppSettings _appSettings;

        private IClientViewHelper _clientViewHelper;

        private LogController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();
            MockSessionData();

            _customerLogDataAccess = Substitute.For<ICustomerLogDataAccess>();
            _appSettings = Substitute.For<IAppSettings>();
            _logInLogDataAccess = Substitute.For<ILogInLogDataAccess>();
            _clientViewHelper = Substitute.For<IClientViewHelper>();

            _controller = Substitute.ForPartsOf<LogController>(
                SessionService,
                ProductService,
                LangService,
                _customerLogDataAccess,
                _logInLogDataAccess,
                _appSettings,
                _clientViewHelper);
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _controller = null;
        }

        [TestCase(null)]
        [TestCase("")]
        public void LoginLog_UserNameIsInvalid_ReturnEmptyListOfString(string userName)
        {
            // Action
            var result = _controller.LoginLog(userName);

            // Assert
            Assert.That(result.GetType().GetProperty("rows").GetValue(result, null), Is.Empty);
        }

        [Test]
        public void LoginLog_ValidUserName_ReturnCorrectResult()
        {
            // Arrange
            const string UserName = "test";

            var dataList = new List<LogInLog>();
            _logInLogDataAccess.GetDataList(SessionLicenseeId, UserName, Arg.Any<string>()).Returns(dataList);

            // Action
            var result = _controller.LoginLog(UserName);

            // Assert
            Assert.That(result.GetType().GetProperty("rows").GetValue(result, null), Is.EqualTo(dataList));
        }

        [Test]
        public void GroupCommission_WhenCall_ReturnEmptyResult()
        {
            // Arrrange
            _clientViewHelper.GetViewPath(Arg.Any<string>(), Arg.Any<ClientViewType>()).Returns(new Model.Core.DTO.Common.ViewByOption { IsNavigationView = true });
            _appSettings.ClientViewType.Returns("0");

            var dataList = new List<CustomerLog>
            {
                new CustomerLog { LogId = 2, ActionId = 1, TrackingValue = "<doc <GroupName=\"Group\"/> />", TrackingValueOld = "<doc <GroupName=\"OldGroup\"/> />" },
                new CustomerLog { LogId = 4, ActionId = 2, TrackingValue = "<doc <GroupColor=\"Red\"/> />", TrackingValueOld = "<doc <GroupColor=\"Green\"/> />" },
                new CustomerLog { LogId = 3, ActionId = 2401, TrackingValue = string.Empty, TrackingValueOld = string.Empty },
                new CustomerLog { LogId = 1, ActionId = 2403, TrackingValue = string.Empty, TrackingValueOld = string.Empty }
            };

            _customerLogDataAccess.GetGroupCommList(Arg.Any<CustomerLogMetaModel>()).Returns(dataList);

            // Action
            var result = _controller.GroupCommission(2018, 1, 1, 1);
            var rows = (List<CustomerLog>)result.GetType().GetProperty("rows").GetValue(result, null);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(rows.Select(x => x.LogId), Is.EqualTo(new[] { 2, 4, 3, 1 }));
                Assert.That(rows[0].TrackingValue, Is.EqualTo("<doc GroupName=\"Group\" />"));
            });
        }

        [TestCase(null)]
        [TestCase("")]
        public void CustomerSetting_InvalidModel_ReturnEmptyResult(string userName)
        {
            // Action
            var result = _controller.CustomerSetting(userName, 0, 2018, 1, 1, 1);

            // Assert
            Assert.That(result.GetType().GetProperty("rows").GetValue(result, null), Is.Empty);
        }

        [Test]
        public void CustomerSetting_ValidModel_ReturnEmptyResult()
        {
            // Arrrange
            const string UserName = "Test";

            _clientViewHelper.GetViewPath(Arg.Any<string>(), Arg.Any<ClientViewType>()).Returns(new Model.Core.DTO.Common.ViewByOption { IsNavigationView = true });
            _appSettings.ClientViewType.Returns("0");

            var dataList = new List<CustomerLog>
            {
                new CustomerLog { LogId = 2, ActionId = 1, TrackingValue = string.Empty, TrackingValueOld = string.Empty },
                new CustomerLog { LogId = 4, ActionId = 2, TrackingValue = "<doc <GroupColor=\"Red\"/> />", TrackingValueOld = "<doc <GroupColor=\"Green\"/> />" },
                new CustomerLog { LogId = 3, ActionId = 2401, TrackingValue = string.Empty, TrackingValueOld = string.Empty },
                new CustomerLog { LogId = 1, ActionId = 2403, TrackingValue = string.Empty, TrackingValueOld = string.Empty }
            };

            _customerLogDataAccess.GetCustomerSettingList(Arg.Any<CustomerLogMetaModel>()).Returns(dataList);

            // Action
            var result = _controller.CustomerSetting(UserName, 0, 2018, 1, 1, 1);
            var rows = (List<CustomerLog>)result.GetType().GetProperty("rows").GetValue(result, null);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(rows.Select(x => x.LogId), Is.EqualTo(new[] { 4, 3, 2, 1 }));
                Assert.That(rows[0].TrackingValue, Is.EqualTo("<doc GroupColor=\"Red\" />"));
            });
        }
    }
}