// -----------------------------------------------------------------------
// <copyright file="CustomerCancelBetItem.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Customer.DTO
{
    /// <summary>
    /// CustomerCancelBetItem class.
    /// </summary>
    public class CustomerCancelBetItem
    {
        public int CustId { get; set; }

        public string UserName { get; set; }

        public string NickName { get; set; }

        public int BetCount { get; set; }
    }
}