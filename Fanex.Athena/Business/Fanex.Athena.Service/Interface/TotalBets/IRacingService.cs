// -----------------------------------------------------------------------
// <copyright file="IRacingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.TotalBets
{
    using Models.DTO.TotalBets;
    using Models.MetaModel.TotalBets;

    public interface IRacingService
    {
        Racing GetRacingDataList(RacingMetaModel metaModel);
    }
}