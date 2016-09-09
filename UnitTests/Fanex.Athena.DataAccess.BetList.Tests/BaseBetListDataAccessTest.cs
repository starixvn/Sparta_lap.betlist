namespace Fanex.Athena.DataAccess.Core.Tests.Common
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using BetList.Tests.ClassTest;
    using Model.BetList.DTO;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class BaseBetListDataAccessTest
    {
        private FakeBaseBetListDataAccess _dataAccess;
        private IDataReader _reader;

        [SetUp]
        public void SetUp()
        {
            _dataAccess = Substitute.ForPartsOf<FakeBaseBetListDataAccess>();
            _reader = Substitute.For<IDataReader>();
        }

        [TestCase(10.2, "<span class='bl-underdog'>10.20</span>")]
        [TestCase(1000.2, "<span class='bl-underdog'>1,000.20</span>")]
        [TestCase(0, "<span class='bl-underdog'>0.00</span>")]
        [TestCase(-10.2, "<span class='bl-favorite'>-10.20</span>")]
        [TestCase(-1000.2, "<span class='bl-favorite'>-1,000.20</span>")]
        public void NumberReport_WhenCall_DataReturnedCorrectly(object value, string expected)
        {
            // Action
            var result = _dataAccess.NumberReport(value);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        [Test]
        public void ReadLrfTicket_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            var currency = "UUS";
            var expected = GetFakeTicket();
            var dataList = GetFakeBetListData(currency, expected);
            _reader.Read().Returns(true, false);
            ConfigureReader(dataList);

            // Action
            var result = _dataAccess.ReadLrfTicket(_reader, currency)[0];

            // Assert
            Assert.That(result.UserName, Is.EqualTo(expected.UserName));
            Assert.That(result.RefNo, Is.EqualTo(expected.RefNo));
            Assert.That(result.MatchCode, Is.EqualTo(expected.MatchCode));
            Assert.That(result.Status, Is.EqualTo(expected.Status));
            Assert.That(result.BetTeam, Is.EqualTo(expected.BetTeam));
            Assert.That(result.EventStatus, Is.EqualTo(expected.EventStatus));
            Assert.That(result.ShowTime, Is.EqualTo(expected.ShowTime));
            Assert.That(result.OddsType, Is.EqualTo(expected.OddsType));
            Assert.That(result.ExternalId, Is.EqualTo(expected.ExternalId));
            Assert.That(result.Race, Is.EqualTo(expected.Race));
            Assert.That(result.BetFrom, Is.EqualTo(expected.BetFrom));
            Assert.That(result.BetCheck, Is.EqualTo(expected.BetCheck));

            Assert.That(result.IsLive, Is.EqualTo(expected.IsLive));
            Assert.That(result.IsNeutral, Is.EqualTo(expected.IsNeutral));

            Assert.That(result.SportTypeId, Is.EqualTo(expected.SportTypeId));
            Assert.That(result.BetTypeId, Is.EqualTo(expected.BetTypeId));
            Assert.That(result.LeagueId, Is.EqualTo(expected.LeagueId));
            Assert.That(result.HomeId, Is.EqualTo(expected.HomeId));
            Assert.That(result.AwayId, Is.EqualTo(expected.AwayId));
            Assert.That(result.MatchId, Is.EqualTo(expected.MatchId));
            Assert.That(result.CustId, Is.EqualTo(expected.CustId));
            Assert.That(result.CurrencyId, Is.EqualTo(expected.CurrencyId));
            Assert.That(result.LiveHomeScore, Is.EqualTo(expected.LiveHomeScore));
            Assert.That(result.LiveAwayScore, Is.EqualTo(expected.LiveAwayScore));
            Assert.That(result.SpecialLevel, Is.EqualTo(expected.SpecialLevel));
            Assert.That(result.StatusId, Is.EqualTo(expected.StatusId));

            Assert.That(result.TransId, Is.EqualTo(expected.TransId));
            Assert.That(result.BetId, Is.EqualTo(expected.BetId));

            Assert.That(result.WinlostDate, Is.EqualTo(expected.WinlostDate));
            Assert.That(result.TransDate, Is.EqualTo(expected.TransDate));

            Assert.That(result.PlayerComm, Is.EqualTo(expected.PlayerComm));
            Assert.That(result.Comm, Is.EqualTo(expected.Comm));
            Assert.That(result.LicenseeWL, Is.EqualTo(expected.LicenseeWL));
            Assert.That(result.Winlost, Is.EqualTo(expected.Winlost));
            Assert.That(result.BetEffective, Is.EqualTo(expected.BetEffective));
            Assert.That(result.Stake, Is.EqualTo(expected.Stake));
            Assert.That(result.ActualStake, Is.EqualTo(expected.ActualStake));
            Assert.That(result.Odds, Is.EqualTo(expected.Odds));
            Assert.That(result.MasterOdds, Is.EqualTo(expected.MasterOdds));
            Assert.That(result.Handicap1, Is.EqualTo(expected.Handicap1));
            Assert.That(result.Handicap2, Is.EqualTo(expected.Handicap2));

            Assert.That(result.TransDesc, Is.EqualTo(expected.TransDesc));
            Assert.That(result.Currency, Is.EqualTo(expected.Currency));
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private IDictionary<string, object> GetFakeBetListData(string baseCurrency, LrfTicket ticket)
        {
            var playerCommCol = "playercomm" + baseCurrency;
            var commCol = "comm" + baseCurrency;
            var licenseeWlCol = "licenseewl" + baseCurrency;
            var winlostCol = "winlost" + baseCurrency;
            var beteffectiveCol = "beteffective" + baseCurrency;
            var stakeCol = "stake" + baseCurrency;
            var actualStakeCol = "actual_stake" + baseCurrency;

            var dataList = new Dictionary<string, object>();
            dataList.Add("username", ticket.UserName);
            dataList.Add("refno", ticket.RefNo);
            dataList.Add("matchcode", ticket.MatchCode);
            dataList.Add("status", ticket.Status);
            dataList.Add("betteam", ticket.BetTeam);
            dataList.Add("betstatus", ticket.EventStatus);
            dataList.Add("showtime", ticket.ShowTime);
            dataList.Add("oddstype", ticket.OddsType);
            dataList.Add("nickname", ticket.ExternalId);
            dataList.Add("race", ticket.Race);
            dataList.Add("betfrom", ticket.BetFrom);
            dataList.Add("betcheck", ticket.BetCheck);

            dataList.Add("liveindicator", ticket.IsLive);
            dataList.Add("isneutral", ticket.IsNeutral);

            dataList.Add("sporttype", ticket.SportTypeId);
            dataList.Add("bettype", ticket.BetTypeId);
            dataList.Add("leagueid", ticket.LeagueId);
            dataList.Add("awayid", ticket.AwayId);
            dataList.Add("homeid", ticket.HomeId);
            dataList.Add("matchid", ticket.MatchId);
            dataList.Add("custid", ticket.CustId);
            dataList.Add("currencyid", ticket.CurrencyId);
            dataList.Add("livehomescore", ticket.LiveHomeScore);
            dataList.Add("liveawayscore", ticket.LiveAwayScore);
            dataList.Add("SpecialLevel", ticket.SpecialLevel);
            dataList.Add("StatusId", ticket.StatusId);

            dataList.Add("transid", ticket.TransId);
            dataList.Add("betid", ticket.BetId);

            dataList.Add("winlostdate", ticket.WinlostDate);
            dataList.Add("transdate", ticket.TransDate);

            dataList.Add(playerCommCol, ticket.PlayerComm);
            dataList.Add(commCol, ticket.Comm);
            dataList.Add(licenseeWlCol, ticket.LicenseeWL);
            dataList.Add(winlostCol, ticket.Winlost);
            dataList.Add(beteffectiveCol, ticket.BetEffective);
            dataList.Add(stakeCol, ticket.Stake);
            dataList.Add(actualStakeCol, ticket.ActualStake);
            dataList.Add("odds", ticket.Odds);
            dataList.Add("modds", ticket.MasterOdds);
            dataList.Add("hdp1", ticket.Handicap1);
            dataList.Add("hdp2", ticket.Handicap2);
            dataList.Add("transdesc", ticket.TransDesc);
            dataList.Add("currency", ticket.Currency);

            return dataList;
        }

        private void ConfigureReader(IDictionary<string, object> dataList)
        {
            var index = 0;

            _reader.FieldCount.Returns(dataList.Count);

            foreach (var item in dataList)
            {
                _reader.GetName(index).Returns(item.Key);
                _reader[item.Key].Returns(item.Value);
                index++;
            }
        }

        private LrfTicket GetFakeTicket()
        {
            return new LrfTicket
            {
                UserName = "UserNameTest",
                RefNo = "RefNoTest",
                MatchCode = "MatchCodeTest",
                Status = "StatusTest",
                BetTeam = "BetTeamTest",
                EventStatus = "EventStatusTest",
                ShowTime = "ShowTimeTest",
                OddsType = "OddsTypeTest",
                ExternalId = "ExternalIdTest",
                Race = "RaceTest",
                BetFrom = "BetFromTest",
                BetCheck = "BetCheckTest",
                IsLive = true,
                IsNeutral = true,
                SportTypeId = 1,
                BetTypeId = 2,
                LeagueId = 3,
                HomeId = 4,
                AwayId = 5,
                MatchId = 6,
                CustId = 7,
                CurrencyId = 8,
                LiveHomeScore = 9,
                LiveAwayScore = 10,
                SpecialLevel = 11,
                StatusId = 12,
                TransId = 100,
                BetId = 101,
                WinlostDate = new DateTime(2016, 8, 10),
                TransDate = new DateTime(2016, 8, 17),
                PlayerComm = 200.1m,
                Comm = 200.2m,
                LicenseeWL = 200.3m,
                Winlost = 200.4m,
                BetEffective = 200.5m,
                Stake = 200.6m,
                ActualStake = 200.7m,
                Odds = 200.8m,
                MasterOdds = 200.9m,
                Handicap1 = 200.11m,
                Handicap2 = 200.12m,
                TransDesc = "TransDescTest",
                Currency = "CurrencyTest"
            };
        }
    }
}