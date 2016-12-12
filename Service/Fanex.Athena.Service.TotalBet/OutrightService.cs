// -----------------------------------------------------------------------
// <copyright file="OutrightService.cs" company="">
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

    public class OutrightService : IOutrightService
    {
        private readonly IOutrightDataAccess _outrightDataAccess;

        public OutrightService()
        {
            _outrightDataAccess = new OutrightDataAccess();
        }

        public OutrightService(IOutrightDataAccess outrightDataAccess)
        {
            _outrightDataAccess = outrightDataAccess;
        }

        public IList<Outright> GetOutrightDataList(OutrightMetaModel metaModel)
        {
            return _outrightDataAccess.GetOutrightDataList(metaModel);
        }
    }
}