// -----------------------------------------------------------------------
// <copyright file="OutrightService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.TotalBet
{
    using System.Collections.Generic;
    using DataAccess.TotalBet;
    using Interfaces;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;

    /// <summary>
    /// Outright Service class.
    /// </summary>
    public class OutrightService : IOutrightService
    {
        #region Fields

        private readonly OutrightDataAccess _outrightDataAccess;

        #endregion Fields

        #region Constructor

        public OutrightService()
        {
            _outrightDataAccess = new OutrightDataAccess();
        }

        #endregion Constructor

        public IList<Outright> GetOutrightDataList(OutrightMetaModel metaModel)
        {
            return _outrightDataAccess.GetOutrightDataList(metaModel);
        }
    }
}