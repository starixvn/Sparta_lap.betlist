namespace Fanex.Athena.BetList.Service.UnitTests.Extensions
{
    using Fanex.BetList.Core.Entities;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Extensions;
    using UnitTest.Common.Helpers;

    public class TicketHelperExtensionTest
    {
        [TestCase(null)]
        [TestCase(" ")]
        [TestCase(0)]
        public void GetValidOddsTypeNameById_OddTypeIdIsEmpty_ReturnEmptyString(object oddsTypeId)
        {
            // Arrange
            var ticketHelper = Substitute.For<ITicketHelper>();

            // Action
            var result = ticketHelper.GetValidOddsTypeNameById(oddsTypeId);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetValidOddsTypeNameById_OddTypeIdIsNotEmpty_CallGetOddsTypeNameByIdMethod()
        {
            // Arrange
            var ticketHelper = Substitute.For<ITicketHelper>();
            var oddTypeId = FakeHelper.Create<uint>() + 1;

            // Action
            var result = ticketHelper.GetValidOddsTypeNameById(oddTypeId);

            // Assert
            ticketHelper.Received(1).GetOddsTypeNameById(oddTypeId);
        }
    }
}