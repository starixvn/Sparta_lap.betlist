// -----------------------------------------------------------------------
// <copyright file="ITurnoverDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    /// <summary>
    ///     The  mobile win loss data access.
    /// </summary>
    public interface ITurnoverDataAccess
    {
        #region Public Methods and Operators

        IList<TurnoverDTO> GetDataList(TurnoverMetaModel metaModel);

        #endregion Public Methods and Operators
    }
}