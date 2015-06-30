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

        private readonly INumberGameDataAccess _numberGameDataAccess;

        #endregion Field

        #region Constructor

        public NumberGameService()
        {
            _numberGameDataAccess = new NumberGameDataAccess();
        }

        #endregion Constructor

        public IList<NumberGame> GetNumberGameDataList(NumberGameMetaModel metaModel, out NumberGame totalData)
        {
            return _numberGameDataAccess.GetNumBerGameDataList(metaModel, out totalData);
        }
    }
}