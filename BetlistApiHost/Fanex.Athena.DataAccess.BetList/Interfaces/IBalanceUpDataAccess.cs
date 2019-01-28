namespace Fanex.Athena.DataAccess.BetList.Interfaces
{
    using Fanex.Athena.Model.Core.DTO.Configuration;

    public interface IBalanceUpDataAccess
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Reviewed. Suppression is OK here.")]
        BalanceUp GetBalanceUp();
    }
}