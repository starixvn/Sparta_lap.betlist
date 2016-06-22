// -----------------------------------------------------------------------
// <copyright file="RacingViewModel.cs" company="">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.TotalBet.ViewModel
{
    using DTO;
    using MetaModel;

    /// <summary>
    /// RacingViewModel class.
    /// </summary>
    public class RacingViewModel : TotalBetsBaseViewModel
    {
        public RacingMetaModel Param { get; set; }

        public Racing MainData { get; set; }
    }
}