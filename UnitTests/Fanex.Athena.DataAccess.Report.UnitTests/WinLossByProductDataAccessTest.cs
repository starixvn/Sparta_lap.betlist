namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core.Configuration.Interfaces;
    using Core.UnitTests.TestHelpers;
    using Data;
    using Extension.Utility;
    using Model.Core.DTO.Configuration;
    using Model.Core.MetaModel.Product;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class WinLossByProductDataAccessTest : BaseDataAccessHelperTest<WinLossByProductDataAccess>
    {
        private const string GetWinlossByProductsSp = "Lrf_Rpt_WinlossByProduct";
        private const string GetWinlossByProductAgentsSp = "Lrf_Rpt_WinlossByProductDetail_Ag";
        private const string GetWinlossByProductCustomersSp = "Lrf_Rpt_WinlossByProductDetail";

        private readonly BalanceUp _balanceUp = new BalanceUp
        {
            BalanceUpDay = new DateTime(2018, 1, 1),
            MoveBetsDay = new DateTime(2018, 2, 2),
        };

        private IAppSettings _appSetting;

        private WinLossByProductMetaModel _metaModel;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            balanceUpDataAccess.GetBalanceUp().Returns(_balanceUp);

            _appSetting = Substitute.For<IAppSettings>();
            _appSetting.HistoryDate.Returns(new DateTime(2016, 8, 31));
            DataAccess = Substitute.ForPartsOf<WinLossByProductDataAccess>(balanceUpDataAccess);

            _metaModel = new WinLossByProductMetaModel(_appSetting)
            {
                ReportBy = 1,
                Username = "abc",
                AgentListXml = "<xml>",
                FromDate = new DateTime(2017, 1, 1),
                ToDate = new DateTime(2017, 2, 2),
                DrillDownFromDate = new DateTime(2018, 1, 1),
                DrillDownToDate = new DateTime(2018, 2, 2),
                DrawFilter = 2,
                MemberId = 3,
                CustomerId = 4,
                ProductOptions = new List<ProductOption>
                {
                    new ProductOption { Code = "SB", Status = true }
                }
            };

            ObjectDb = Substitute.For<IObjectDb>();
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            _appSetting = null;

            base.FixtureTearDown();
        }

        [Test]
        public void GetWinlossByProducts_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetWinlossByProductsSp)).Returns(ObjectDb);

            // Action
            DataAccess.GetWinlossByProducts(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("reporttype", DbType.Byte, _metaModel.ReportBy);
                ObjectDb.Received().AddInParameter("username", DbType.String, _metaModel.Username);
                AssertCommonSpParameters(isDrilldown: false);
            });
        }

        [Test]
        public void GetWinlossByProducts_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetWinlossByProductsSp)).Returns(ObjectDb);

            ObjectDb.GetParameterValue("CustID").Returns(1000);
            ObjectDb.GetParameterValue("SystemName").Returns("ABC");
            Reader.Read().Returns(true, false);

            var readerDataMapping = new
            {
                TurnoverUSD = 999,
                reporttype = 20190101
            };
            MapReaderData(readerDataMapping);

            // Action
            var result = DataAccess.GetWinlossByProducts(
                new WinLossByProductMetaModel(_appSetting)
                {
                    BaseCurrency = "USD"
                });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList.Count(), Is.EqualTo(1));
                Assert.That(result.DataList.ElementAt(0).TurnOver, Is.EqualTo(999));
                Assert.That(result.DataList.ElementAt(0).Date, Is.EqualTo(new DateTime(2019, 1, 1)));

                Assert.That(result.ResultInfo.ExtendedData, Is.EqualTo("1000;ABC"));
            });
        }

        [Test]
        public void GetWinlossByProducts_WhenReceiveObjectDbException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                GetWinlossByProductsSp,
                () => DataAccess.GetWinlossByProducts(_metaModel));
        }

        [Test]
        public void GetWinlossByProductAgents_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetWinlossByProductAgentsSp)).Returns(ObjectDb);

            // Action
            DataAccess.GetWinlossByProductAgents(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("CustID", DbType.Int32, _metaModel.MemberId);
                AssertCommonSpParameters(isDrilldown: true);
            });
        }

        [Test]
        public void GetWinlossByProductAgents_WhenReceiveObjectDbException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                GetWinlossByProductAgentsSp,
                () => DataAccess.GetWinlossByProductAgents(_metaModel));
        }

        [Test]
        public void GetWinlossByProductCustomers_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetWinlossByProductCustomersSp)).Returns(ObjectDb);

            // Action
            DataAccess.GetWinlossByProductCustomers(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("custid", DbType.Int32, _metaModel.CustomerId);
                ObjectDb.Received().AddInParameter("MemberID", DbType.Int32, _metaModel.MemberId);
                AssertCommonSpParameters(isDrilldown: true);
            });
        }

        [Test]
        public void GetWinlossByProductCustomers_WhenReceiveObjectDbException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                GetWinlossByProductCustomersSp,
                () => DataAccess.GetWinlossByProductCustomers(_metaModel));
        }

        [Test]
        public void GetData_HasSystemIdColumn_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetWinlossByProductCustomersSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);
            Reader.FieldCount.Returns(1);
            Reader.GetName(0).Returns("username");
            ObjectDb.GetParameterValue("CustID").Returns(1000);
            ObjectDb.GetParameterValue("SystemName").Returns("ABC");

            var readerDataMapping = new
            {
                username = "Test"
            };
            MapReaderData(readerDataMapping);

            // Action
            var result = DataAccess.GetWinlossByProductCustomers(
                new WinLossByProductMetaModel(_appSetting)
                {
                    BaseCurrency = string.Empty
                });

            // Assert
            Assert.That(result.DataList.ElementAt(0).SystemId, Is.EqualTo(readerDataMapping.username));
        }

        [Test]
        public void GetData_HasExternalIdColumn_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetWinlossByProductCustomersSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);
            Reader.FieldCount.Returns(1);
            Reader.GetName(0).Returns("nickname");
            ObjectDb.GetParameterValue("CustID").Returns(1000);
            ObjectDb.GetParameterValue("SystemName").Returns("ABC");

            var readerDataMapping = new
            {
                nickname = "Test"
            };
            MapReaderData(readerDataMapping);

            // Action
            var result = DataAccess.GetWinlossByProductCustomers(
                new WinLossByProductMetaModel(_appSetting)
                {
                    BaseCurrency = string.Empty
                });

            // Assert
            Assert.That(result.DataList.ElementAt(0).ExternalId, Is.EqualTo(readerDataMapping.nickname));
        }

        [Test]
        public void GetData_HasCustomerIdColumn_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetWinlossByProductCustomersSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);
            Reader.FieldCount.Returns(1);
            Reader.GetName(0).Returns("custid");
            ObjectDb.GetParameterValue("CustID").Returns(1000);
            ObjectDb.GetParameterValue("SystemName").Returns("ABC");

            var readerDataMapping = new
            {
                custid = 321
            };
            MapReaderData(readerDataMapping);

            // Action
            var result = DataAccess.GetWinlossByProductCustomers(
                new WinLossByProductMetaModel(_appSetting)
                {
                    BaseCurrency = string.Empty
                });

            // Assert
            Assert.That(result.DataList.ElementAt(0).CustomerId, Is.EqualTo(readerDataMapping.custid));
        }

        private void AssertCommonSpParameters(bool isDrilldown)
        {
            ObjectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            ObjectDb.Received().AddInParameter("fromdate", DbType.Date, isDrilldown ? _metaModel.DrillDownFromDate.Date : _metaModel.FromDate.Date);
            ObjectDb.Received().AddInParameter("todate", DbType.Date, isDrilldown ? _metaModel.DrillDownToDate.Date : _metaModel.ToDate.Date);
            ObjectDb.Received().AddInParameter("isold", DbType.Boolean, _metaModel.IsOld);
            ObjectDb.Received().AddInParameter("drawfilter", DbType.Int32, _metaModel.DrawFilter);
            ObjectDb.Received().AddInParameter("movebetsday", DbType.DateTime, _balanceUp.MoveBetsDay);
            ObjectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
        }
    }
}