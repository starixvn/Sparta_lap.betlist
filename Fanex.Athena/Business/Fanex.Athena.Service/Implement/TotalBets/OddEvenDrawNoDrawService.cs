// -----------------------------------------------------------------------
// <copyright file="OddEvenDrawNoDrawService.cs" company="Nexcel">
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