namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Core.UnitTests.TestHelpers;
    using Model.Report.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class SubsidiaryDataAccessTest : BaseDataAccessHelperTest<SubsidiaryDataAccess>
    {
        protected override void SetUpDataAccess()
        {
            DataAccess = Substitute.ForPartsOf<SubsidiaryDataAccess>();
        }

        [Test]
        public void GetSubsidiaryList_WhenCall_PassCorrectParams()
        {
            // Arrange
            const string SpName = "Lrf_SubsidiarySel";
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);
            Reader.Read().Returns(false);

            const string LisenseeName = "12Bet";

            // Action
            DataAccess.GetSubsidiaryList(LisenseeName);

            // Assert
            ObjectDb.Received(1).AddInParameter("LicenseeName", DbType.String, LisenseeName);
        }

        [Test]
        public void GetOperatorList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            const string SpName = "Lrf_SubsidiarySel";
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);
            Reader.Read().Returns(true, false);

            const string LisenseeName = "12Bet";
            const int AgentId = 1234;
            const int CurrencyId = 13;
            const string OperatorName = "India";

            var readerDataMapping = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "AgentID", AgentId },
                { "OperatorID", OperatorName },
                { "Currency", CurrencyId }
            };
            DataAccessTestHelper.MapReaderData(Reader, readerDataMapping);

            var expectedData = new List<Subsidiary>
            {
                new Subsidiary
                {
                    AgentId = AgentId,
                    SubsidiaryName = OperatorName,
                    CurrencyId = CurrencyId
                }
            };
            var expectedResult = ObjectHelper.ToJson(expectedData);

            // Action
            var result = ObjectHelper.ToJson(DataAccess.GetSubsidiaryList(LisenseeName));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}