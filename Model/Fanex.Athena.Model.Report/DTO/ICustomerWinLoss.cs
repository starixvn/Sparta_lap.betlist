// -----------------------------------------------------------------------
// <copyright file="ICustomerWinLoss.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.DTO
{
    public interface ICustomerWinLoss
    {
        decimal CustomerWinLoss { get; set; }

        decimal CustomerComm { get; set; }

        decimal CustomerTotal { get; }
    }
}