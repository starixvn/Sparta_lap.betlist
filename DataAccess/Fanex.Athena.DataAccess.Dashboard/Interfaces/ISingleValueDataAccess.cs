namespace Fanex.Athena.DataAccess.Dashboard.Interfaces
{
    using Fanex.Athena.Model.Dashboard.MetaModel;

    public interface ISingleValueDataAccess
    {
        decimal GetCustomerBalance(DashboardBaseModel metaModel);

        decimal GetPendingFunds(DashboardBaseModel metaModel);

        decimal GetTodayDeposit(DashboardBaseModel metaModel);

        decimal GetTodayWithdrawal(DashboardBaseModel metaModel);
    }
}