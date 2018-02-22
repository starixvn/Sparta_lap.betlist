using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using NPOI.SS.UserModel;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Excel.BetList.UnitTests
{
    [TestFixture]
    internal class BetListExcelBuilderExtensionTest
    {
        private IBetListExcelBuilder _builder;
        private IFont _font;

        [SetUp]
        public void Setup()
        {
            _builder = Substitute.For<IBetListExcelBuilder>();
            _font = Substitute.For<IFont>();
        }

        [TestCase(false, "1,000.00\n")]
        [TestCase(true, "1,000.00\n2.99")]
        public void AddCustomerWL_ShowPlayerComm_CallAddCellCorrectly(bool isShowPlayerComm, string expectedValue)
        {
            // Arrange
            var ticket = new Ticket
            {
                Winlost = 1000,
                PlayerComm = 2.99m
            };

            // Action
            BetListExcelBuilderExtension.AddCustomerWL(_builder, ticket, isShowPlayerComm, _font);

            // Assert
            _builder.Received(1).AddCell(Arg.Is<IRichTextString>(x => x.String == expectedValue));
        }

        [TestCase("running", "--")]
        [TestCase("abc", "1,000.00\n2.99")]
        public void AddCustomerWL_Status_CallAddCellCorrectly(string status, string expectedValue)
        {
            // Arrange
            var ticket = new Ticket
            {
                Status = status,
                Winlost = 1000,
                PlayerComm = 2.99m
            };

            // Action
            BetListExcelBuilderExtension.AddCustomerWL(_builder, ticket, true, _font);

            // Assert
            _builder.Received(1).AddCell(Arg.Is<IRichTextString>(x => x.String == expectedValue));
        }
    }
}