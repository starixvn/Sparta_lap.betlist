namespace Fanex.Athena.Common.DataAccess.Interfaces
{
    using Model.DTO;

    public interface IBalanceUpDataAccess
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Reviewed. Suppression is OK here.")]
        BalanceUp GetBalanceUp();
    }
}