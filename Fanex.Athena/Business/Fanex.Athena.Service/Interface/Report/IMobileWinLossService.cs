// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMobileWinLossService.cs" company="Nexcel">
// The  mobile win loss service.
// </copyright>
// <summary>
//   The  mobile win loss service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    /// <summary>
    ///     The  mobile win loss service.
    /// </summary>
    public interface IMobileWinLossService
    {
        #region Public Methods and Operators

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        IList<MobileWinLoss> GetMasterData(MobileWinLossMetaModel metaModel, out Dictionary<string, MobileWinLoss> summaryTotal);

        IList<MobileWinLoss> TotalCalc(IList<MobileWinLoss> dataList);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "Reviewed. Suppression is OK here.")]
        IDictionary<string, IList<MobileWinLoss>> ReStructuredData(IList<MobileWinLoss> dataList);

        #endregion Public Methods and Operators
    }
}