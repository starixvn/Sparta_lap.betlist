namespace Fanex.Athena.Api.Report.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Excel.Base.Entities;
    using Excel.Report;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Model.Core.MetaModel.Product;
    using Model.Report.DTO;
    using Model.Report.ViewModel;
    using NSubstitute;
    using NUnit.Framework;

    public partial class PlatformWinLossReportControllerTest
    {
        [TestCase(true, new[] { Product.BA, Product.RB, Product.NG, Product.LC }, new[] { Product.BA, Product.RB })]
        [TestCase(false, new[] { Product.SB, Product.VS, Product.CS, Product.KN }, new[] { Product.RB })]
        [TestCase(false, new string[0], new string[0])]
        public void Index_ExportExcelMemberWithLevel1_PassCorrectParamsToCreateExcel(
            bool hasLicenseeComm,
            string[] productOptions,
            string[] productAccess)
        {
            // Arrange
            StubLicenseeSessionData(new LicenseeSession
            {
                LicenseeId = 1,
                IsAllowedCustomerComm = hasLicenseeComm
            });
            var metalModel = CreateMetaModel(1, true);
            metalModel.ProductOptions = SetUpProductOption(productOptions);

            StubDataAccessGetMemberData();
            StubProductAccess(productAccess);

            ShowingOption expetedShowingOption = null;
            var actualExcelModel = new PlatformWinLossViewModel();
            var excelModel = new PlatformWinLossViewModel { Param = metalModel };
            var excel = Substitute.ForPartsOf<PlatformWinLossMemberExcel>(
               Arg.Any<string>(),
               excelModel,
               Arg.Do<ShowingOption>(x => expetedShowingOption = x));
            excel.When(x => x.Export()).DoNotCallBase();
            
            _controller.CreateExcelMember(
                Arg.Any<string>(),
                excelModel,
                Arg.Any<ShowingOption>()).ReturnsForAnyArgs(excel);

            var expectedColSpan = GetExpectedColSpan(productAccess);

            // Action
            _controller.Index(metalModel);

            // Assert
            Assert.IsTrue(expetedShowingOption.CustomerCommVisible == hasLicenseeComm);
            VerifyShowingOption(metalModel.ProductOptions, expetedShowingOption);
            _controller.Received().CreateExcelMember(Arg.Any<string>(), Arg.Is<PlatformWinLossViewModel>(p => p.Colspan == expectedColSpan), Arg.Any<ShowingOption>());
        }

        [Test]
        public void Index_ExportExcelSummaryWithLevel0_PassCorrectParamsToCreateExcel()
        {
            // Arrange
            var metalModel = CreateMetaModel(0, true);
            SetUpDataAccessGetSummaryData();

            var excel = Substitute.ForPartsOf<PlatformWinLossSummaryExcel>(
                null,
                metalModel,
                Arg.Any<IList<PlatformWinLossSummary>>(),
                Arg.Any<PlatformWinLossSummary>());

            excel.When(x => x.Export()).DoNotCallBase();

            _controller.CreateExcelSummary(
                null,
                metalModel,
                Arg.Any<IList<PlatformWinLossSummary>>(),
                Arg.Any<PlatformWinLossSummary>())
                .Returns(excel);

            // Action
            _controller.Index(metalModel);

            // Assert
            Assert.Multiple(() =>
            {
                _controller.Received().CreateExcelSummary(
                    Arg.Any<string>(),
                    metalModel,
                    Arg.Is<IList<PlatformWinLossSummary>>(x => x.Count == 2),
                    Arg.Is<PlatformWinLossSummary>(x => x.CustomerComm == ExpectedCustomerComm));
                Assert.That(metalModel.AgentListXml, Is.EqualTo(AgentListXml));
            });
        }

        private static IList<ProductOption> SetUpProductOption(string[] productCodes)
        {
            var productOptions = new List<string>
            {
                Product.BA,
                Product.RB,
                Product.NG,
                Product.LC,
                Product.SB,
                Product.VS,
                Product.CS,
                Product.KN
            };

            return productOptions.Select(p => new ProductOption { Code = p, Status = productCodes.Contains(p) }).ToList();
        }

        private static void VerifyShowingOption(IEnumerable<ProductOption> productOptions, ShowingOption expetedShowingOption)
        {
            Assert.Multiple(() =>
            {
                Assert.IsTrue(expetedShowingOption.SBVisible == productOptions.Any(i => i.Code == Product.SB && i.Status));
                Assert.IsTrue(expetedShowingOption.BAVisible == productOptions.Any(i => i.Code == Product.BA && i.Status));
                Assert.IsTrue(expetedShowingOption.BGVisible == productOptions.Any(i => i.Code == Product.BG && i.Status));
                Assert.IsTrue(expetedShowingOption.BTVisible == productOptions.Any(i => i.Code == Product.BT && i.Status));
                Assert.IsTrue(expetedShowingOption.CSVisible == productOptions.Any(i => i.Code == Product.CS && i.Status));
                Assert.IsTrue(expetedShowingOption.KNVisible == productOptions.Any(i => i.Code == Product.KN && i.Status));
                Assert.IsTrue(expetedShowingOption.LCVisible == productOptions.Any(i => i.Code == Product.LC && i.Status));
                Assert.IsTrue(expetedShowingOption.NGVisible == productOptions.Any(i => i.Code == Product.NG && i.Status));
                Assert.IsTrue(expetedShowingOption.RBVisible == productOptions.Any(i => i.Code == Product.RB && i.Status));
                Assert.IsTrue(expetedShowingOption.VSVisible == productOptions.Any(i => i.Code == Product.VS && i.Status));
            });
        }

        private static int GetExpectedColSpan(string[] productAccess)
        {
            var expectedColSpan = 7;

            if (productAccess.Contains(Product.BA))
            {
                expectedColSpan += 2;
            }

            if (productAccess.Contains(Product.RB))
            {
                expectedColSpan += 2;
            }

            return expectedColSpan;
        }
    }
}