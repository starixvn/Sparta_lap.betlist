// -----------------------------------------------------------------------
// <copyright file="IBalanceDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public interface IBalanceDataAccess
    {
        Balance GetReportData(BalanceMetaModel metaModel);
    }
}