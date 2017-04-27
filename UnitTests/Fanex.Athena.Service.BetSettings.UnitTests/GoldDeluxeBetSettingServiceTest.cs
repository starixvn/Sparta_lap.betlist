namespace Fanex.Athena.Service.BetSettings.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Web.Script.Serialization;
    using BetSettings;
    using BetSettings.Interfaces;
    using Core.Users.Interfaces;
    using Core.Utility.Interfaces;
    using DataAccess.BetSettings.Interfaces;
    using Extension.Utility;
    using Model.BetSettings.DTO;
    using Model.BetSettings.DTO.GoldDeluxe;
    using Model.BetSettings.ViewModel;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class GoldDeluxeBetSettingTest
    {
        private const int LicenseeId = 1;
        private const string SiteName = "spondemo";
        private const int CustId = 2;

        private IAuthorizationService _authorizationService;
        private IThirdPartyBetSettingDataAccess _dataAccess;
        private IAppSettings _appSettings;
        private IWebClientService _webClientService;
        private IGoldDeluxeBetSettingService _service;

        [OneTimeSetUp]
        public void SetUp()
        {
            _authorizationService = Substitute.For<IAuthorizationService>();
            _dataAccess = Substitute.For<IThirdPartyBetSettingDataAccess>();
            _appSettings = Substitute.For<IAppSettings>();
            _webClientService = Substitute.For<IWebClientService>();
            _service = new GoldDeluxeBetSettingService(_authorizationService, _dataAccess, _appSettings, _webClientService);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _authorizationService = null;
            _dataAccess = null;
            _appSettings = null;
            _webClientService = null;
            _service = null;
        }

        [Test]
        public void GetViewModel_WhenCall_ApiResultRetriviedCorrectly()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            _dataAccess.GetBetSetting(CustId, ProductTypes.GoldDeluxe).Returns(new ThirdPartyBetSetting()
            {
                IsLineEnabled = true,
                IsEnabled = true
            });

            _appSettings.GetValue("GoldDeluxeGetUserGroupApiUrl").Returns("http://api1.com");
            _appSettings.GetValue("GoldDeluxeGroupBettingLimitApiUrl").Returns("http://api2.com");
            _webClientService.UploadString(new Uri("http://api1.com"), "SiteName=spondemo&IBCCustID=2&ThirdPartyId=5&Currency=0")
                .Returns("<GetUserGroupBettingLimit><ReturnCode>0</ReturnCode><ReturnMessage>WTF</ReturnMessage></GetUserGroupBettingLimit>");
            _webClientService.UploadString(new Uri("http://api2.com"), "SiteName=spondemo&Currency=0&ThirdPartyId=5")
                .Returns("<Game GameType='1' Name='A'><Group GroupType='AA'><Min>1</Min><Max>100</Max></Group></Game>");

            // Action
            var result = _service.GetViewModel(LicenseeId, CustId, SiteName);

            // Assert
            Assert.That(result.BetSetting.Group, Is.EqualTo("WTF"));
            Assert.That(result.Games[0].GameType, Is.EqualTo("1"));
            Assert.That(result.Games[0].Groups[0].GroupType, Is.EqualTo("AA"));
        }

        [Test]
        public void GetViewModel_HaveNoPermission_ReturnNull()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(false);

            // Action
            var result = _service.GetViewModel(LicenseeId, CustId, SiteName);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetViewModel_IsLineEnabledValueIsFalse_ReturnCorrectViewModel()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            _dataAccess.GetBetSetting(CustId, ProductTypes.GoldDeluxe).Returns(new ThirdPartyBetSetting()
            {
                IsLineEnabled = false,
                IsEnabled = true,
                CurrencyId = 1
            });

            _appSettings.GetValue("GoldDeluxeGetUserGroupApiUrl").Returns("http://api1.com");
            _appSettings.GetValue("GoldDeluxeGroupBettingLimitApiUrl").Returns("http://api2.com");
            _webClientService.UploadString(new Uri("http://api1.com"), "SiteName=spondemo&IBCCustID=2&ThirdPartyId=5&Currency=1")
                .Returns("<GetUserGroupBettingLimit><ReturnCode>0</ReturnCode><ReturnMessage>WTF</ReturnMessage></GetUserGroupBettingLimit>");
            _webClientService.UploadString(new Uri("http://api2.com"), "SiteName=spondemo&Currency=1&ThirdPartyId=5")
                .Returns("<Game GameType='1' Name='A'><Group GroupType='AA'><Min>1</Min><Max>100</Max></Group></Game>");

            var serializer = new JavaScriptSerializer();
            var setting = new GoldDeluxeBetSetting
            {
                CurrencyId = 1,
                IsEnabled = true,
                IsLineEnabled = false,
                Group = "WTF"
            };
            var goldDeluxeGame = new List<GoldDeluxeGame>
            {
                new GoldDeluxeGame
                {
                    GameType = "1",
                    Name = "A",
                    Groups = new List<GoldDeluxeGroup>
                    {
                        new GoldDeluxeGroup
                        {
                            GroupType = "AA",
                            Min = 1,
                            Max = 100
                        }
                    }
                }
            };
            var expectedResult = serializer.Serialize(new GoldDeluxeBetSettingViewModel { BetSetting = setting, Games = goldDeluxeGame, IsEnabled = true });

            // Action
            var result = serializer.Serialize(_service.GetViewModel(LicenseeId, CustId, SiteName));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetViewModel_DefaultGroupValueIsAssignedForSetting_ReturnCorrectViewModel()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            _dataAccess.GetBetSetting(CustId, ProductTypes.GoldDeluxe).Returns(new ThirdPartyBetSetting()
            {
                IsLineEnabled = true,
                IsEnabled = true,
                CurrencyId = 1
            });
            _appSettings.GetValue("GoldDeluxeGetUserGroupApiUrl").Returns("http://api1.com");
            _appSettings.GetValue("GoldDeluxeGroupBettingLimitApiUrl").Returns("http://api2.com");
            _webClientService.UploadString(new Uri("http://api1.com"), "SiteName=spondemo&IBCCustID=2&ThirdPartyId=5&Currency=1")
                .Returns("<GetUserGroupBettingLimit><ReturnCode>2</ReturnCode><ReturnMessage>ABC</ReturnMessage></GetUserGroupBettingLimit>");
            _webClientService.UploadString(new Uri("http://api2.com"), "SiteName=spondemo&Currency=1&ThirdPartyId=5")
                .Returns("<Game GameType='1' Name='A'><Group GroupType='default'><Min>1</Min><Max>100</Max></Group></Game>");

            var serializer = new JavaScriptSerializer();
            var setting = new GoldDeluxeBetSetting
            {
                CurrencyId = 1,
                IsEnabled = true,
                IsLineEnabled = true,
                Group = "default",
                CheckedGroups = new string[1] { "default" }
            };
            var goldDeluxeGame = new List<GoldDeluxeGame>
            {
                new GoldDeluxeGame
                {
                    GameType = "1",
                    Name = "A",
                    Groups = new List<GoldDeluxeGroup>
                    {
                        new GoldDeluxeGroup
                        {
                            GroupType = "default",
                            Min = 1,
                            Max = 100
                        }
                    }
                }
            };
            var expectedResult = serializer.Serialize(new GoldDeluxeBetSettingViewModel { BetSetting = setting, Games = goldDeluxeGame, IsEnabled = true });

            // Action
            var result = serializer.Serialize(_service.GetViewModel(LicenseeId, CustId, SiteName));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetViewModel_IsLineEnabledValueIsTrueAndNotAnySatisfiedCondition_ReturnCorrectViewModel()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            _dataAccess.GetBetSetting(CustId, ProductTypes.GoldDeluxe).Returns(new ThirdPartyBetSetting()
            {
                IsLineEnabled = true,
                IsEnabled = true,
                CurrencyId = 1
            });
            _appSettings.GetValue("GoldDeluxeGetUserGroupApiUrl").Returns("http://api1.com");
            _appSettings.GetValue("GoldDeluxeGroupBettingLimitApiUrl").Returns("http://api2.com");
            _webClientService.UploadString(new Uri("http://api1.com"), "SiteName=spondemo&IBCCustID=2&ThirdPartyId=5&Currency=1")
                .Returns("<GetUserGroupBettingLimit><ReturnCode>0</ReturnCode><ReturnMessage>A</ReturnMessage></GetUserGroupBettingLimit>");
            _webClientService.UploadString(new Uri("http://api2.com"), "SiteName=spondemo&Currency=1&ThirdPartyId=5")
                .Returns("<Game GameType='1' Name='A'><Group GroupType='A'><Min>1</Min><Max>100</Max></Group></Game>");

            var serializer = new JavaScriptSerializer();
            var setting = new GoldDeluxeBetSetting
            {
                CurrencyId = 1,
                IsEnabled = true,
                IsLineEnabled = true,
                Group = "A",
                CheckedGroups = new string[1] { "A" }
            };
            var goldDeluxeGame = new List<GoldDeluxeGame>
            {
                new GoldDeluxeGame
                {
                    GameType = "1",
                    Name = "A",
                    Groups = new List<GoldDeluxeGroup>
                    {
                        new GoldDeluxeGroup
                        {
                            GroupType = "A",
                            Min = 1,
                            Max = 100
                        }
                    }
                }
            };
            var expectedResult = serializer.Serialize(new GoldDeluxeBetSettingViewModel { BetSetting = setting, Games = goldDeluxeGame, IsEnabled = true });

            // Action
            var result = serializer.Serialize(_service.GetViewModel(LicenseeId, CustId, SiteName));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void UpdateBetSetting_ApiReturnCodeIsZero_ReturnSuccessResult()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            _dataAccess.GetBetSetting(CustId, ProductTypes.GoldDeluxe).Returns(new ThirdPartyBetSetting()
            {
                IsLineEnabled = true,
                IsEnabled = true
            });

            var group = "A";
            _appSettings.GetValue("GoldDeluxeSetUserGroupApiUrl").Returns("http://api3.com");

            _webClientService.UploadString(new Uri("http://api3.com"), "SiteName=spondemo&IBCCustID=2&ThirdPartyId=5&GroupType=" + group)
                .Returns("<SetUserGroupBettingLimit><ReturnCode>0</ReturnCode></SetUserGroupBettingLimit>");

            // Action
            var result = _service.UpdateBetSetting(LicenseeId, SiteName, CustId, group);

            // Assert
            Assert.That(result.Code, Is.EqualTo(0));
        }

        [Test]
        public void UpdateBetSetting_IsEnabledInSettingIsFale_ReturnCorrectErrorResult()
        {
            // Arrange
            _dataAccess.GetBetSetting(CustId, ProductTypes.GoldDeluxe).Returns(new ThirdPartyBetSetting()
            {
                IsEnabled = false
            });
            var group = "A";

            // Action
            var result = _service.UpdateBetSetting(LicenseeId, SiteName, CustId, group);

            // Assert
            Assert.That(result.Code, Is.EqualTo(1));
            Assert.That(result.Message, Is.EqualTo("Invalid parameter. Invalid custId."));
        }

        [Test]
        public void UpdateBetSetting_HaveNoPermission_ReturnCorrectErrorResult()
        {
            // Arrange
            _dataAccess.GetBetSetting(CustId, ProductTypes.GoldDeluxe).Returns(new ThirdPartyBetSetting()
            {
                IsEnabled = true
            });
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(false);
            var group = "A";

            // Action
            var result = _service.UpdateBetSetting(LicenseeId, SiteName, CustId, group);

            // Assert
            Assert.That(result.Code, Is.EqualTo(1));
            Assert.That(result.Message, Is.EqualTo("Invalid parameter. Invalid custId."));
        }

        [Test]
        public void UpdateBetSetting_ApiReturnCodeIsNotZero_ReturnCorrectErrorResult()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            _dataAccess.GetBetSetting(CustId, ProductTypes.GoldDeluxe).Returns(new ThirdPartyBetSetting()
            {
                IsLineEnabled = true,
                IsEnabled = true
            });

            var group = "A";
            _appSettings.GetValue("GoldDeluxeSetUserGroupApiUrl").Returns("http://api3.com");

            _webClientService.UploadString(new Uri("http://api3.com"), "SiteName=spondemo&IBCCustID=2&ThirdPartyId=5&GroupType=" + group)
                .Returns("<SetUserGroupBettingLimit><ReturnCode>12</ReturnCode></SetUserGroupBettingLimit>");

            // Action
            var result = _service.UpdateBetSetting(LicenseeId, SiteName, CustId, group);

            // Assert
            Assert.That(result.Code, Is.EqualTo(1));
            Assert.That(result.Message, Is.EqualTo("Invalid parameter. Invalid custId."));
        }

        [Test]
        public void UpdateStatus_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            _dataAccess.GetBetSetting(CustId, ProductTypes.GoldDeluxe).Returns(new ThirdPartyBetSetting()
            {
                IsLineEnabled = true,
                IsEnabled = true
            });

            var expectedResult = new ResultInfo
            {
                Code = 999
            };
            var userId = 1;
            var language = "en-US";
            var ip = "ip";
            _dataAccess.UpdateStatus(ProductTypes.GoldDeluxe, userId, CustId, language, ip, true).Returns(expectedResult);

            // Action
            var result = _service.UpdateStatus(LicenseeId, userId, CustId, language, ip, true);

            // Assert
            Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
        }

        [Test]
        public void UpdateStatus_HaveNoPermission_ReturnCorrectErrorResult()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(false);

            var expectedResult = new ResultInfo
            {
                Code = 1,
                Message = "Invalid parameter. Invalid custId."
            };
            var userId = 1;
            var language = "en-US";
            var ip = "ip";

            // Action
            var result = _service.UpdateStatus(LicenseeId, userId, CustId, language, ip, true);

            // Assert
            Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
            Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
        }

        [Test]
        public void UpdateStatus_IsLineEnabledInSettingIsFalse_ReturnCorrectErrorResult()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            _dataAccess.GetBetSetting(CustId, ProductTypes.GoldDeluxe).Returns(new ThirdPartyBetSetting()
            {
                IsLineEnabled = false
            });

            var expectedResult = new ResultInfo
            {
                Code = 1,
                Message = "Invalid parameter. Invalid custId."
            };
            var userId = 1;
            var language = "en-US";
            var ip = "ip";

            // Action
            var result = _service.UpdateStatus(LicenseeId, userId, CustId, language, ip, true);

            // Assert
            Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
            Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
        }
    }
}