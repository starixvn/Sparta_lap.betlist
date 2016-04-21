// -----------------------------------------------------------------------
// <copyright file="LogInfoViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Customer
{
    using Fanex.Athena.Model.Customer.DTO;
    using Model.Core.ViewModel;

    /// <summary>
    /// LogInfoViewModel class.
    /// </summary>
    public class LogInfoViewModel : BaseViewModel
    {
        public LogInfo LogInfo { get; set; }
    }
}