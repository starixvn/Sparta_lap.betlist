// -----------------------------------------------------------------------
// <copyright file="ResultViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.ViewModel
{
    /// <summary>
    /// ResultViewModel class.
    /// </summary>
    public class ResultViewModel
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public object ExtendedData { get; set; }
    }
}