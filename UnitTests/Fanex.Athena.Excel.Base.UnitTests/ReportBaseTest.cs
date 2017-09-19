namespace Fanex.Athena.Excel.Base.UnitTests
{
    using System.Collections.Generic;
    using Entities;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ReportBaseTest
    {
        [Test]
        public void Export_WhenCall_AssignDataMethodIsCalled()
        {
            // Arrange
            var report = Substitute.ForPartsOf<ReportBase>("ExportFileName", new ShowingOption(), 0);
            report.When(x => x.AssignData()).DoNotCallBase();
            report.When(x => x.LoadTemplate()).DoNotCallBase();
            report.When(x => x.RemoveTemplate()).DoNotCallBase();

            // Action
            report.Export();

            // Assert
            report.Received().LoadTemplate();
            report.Received().AssignData();
            report.Received().RemoveTemplate();
        }
    }
}