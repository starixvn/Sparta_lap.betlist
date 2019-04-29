namespace Fanex.Athena.Module.Dashboard.Data.Interfaces
{
    using System.Threading.Tasks;
    using Fanex.Athena.Module.Dashboard.MetaModel;

    public interface ISingleValueDataAccess
    {
        Task<decimal> GetCustomerBalance(DashboardBaseModel metaModel);

        Task<decimal> GetPendingFunds(DashboardBaseModel metaModel);

        Task<decimal> GetTodayDeposit(DashboardBaseModel metaModel);

        Task<decimal> GetTodayWithdrawal(DashboardBaseModel metaModel);
    }
}