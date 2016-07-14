// -----------------------------------------------------------------------
// <copyright file="CurrencyUser.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.DTO.Users
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