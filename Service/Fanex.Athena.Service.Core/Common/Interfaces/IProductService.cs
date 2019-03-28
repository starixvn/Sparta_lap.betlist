namespace Fanex.Athena.Service.Core.Common.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IProductService
    {
        Task<ProductAccess> GetProductAccess(int licenseeId, string languageCode, string functionConfigKey = null);

        Task<IList<Product>> GetProductList(int licenseeId, string languageCode);

        Task<ProductAccess> GetProductAccessForActiveCustomerSummary(int licenseeId, string languageCode);

        Task<ProductAccess> GetSingleWalletProductAccess(int licenseeId, string languageCode, string functionConfigKey = null);
    }
}