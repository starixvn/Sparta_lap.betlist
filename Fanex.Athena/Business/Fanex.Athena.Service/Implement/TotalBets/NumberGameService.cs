// -----------------------------------------------------------------------
// <copyright file="NumberGameService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.TotalBets
{
    using System.Collections.Generic;
    using DataAccess.Implement.TotalBets;
    using Fanex.Athena.DataAccess.Interface.TotalBets;
    using Interface.TotalBets;
    using Models.DTO.TotalBets;
    using Models.MetaModel.TotalBets;

    /// <summary>
    /// Number game service class.
    /// </summary>
    public class NumberGameService : INumberGameService
    {
        #region Field

        protected readonly INumberGameDataAccess NumberGameDataAccess;

        #endregion Field

        #region Constructor

        public NumberGameService()
        {
            NumberGameDataAccess = new NumberGameDataAccess();
        }

        #endregion Constructor

        public IList<NumberGame> GetNumberGameDataList(NumberGameMetaModel metaModel)
        {
            return NumberGameDataAccess.GetNumBerGameDataList(metaModel);
        }
    }
}