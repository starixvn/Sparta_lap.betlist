// -----------------------------------------------------------------------
// <copyright file="CurrencyUser.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Users
{
    /// <summary>
    /// CurrencyUser class.
    /// </summary>
    public class CurrencyUser
    {
        public CurrencyUser()
        {
            CurrencyId = 0;
        }

        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public bool IsActive { get; set; }
    }
}