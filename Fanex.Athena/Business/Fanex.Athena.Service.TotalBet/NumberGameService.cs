// -----------------------------------------------------------------------
// <copyright file="NumberGameService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.TotalBet
{
    using System.Collections.Generic;
    using DataAccess.TotalBet;
    using Fanex.Athena.DataAccess.TotalBet.Interfaces;
    using Interfaces;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;

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