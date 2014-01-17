// -----------------------------------------------------------------------
// <copyright file="OutrightService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.TotalBets
{
    using System.Collections.Generic;
    using DataAccess.Implement.TotalBets;
    using Interface.TotalBets;
    using Models.DTO.TotalBets;
    using Models.MetaModel.TotalBets;

    /// <summary>
    /// Outright Service class.
    /// </summary>
    public class OutrightService : IOutrightService
    {
        #region Fields

        protected readonly OutrightDataAccess OutrightDataAccess;

        #endregion Fields

        #region Constructor

        public OutrightService()
        {
            OutrightDataAccess = new OutrightDataAccess();
        }

        #endregion Constructor

        public IList<Outright> GetOutrightDataList(OutrightMetaModel metaModel)
        {
            return OutrightDataAccess.GetOutrightDataList(metaModel);
        }
    }
}