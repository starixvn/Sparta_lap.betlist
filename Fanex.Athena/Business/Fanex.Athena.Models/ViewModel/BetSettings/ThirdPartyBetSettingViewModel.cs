// -----------------------------------------------------------------------
// <copyright file="ThirdPartyBetSettingViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.BetSettings
{
    /// <summary>
    /// ThirdPartyBetSettingViewModel class.
    /// </summary>
    public class ThirdPartyBetSettingViewModel : BetSettingViewModel
    {
        public ThirdPartyBetSettingViewModel() 
        {           
        }

        public bool IsLineEnabled { get; set; }
    }
}