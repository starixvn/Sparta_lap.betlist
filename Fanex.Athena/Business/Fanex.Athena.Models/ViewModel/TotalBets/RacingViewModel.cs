// -----------------------------------------------------------------------
// <copyright file="RacingViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.TotalBets
{
    using DTO.TotalBets;
    using MetaModel.TotalBets;

    /// <summary>
    /// RacingViewModel class.
    /// </summary>
    public class RacingViewModel : TotalBetsBaseViewModel
    {
        public RacingMetaModel Param { get; set; }

        public Racing MainData { get; set; }
    }
}