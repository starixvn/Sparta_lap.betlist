// -----------------------------------------------------------------------
// <copyright file="OddEvenDrawNoDrawService.cs" company="">
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
    /// TODO: Update class.
    /// </summary>
    public class OddEvenDrawNoDrawService : IOddEvenDrawNoDrawService
    {
        #region Fields

        private readonly OddEvenDrawNoDrawDataAccess _oddEvenDrawNoDrawDataAccess;

        #endregion Fields

        #region Constructor

        public OddEvenDrawNoDrawService()
        {
            _oddEvenDrawNoDrawDataAccess = new OddEvenDrawNoDrawDataAccess();
        }

        #endregion Constructor

        public IList<OddEvenDrawNoDraw> GetOddEvenDrawNoDrawDataList(OddEvenDrawNoDrawMetaModel metaModel, out OddEvenDrawNoDraw totalData)
        {
            return _oddEvenDrawNoDrawDataAccess.GetOddEventDrawNoDrawDataList(metaModel, out totalData);
        }
    }
}