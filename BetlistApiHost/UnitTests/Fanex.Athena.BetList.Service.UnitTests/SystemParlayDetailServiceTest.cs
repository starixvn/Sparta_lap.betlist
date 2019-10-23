using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanex.Athena.BetList.DataAccess.Interfaces;
using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Athena.BetList.Model.ViewModel;
using Fanex.Athena.Common.Utility.Services.Interfaces;
using Fanex.Athena.UnitTest.Common.Helpers;
using Fanex.BetList.Core.Entities;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.BetList.Service.UnitTests
{
    public class SystemParlayDetailServiceTest
    {
        private const decimal ExpectedOutstanding = 50;

        [Test]
        public async Task BuildHtmlContent_WhenCall_BuilderCalledWithCorrectViewModel()
        {
            // Arrange
            var metaModel = FakeHelper.Create<SystemParlayDetailMetaModel>();
            var dataAccess = Substitute.For<ISystemParlayDetailDataAccess>();
            var expectedData = CreateFakeData();
            dataAccess.GetData(metaModel).Returns(Task.FromResult(expectedData));

            var ticketHelper = Substitute.For<ITicketHelper>();
            const string ViewPath = "ViewPath";

            const string ResultContent = "ResultContent";
            SystemParlayDetailViewModel viewModel = null;
            var templateBuilder = Substitute.For<ITemplateBuilder>();
            templateBuilder.BuildContent(ViewPath, Arg.Any<SystemParlayDetailViewModel>())
                .Returns(x =>
                {
                    viewModel = x.ArgAt<SystemParlayDetailViewModel>(1);

                    return ResultContent;
                });

            var service = new SystemParlayDetailService(dataAccess, templateBuilder);

            // Action
            var result = await service.BuildHtmlContent(metaModel, ticketHelper, ViewPath);
            var dataList = viewModel.DataList.ToList();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(ResultContent));

                Assert.That(dataList[0].Status.ToLowerInvariant(), Is.EqualTo(BetStatus.Won));
                Assert.That(dataList[1].Status.ToLowerInvariant(), Is.EqualTo(BetStatus.Lose));
                Assert.That(dataList[2].Status.ToLowerInvariant(), Is.EqualTo(BetStatus.Draw));
                Assert.That(dataList[3].Status.ToLowerInvariant(), Is.EqualTo(string.Empty));

                Assert.That(viewModel.Outstanding, Is.EqualTo(ExpectedOutstanding));
                Assert.That(viewModel.ShowPlayerComm, Is.EqualTo(metaModel.IsAllowedCustomerComm));
            });
        }

        private static SystemParlayDetailData CreateFakeData()
        {
            var ticket1 = FakeHelper.Create<SystemParlayDetailTicket>();
            ticket1.Serial = 1;
            ticket1.StatusId = Convert.ToInt32(BetStatusId.Lost);

            var ticket2 = FakeHelper.Create<SystemParlayDetailTicket>();
            ticket2.Serial = 2;
            ticket2.StatusId = Convert.ToInt32(BetStatusId.Won);

            var ticket3 = FakeHelper.Create<SystemParlayDetailTicket>();
            ticket3.Serial = 3;
            ticket3.StatusId = Convert.ToInt32(BetStatusId.Draw);

            var unknownStatusTicket = FakeHelper.Create<SystemParlayDetailTicket>();
            unknownStatusTicket.Serial = 4;
            unknownStatusTicket.StatusId = 999;

            var serial1 = FakeHelper.Create<SystemParlayDetailSerial>(x => x.Serial, 1);
            var serial2 = FakeHelper.Create<SystemParlayDetailSerial>(x => x.Serial, 2);
            var serial3 = FakeHelper.Create<SystemParlayDetailSerial>(x => x.Serial, 3);
            var serial4 = FakeHelper.Create<SystemParlayDetailSerial>(x => x.Serial, 4);

            return new SystemParlayDetailData
            {
                Outstanding = ExpectedOutstanding,
                TicketList = new List<SystemParlayDetailTicket> { ticket1, unknownStatusTicket, ticket2, ticket3 },
                SerialList = new List<SystemParlayDetailSerial> { serial1, serial2, serial3, serial4 }
            };
        }
    }
}