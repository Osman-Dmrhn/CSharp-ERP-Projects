using ProductionAndStockERP.Dtos.ProductDtos;
using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Models;

namespace ProductionAndStockERP.Interfaces
{
    public interface IProductService
    {
        Task<ResponseHelper<PagedResponse<ProductDto>>> GetAllProductsAsync(ProductFilterParameters filters);
        Task<ResponseHelper<ProductDto>> GetProductByIdAsync(int id);
        Task<ResponseHelper<ProductDto>> CreateProductAsync(Product product, int performingUserId);
        Task<ResponseHelper<ProductDto>> UpdateProductAsync(Product product, int performingUserId);
        Task<ResponseHelper<bool>> DeleteProductAsync(int id, int performingUserId);
    }
}
