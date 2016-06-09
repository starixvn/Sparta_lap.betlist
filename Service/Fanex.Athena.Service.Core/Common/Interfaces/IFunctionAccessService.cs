// -----------------------------------------------------------------------
// <copyright file="IFunctionAccessService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Common.Interfaces
{
    public interface IFunctionAccessService
    {
        bool CanViewTotalBets { get; }

        bool CanViewForecasts { get; }

        bool CanViewReports { get; }

        bool CanViewDataWarehouse { get; }

        bool CanManageMember { get; }

        bool CanManageAccount { get; }
    }
}