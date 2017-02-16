namespace Fanex.Athena.Model.Dashboard.ViewModel
{
    public class RegisteredCustomerViewModel
    {
        public int DepositCount { get; set; }

        public int NonDepositCount { get; set; }

        public int Total => DepositCount + NonDepositCount;
    }
}
