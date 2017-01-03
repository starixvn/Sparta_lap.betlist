// -----------------------------------------------------------------------
// <copyright file="OddEvenDrawNoDrawService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.TotalBet
{
    using System.Collections.Generic;
    using DataAccess.TotalBet;
    using DataAccess.TotalBet.Interfaces;
    using Interfaces;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;

    public class OddEvenDrawNoDrawService : IOddEvenDrawNoDrawService
    {
        private readonly IOddEvenDrawNoDrawDataAccess _oddEvenDrawNoDrawDataAccess;

        public OddEvenDrawNoDrawService()
        {
            _oddEvenDrawNoDrawDataAccess = new OddEvenDrawNoDrawDataAccess();
        }

        public OddEvenDrawNoDrawService(IOddEvenDrawNoDrawDataAccess oddEvenDrawNoDrawDataAccess)
        {
            _oddEvenDrawNoDrawDataAccess = oddEvenDrawNoDrawDataAccess;
        }

        public IList<OddEvenDrawNoDraw> GetOddEvenDrawNoDrawDataList(OddEvenDrawNoDrawMetaModel metaModel, out OddEvenDrawNoDraw totalData)
        {
            return _oddEvenDrawNoDrawDataAccess.GetOddEventDrawNoDrawDataList(metaModel, out totalData);
        }
    }
}