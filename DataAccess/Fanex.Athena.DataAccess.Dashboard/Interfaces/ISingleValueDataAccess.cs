namespace Fanex.Athena.DataAccess.Dashboard.Interfaces
{
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Dashboard.MetaModel;

    public interface ISingleValueDataAccess
    {
        Task<decimal> GetCustomerBalance(DashboardBaseModel metaModel);

        Task<decimal> GetPendingFunds(DashboardBaseModel metaModel);

        Task<decimal> GetTodayDeposit(DashboardBaseModel metaModel);

        Task<decimal> GetTodayWithdrawal(DashboardBaseModel metaModel);
    }
}